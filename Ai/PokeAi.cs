using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace PokeTorchAi;

public sealed class PokeAi : Module<Tensor, Tensor>
{
    private readonly Module<Tensor, Tensor> _features;
    private readonly Module<Tensor, Tensor> _linearLayers;

    public PokeAi(string name, int numActions, int inputChannels) : base(name)
    {
        _features = Sequential(
            ("c1", Conv2d(inputChannels, 128, kernelSize: 3, stride: 2, padding: 1)),
            ("r1", ReLU(inplace: true)),
            ("mp1", MaxPool2d(kernelSize: new long[] { 2, 2 })),
            ("c2", Conv2d(128, 64, kernelSize: 3, padding: 1)),
            ("r2", ReLU(inplace: true)),
            ("mp2", MaxPool2d(kernelSize: new long[] { 2, 2 }))
        );


        _linearLayers = Sequential(
            ("d1", Dropout()),
            ("l1", Linear(18 * 18 * 64, 1024)),
            ("r1", ReLU(inplace: true)),
            ("d2", Dropout()),
            ("l2", Linear(1024, 562)),
            ("r3", ReLU(inplace: true)),
            ("d3", Dropout()),
            ("l3", Linear(562, numActions))
        );
        RegisterComponents();
    }

    public override Tensor forward(Tensor input)
    {
        var f = _features.forward(input);
        var v = f.view([-1, 64 * 18 * 18]);
        var l = _linearLayers.forward(v);

        return l;
    }

    public static float[,] ReshapeArrayTo2D(float[] flatArray, long[] shape)
    {
        var reshapedArray = new float[shape[0], shape[1]];
        var index = 0;

        for (var i = 0; i < shape[0]; i++)
        {
            for (var j = 0; j < shape[1]; j++)
            {
                reshapedArray[i, j] = flatArray[index++];
            }
        }

        return reshapedArray;
    }
}