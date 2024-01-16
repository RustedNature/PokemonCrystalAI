
using TorchSharp;



namespace PokeTorchAi.Ai;



public class Agent(
    int numActions,
    int batchSize = 1,
    float discountFactor = 0.99f,
    float epsilon = 1.0f,
    float epsilonDecay = 0.995f,
    float minEpsilon = 0.01f)
{
    private int _updateCounter = 0;
    private const string ModelPath = "MODEL";
    private const string ModelName = "PokeNet.pt";
    private static readonly Random Rng = new Random();
    private readonly ExperienceReplay _replayMemory = new ExperienceReplay(100_000);
    private PokeAi _model = new PokeAi("PokeNetModel", numActions, 1).cuda();
    private PokeAi _targetModel = new PokeAi("PokeNetTarget", numActions, 1).cuda();


    public long SelectAction(torch.Tensor stateImage)
    {
        long action = 0;
        if (Rng.NextDouble() > epsilon)
        {
            _model.eval();
            var withNoGrad = torch.no_grad();
            var qValues = _model.forward(stateImage.cuda());
            withNoGrad.Dispose();
            _model.train();
            action = qValues.argmax().item<long>();
            Console.WriteLine("PREDICT");
        }
        else
        {
            action = Rng.Next(0, numActions);
            Console.WriteLine("RANDOM");
        }

        return action;
    }

    

    public void UpdateModel()
    {
        if (_replayMemory.GetMemoryCount() < batchSize)
        {
            return;
        }
        using var optimizer = torch.optim.Adam(_model.parameters());
        optimizer.zero_grad();
        var experiences = _replayMemory.Sample(batchSize);
        foreach (var (state, action, reward, nextState) in experiences)
        {
            
            var currentQ = _model.forward(state.cuda())[0, action];
            var maxNextQ = _targetModel.forward(nextState.cuda()).max().item<float>();
            var expectedQ = reward + (discountFactor * maxNextQ);

            var loss = torch.nn.functional.mse_loss(currentQ.cuda(), torch.tensor(expectedQ).cuda());
            loss.backward();
        }

        optimizer.step();
        epsilon = Math.Max(minEpsilon, epsilon * epsilonDecay);
        Console.WriteLine(epsilon);
        ++_updateCounter;
        UpdateTargetModel();
    }

    private void UpdateTargetModel()
    {
        if ((_updateCounter + 1) % 10_000 != 0) return;
        var cpuModel = _model.cpu();
        var cpuTargetModel = _targetModel.cpu();

        cpuTargetModel.load_state_dict(cpuModel.state_dict());

        Console.WriteLine("Model parameters copied.");

        _model = cpuModel.cuda();
        _targetModel = cpuTargetModel.cuda();

        Console.WriteLine("Models moved back to GPU.");
    }

    public void UpdateExperienceMemory(torch.Tensor state, int action, float reward, torch.Tensor nextState)
    {
        _replayMemory.Add(state, action, reward, nextState);
    }

    public void SaveModel()
    {
        using var fs = new FileStream(ModelPath + "/" + ModelName, FileMode.Create);
        _model.save(fs);
        using var fs2 = new FileStream(ModelPath + "/" + ModelName + "Target", FileMode.Create);
        _targetModel.save(fs2);

    }

    public bool LoadModel()
    {
        if (!Directory.Exists(ModelPath + "/"))
        {
            Directory.CreateDirectory(ModelPath + "/");
            return false;
        }

        if (!File.Exists(ModelPath + "/" + ModelName)) return false;

        using var fs = new FileStream(ModelPath + "/" + ModelName, FileMode.Open);
        _model.load(fs);
        using var fs2 = new FileStream(ModelPath + "/" + ModelName + "Target", FileMode.Open);
        _targetModel.load(fs2);
        return true;
    }
}