using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace PokeTorchAi;

public sealed class PokeAi : Module<Tensor, Tensor>
{
    private readonly Module<Tensor, Tensor> _features;
    private readonly Module<Tensor, Tensor> _linearLayers;
    const int FirstConvOutput = 128;
    const int SecondConvOutput = 64;
    const int FlatLayerSize = SecondConvOutput * 18 * 18;//18 is the size of the image/features that we want to put in the flat layer


    public PokeAi(string name, int numActions, int inputChannels) : base(name)
    {
        _features = Sequential(
            ("c1", Conv2d(inputChannels, FirstConvOutput, kernelSize: 3, stride: 2, padding: 1)),
            ("r1", ReLU(inplace: true)),
            ("mp1", MaxPool2d(kernelSize: new long[] { 2, 2 })),
            ("c2", Conv2d(FirstConvOutput, SecondConvOutput, kernelSize: 3, padding: 1)),
            ("r2", ReLU(inplace: true)),
            ("mp2", MaxPool2d(kernelSize: new long[] { 2, 2 }))
        );


        _linearLayers = Sequential(
            ("d1", Dropout()),
            ("l1", Linear(FlatLayerSize, 512)),
            ("r1", ReLU(inplace: true)),
            ("d2", Dropout()),
            ("l2", Linear(512, 64)),
            ("r3", ReLU(inplace: true)),
            ("d3", Dropout()),
            ("l3", Linear(64, numActions))
        );
        RegisterComponents();
    }

    public override Tensor forward(Tensor input)
    {
        var f = _features.forward(input);
        var v = f.view([-1, FlatLayerSize]);
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