using System.Data.SqlTypes;
using TorchSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TorchSharp;
using static TorchSharp.torch.nn;
using static TorchSharp.torch;
using static TorchSharp.TensorExtensionMethods;
using static TorchSharp.torch.distributions;
namespace PokeTorchAi;

class Program
{
    static void Main(string[] args)
    {
        
        Random rng = new Random();
        var cnt = 0;
        var agent = new Agent(numActions: 10, batchSize: 16, discountFactor: 0.99f, epsilon: 1.0f, epsilonDecay: 0.995f, minEpsilon: 0.01f);
        agent.LoadModel();
            for (int i = 0; i < 10_000; i++)
            {
                agent.UpdateExperienceMemory(  torch.rand(1,144,144), rng.Next(0,10), (float)rng.NextDouble(), torch.rand(1,144,144));
            }
        while (cnt != 500)
        {

            Console.WriteLine(agent.SelectAction(torch.rand( 1, 144, 144)));
            
            agent.UpdateModel();
            cnt++;
        }
        agent.SaveModel();
    }
    
}