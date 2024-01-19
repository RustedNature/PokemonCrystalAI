using TorchSharp;



namespace PokeTorchAi.Ai;



public class Agent
{
    private const int InputChannels = 3;
    private const string ModelPath = "MODEL";
    private const string ModelName = "PokeNet.pt";

    private float _epsilon;
    private float _minEpsilon;
    private float _epsilonDecay;
    private float _discountFactor;
    private int _batchSize;
    private int _numActions;
    private PokeAi _model;
    private PokeAi _targetModel;
    private static readonly Random Rng = new Random();
    private readonly ExperienceReplay _replayMemory = new ExperienceReplay(250_000);
    int _updateCounter = 0;


    public Agent(int numActions,
        int batchSize = 1,
        float discountFactor = 0.99f,
        float epsilon = 1.0f,
        float epsilonDecay = 0.995f,
        float minEpsilon = 0.01f)
    {
        _minEpsilon = minEpsilon;
        _epsilonDecay = epsilonDecay;
        _discountFactor = discountFactor;
        _batchSize = batchSize;
        _epsilon = epsilon;
        _numActions = numActions;

        _model = new PokeAi("PokeNetModel", numActions, InputChannels).cuda();
        _targetModel = new PokeAi("PokeNetTarget", numActions, InputChannels).cuda();
    }

    public long SelectAction(torch.Tensor stateImage)
    {
        long action;
        if (torch.rand_float() > _epsilon)
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
            action = torch.randint_int(0, _numActions);
            Console.WriteLine("RANDOM");
        }

        return action;
    }
    public long SelectPredictedActionOnly(torch.Tensor stateImage)
    {

        _model.eval();
        var withNoGrad = torch.no_grad();
        var qValues = _model.forward(stateImage.cuda());
        withNoGrad.Dispose();
        _model.train();
        Console.WriteLine("PREDICT");


        return qValues.argmax().item<long>();
    }

    public int GetMemoryCount()
    {
        return _replayMemory.GetMemoryCount();
    }


    public void UpdateModel()
    {
        if (_replayMemory.GetMemoryCount() < _batchSize)
        {
            return;
        }
        using var optimizer = torch.optim.Adam(_model.parameters());
        optimizer.zero_grad();
        var experiences = _replayMemory.Sample(_batchSize);
        foreach (var (state, action, reward, nextState) in experiences)
        {

            var currentQ = _model.forward(state.cuda())[0, action];
            var maxNextQ = _targetModel.forward(nextState.cuda()).max().item<float>();
            var expectedQ = reward + (_discountFactor * maxNextQ);

            var loss = torch.nn.functional.mse_loss(currentQ.cuda(), torch.tensor(expectedQ).cuda());
            loss.backward();
        }

        optimizer.step();
        _epsilon = Math.Max(_minEpsilon, _epsilon - _epsilonDecay);
        ++_updateCounter;
        UpdateTargetModel();
    }

    private void UpdateTargetModel()
    {
        if ((_updateCounter + 1) % 10_000 != 0) return;
        var cpuModel = _model.cpu();
        var cpuTargetModel = _targetModel.cpu();

        cpuTargetModel.load_state_dict(cpuModel.state_dict());


        _model = cpuModel.cuda();
        _targetModel = cpuTargetModel.cuda();

    }

    public void UpdateExperienceMemory(torch.Tensor state, int action, float reward, torch.Tensor nextState)
    {
        _replayMemory.Add(state, action, reward, nextState);
    }

    public void SaveModel()
    {
        using var fs = new FileStream(ModelPath + "/" + ModelName + ".MoreConvs3channel", FileMode.Create);
        _model.save(fs);
        using var fs2 = new FileStream(ModelPath + "/" + ModelName + "Target.MoreConvs3channel", FileMode.Create);
        _targetModel.save(fs2);

    }

    public bool LoadModel()
    {
        if (!Directory.Exists(ModelPath + "/"))
        {
            Directory.CreateDirectory(ModelPath + "/");
            return false;
        }

        if (!File.Exists(ModelPath + "/" + ModelName + ".MoreConvs3channel")) return false;

        using var fs = new FileStream(ModelPath + "/" + ModelName + ".MoreConvs3channel", FileMode.Open);
        _model.load(fs);
        using var fs2 = new FileStream(ModelPath + "/" + ModelName + "Target.MoreConvs3channel", FileMode.Open);
        _targetModel.load(fs2);
        return true;
    }

    internal void SetMinEpsilon(float minEpsilon)
    {
        _minEpsilon = minEpsilon;
    }

    internal void SetEpsilon(float epsilon)
    {
        _epsilon = epsilon;
    }
}