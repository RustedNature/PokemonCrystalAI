﻿using Newtonsoft.Json;
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
    private static torch.Tensor? tensorImageCurrent = null;
    private static int epoch = 0;
    private static Process? pyBoyProcess;
    private const int BatchSize = 4;

    private static void Main()
    {
        EpsilonDecayTrainingEveryWhile();
        RandomTrainingOnly();
        PredictOnly();
    }
    private static void RandomTrainingOnly()
    {
        while (true)
        {

            SetupPreRequirements(
                out RewardManager rewardManager,
                out Agent agent,
                out PipeServer pipeServer,
               batchSize: BatchSize,
                1f);

            bool done = false;

            while (done is false)
            {
                var tensorImageBefore = tensorImageCurrent;
                var byteImageCurrent = GetCurrentImageAsBytes(pipeServer);
                tensorImageCurrent = CropAndNormalize(byteImageCurrent);

                rewardManager.RefreshMemory(GetCurrentMemoryValues(pipeServer));

                var lastMovement = agent.SelectAction(tensorImageCurrent);
                pipeServer.SendMovementData(lastMovement);

                if (tensorImageBefore is not null)
                {
                    agent.UpdateExperienceMemory(tensorImageBefore!, (int)lastMovement, rewardManager.GetReward(), tensorImageCurrent);

                }
                Console.WriteLine(++epoch);
                if (epoch % 1_000 == 0)
                {
                    Console.WriteLine($"Step: {epoch}");
                }

                if (epoch % 100_000 == 0)
                {
                    Console.WriteLine($"Train for {(int)(agent.GetMemoryCount() / 2)}");
                    for (int i = 0; i < agent.GetMemoryCount() / 2; i++)
                    {
                        if (i % 1000 == 0)
                        {
                            Console.WriteLine($"TrainStep: {i} of {(int)(agent.GetMemoryCount() / 2)}");
                            agent.SaveModel();
                        }
                        agent.UpdateModel();
                    }

                    agent.SaveModel();
                    done = true;
                    Environment.Exit(0);
                }
            }
        }
    }

    private static void PredictOnly()
    {
        SetupPreRequirements(
            out RewardManager rewardManager,
            out Agent agent,
            out PipeServer pipeServer);
        while (true)
        {
            var byteImageCurrent = GetCurrentImageAsBytes(pipeServer);
            tensorImageCurrent = CropAndNormalize(byteImageCurrent);

            rewardManager.RefreshMemory(GetCurrentMemoryValues(pipeServer));

            var lastMovement = agent.SelectPredictedActionOnly(tensorImageCurrent);
            pipeServer.SendMovementData(lastMovement);

        }
    }
    private static void EpsilonDecayTrainingEveryWhile()
    {
        SetupPreRequirements(
            out RewardManager rewardManager,
            out Agent agent,
            out PipeServer pipeServer,
            batchSize: BatchSize,
            epsilonDecay: 0.9995f);
        bool restart = false;
        while (restart is false)
        {
            restart = false;
            var tensorImageBefore = tensorImageCurrent;
            var byteImageCurrent = GetCurrentImageAsBytes(pipeServer);
            tensorImageCurrent = CropAndNormalize(byteImageCurrent);

            rewardManager.RefreshMemory(GetCurrentMemoryValues(pipeServer));

            var lastMovement = agent.SelectAction(tensorImageCurrent);
            pipeServer.SendMovementData(lastMovement);

            if (tensorImageBefore is not null)
            {
                agent.UpdateExperienceMemory(tensorImageBefore!, (int)lastMovement, rewardManager.GetReward(), tensorImageCurrent);
                agent.UpdateModel();
            }
            Console.WriteLine(++epoch);
            if (epoch % 1_000 == 0)
            {
                agent.SaveModel();
            }
            if (epoch % 100_000 == 0)
            {
                pipeServer.SendReset(1);
                rewardManager.ResetRewards();
            }
            else
            {
                pipeServer.SendReset(0);
            }


        }
    }

    private static void SetupPreRequirements(out RewardManager rewardManager, out Agent agent, out PipeServer pipeServer, int batchSize = 8, float epsilonDecay = 0.995f)
    {
        rewardManager = new RewardManager();
        agent = new Agent(NumActions, batchSize: batchSize, discountFactor: 0.99f, epsilon: 1.0f, epsilonDecay: epsilonDecay,
                    minEpsilon: 0.01f);
        if (agent.LoadModel())
        {
            Console.WriteLine("Existing model loaded");
        }

        pipeServer = new PipeServer();

        StartPyBoy();
        pipeServer.StartServer();

    }

    private static void StartPyBoy()
    {
        Process process = new();

        ProcessStartInfo startInfo = new()
        {
            FileName = "python.exe",
            Arguments = @".\PyBoyRun.py",
            WorkingDirectory = @".\PyBoyInteract\Python\"
        };

        process.StartInfo = startInfo;
        pyBoyProcess = process;
        pyBoyProcess.Start();
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