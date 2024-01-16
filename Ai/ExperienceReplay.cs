using TorchSharp;

namespace PokeTorchAi.Ai;

public class ExperienceReplay(int capacity)
{
    private readonly List<(torch.Tensor state, int action, float reward, torch.Tensor nextState)> _memory = [];
    private readonly Random _rng = new();

    public void Add(torch.Tensor state, int action, float reward, torch.Tensor nextState)
    {
        if (_memory.Count >= capacity)
        {
            _memory.RemoveAt(0);
        }
        _memory.Add((state, action, reward, nextState));
    }

    public int GetMemoryCount()
    {
        return _memory.Count;
    }
    public IEnumerable<(torch.Tensor state, int action, float reward, torch.Tensor nextState)> Sample(int batchSize)
    {
        var indices = Enumerable.Range(0, _memory.Count).OrderBy(x => _rng.Next()).Take(batchSize);
        return indices.Select(i => _memory[i]);
    }
}