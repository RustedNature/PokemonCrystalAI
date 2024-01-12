using TorchSharp;

namespace PokeTorchAi;

public class Agent(int numActions,int batchSize = 1,float discountFactor = 0.99f,float epsilon = 1.0f, float epsilonDecay = 0.995f, float minEpsilon = 0.01f)
{
    private const string ModelPath = "MODEL";
    private const string ModelName = "PokeNet.pt";
    private static readonly Random Rng = new Random();
    private readonly ExperienceReplay _replayMemory = new ExperienceReplay(10_000);
    private readonly PokeAi _model = new PokeAi("PokeNetModel",numActions).cuda();
    private readonly PokeAi _targetModel = new PokeAi("PokeNetTarget",numActions).cuda();
    
    public long SelectAction(torch.Tensor state)
    {
        long action = 0;
        if (Rng.NextDouble() > epsilon)
        {
            _model.eval();
            var withNoGrad = torch.no_grad();
            var qValues = _model.forward(state.cuda());
            withNoGrad.Dispose();
            _model.train();
            action = qValues.argmax().item<long>();
        }
        else
        {
            action = Rng.Next(0, numActions);
        }

        return action;
    }

    public void UpdateModel()
    {
        using var optimizer = torch.optim.Adam(_model.parameters());
        optimizer.zero_grad();
        var experiences = _replayMemory.Sample(batchSize);
        foreach (var (state,action,reward,nextState) in experiences)
        {
            var currentQ = _model.forward(state.cuda())[0, action];
            var maxNextQ = _targetModel.forward(nextState.cuda()).max().item<float>();
            var expectedQ = reward + (discountFactor * maxNextQ);
            
            var loss = torch.nn.functional.mse_loss(currentQ.cuda(), torch.tensor(expectedQ).cuda());
            loss.backward();
            
        }
        optimizer.step();
        epsilon = Math.Max(minEpsilon, epsilon * epsilonDecay);
    }

    public void UpdateExperienceMemory(torch.Tensor state, int action, float reward, torch.Tensor nextState)
    {
        _replayMemory.Add(state,action,reward,nextState);
    }

    public void SaveModel()
    {
        using var fs = new  FileStream(ModelPath + "/" + ModelName, FileMode.Create);
        _model.save(fs);
    }

    public void LoadModel()
    {
        if (!Directory.Exists(ModelPath + "/")) Directory.CreateDirectory(ModelPath + "/"); return;
        using var fs = new  FileStream(ModelPath + "/" + ModelName, FileMode.Open);
        _model.load(fs);
    }
}