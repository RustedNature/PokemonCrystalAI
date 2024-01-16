using Newtonsoft.Json;
using PokeTorchAi.Ai;
using PokeTorchAi.Pokemon.Classes;
using PokeTorchAi.PyBoyInteract;
using System.Diagnostics;
using System.Text;
using TorchSharp;


namespace PokeTorchAi;


class Program
{
    private const int NumActions = 8;

    private static void Main(string[] args)
    {
        var rewardManager = new RewardManager();
        var rng = new Random();
        var agent = new Agent(NumActions, batchSize: 8, discountFactor: 0.99f, epsilon: 1.0f, epsilonDecay: 0.9995f,
            minEpsilon: 0.01f);
        if (agent.LoadModel())
        {
            Console.WriteLine("Existing model loaded");
        }


        var pipeServer = new PipeServer();
        torch.Tensor? imageCurrent = null;
        var firstRun = true;
        StartPyBoy();
        pipeServer.StartServer();

        var epoch = 0;
        while (true)
        {
            var imageBefore = imageCurrent;
            var byteImageCurrent = GetCurrentImageAsBytes(pipeServer);
            imageCurrent = CropAndNormalize(byteImageCurrent);

            rewardManager.RefreshMemory(GetCurrentMemoryValues(pipeServer));

            var lastMovement = agent.SelectAction(imageCurrent);
            pipeServer.SendMovementData(lastMovement);

            if (firstRun)
            {
                firstRun = false;
                continue;
            }
            agent.UpdateExperienceMemory(imageBefore!, (int)lastMovement, rewardManager.GetReward(), imageCurrent);
            agent.UpdateModel();
            Console.WriteLine(++epoch);
            if (epoch % 1_000 == 0)
            {
                agent.SaveModel();
            }
        }
    }

    private static void StartPyBoy()
    {
        Process process = new Process();

        // Set the process start info
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "python.exe";
        startInfo.Arguments = @".\PyBoyRun.py";
        startInfo.WorkingDirectory = @".\PyBoyInteract\Python\";
        // Start the process
        process.StartInfo = startInfo;
        process.Start();
    }

    private static torch.Tensor CropAndNormalize(byte[] stateImage)
    {
        var transform =
            torchvision.transforms.Compose([
            torchvision.transforms.CenterCrop(144, 144),
                torchvision.transforms.Grayscale()
        ]);
        return transform.call(torch.tensor(stateImage, torch.ScalarType.Float32).view(3, 160, 144));
    }


    private static List<MemoryValue> GetCurrentMemoryValues(PipeServer pipeServer)
    {
        var byteMemVals = pipeServer.GetData();
        return JsonConvert.DeserializeObject<List<MemoryValue>>(Encoding.UTF8.GetString(byteMemVals))!;
    }

    private static byte[] GetCurrentImageAsBytes(PipeServer pipeServer)
    {
        return pipeServer.GetData();
    }
}