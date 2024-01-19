using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace PokeTorchAi;

public sealed class PokeAi : Module<Tensor, Tensor>
{
    private readonly Module<Tensor, Tensor> _features;
    private readonly Module<Tensor, Tensor> _linearLayers;
    const long InitialImageHeightWidth = 144;

    const long FirstConvOutput = 64;
    const long FirstConvKernel = 4;
    const long FirstConvStride = 2;
    const long FirstConvPadding = 1;
    const long FirstConvGroups = 1;
    const long FirstConvDilation = 1;
    const long FirstMaxPool = 2;

    const long SecondConvOutput = 128;
    const long SecondConvKernel = 4;
    const long SecondConvStride = 1;
    const long SecondConvPadding = 0;
    const long SecondConvGroups = 1;
    const long SecondConvDilation = 1;
    const long SecondMaxPool = 2;


    const long ThirdConvOutput = 256;
    const long ThirdConvKernel = 4;
    const long ThirdConvStride = 1;
    const long ThirdConvPadding = 1;
    const long ThirdConvGroups = 1;
    const long ThirdConvDilation = 1;
    //const long ThirdMaxPool = 2;


    const long ImageDimSizeAfterConv1 = ((InitialImageHeightWidth + 2 * FirstConvPadding - FirstConvDilation * (FirstConvKernel - 1) - 1) / FirstConvStride) + 1;
    const long ImageDimSizeAfterConv2 = ((ImageDimSizeAfterConv1 / FirstMaxPool + 2 * SecondConvPadding - SecondConvDilation * (SecondConvKernel - 1) - 1) / SecondConvStride) + 1;
    const long ImageDimSizeAfterConv3 = ((ImageDimSizeAfterConv2 / SecondMaxPool + 2 * ThirdConvPadding - FirstConvDilation * (ThirdConvKernel - 1) - 1) / ThirdConvStride) + 1;

    const long FlatLayerSize = ThirdConvOutput * ImageDimSizeAfterConv3 * ImageDimSizeAfterConv3;
    private const int Linear1OutputSize = 750;
    private const int Linear2OutputSize = 500;
    private const float DropoutRate = 0.25f;

    public PokeAi(string name, int numActions, int inputChannels) : base(name)
    {
        _features = Sequential(
            ("c1", Conv2d(inputChannels, FirstConvOutput, kernelSize: FirstConvKernel, stride: FirstConvStride, padding: FirstConvPadding, groups: FirstConvGroups, dilation: FirstConvDilation)),
            ("r1", ReLU(inplace: true)),
            ("mp1", MaxPool2d(kernelSize: [FirstMaxPool, FirstMaxPool])),
            ("c2", Conv2d(FirstConvOutput, SecondConvOutput, kernelSize: SecondConvKernel, stride: SecondConvStride, padding: SecondConvPadding, groups: SecondConvGroups, dilation: SecondConvDilation)),
            ("r2", ReLU(inplace: true)),
            ("mp2", MaxPool2d(kernelSize: [SecondMaxPool, SecondMaxPool])),
            ("c3", Conv2d(SecondConvOutput, ThirdConvOutput, kernelSize: ThirdConvKernel, stride: ThirdConvStride, padding: ThirdConvPadding, groups: ThirdConvGroups, dilation: ThirdConvDilation)),
            ("r3", ReLU(inplace: true))
        //("mp3", MaxPool2d(kernelSize: [ThirdMaxPool, ThirdMaxPool]))
        );




        _linearLayers = Sequential(
            ("d1", Dropout(DropoutRate)),
            ("l1", Linear(FlatLayerSize, Linear1OutputSize)),
            ("r1", ReLU(inplace: true)),
            ("d2", Dropout(DropoutRate)),
            ("l2", Linear(Linear1OutputSize, Linear2OutputSize)),
            ("r3", ReLU(inplace: true)),
            ("d3", Dropout(DropoutRate)),
            ("l3", Linear(Linear2OutputSize, numActions))
        );
        RegisterComponents();
    }

    public override Tensor forward(Tensor input)
    {
        var f = _features.forward(input.cuda()).cuda();
        var v = f.view([-1, FlatLayerSize]).cuda();
        var l = _linearLayers.forward(v.cuda());
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