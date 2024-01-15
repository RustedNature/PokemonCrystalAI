using System.IO.Pipes;
using System.Text;

namespace PokeTorchAi.PyBoyInteract;

public class PipeServer
{
    private readonly NamedPipeServerStream? _server = new("PokePipe", PipeDirection.InOut, 1);
    private readonly byte[] _readBuffer = new byte[69_120];

    public void StartServer()
    {
        Console.WriteLine("Waiting for client connection...");
        _server!.WaitForConnection();
        Console.WriteLine("Client connected.");
    }

    public void SendMovementData(long movement)
    {
        
        _server!.Write(Encoding.UTF8.GetBytes(movement.ToString()));
        _server.Flush();
    }

    public byte[] GetData()
    {
        Array.Clear(_readBuffer, 0, _readBuffer.Length);
        var read = _server!.Read(_readBuffer, 0, _readBuffer.Length);
        return _readBuffer;
    }

    public void StopServer()
    {
        if (_server!.IsConnected)
        {
            _server.Disconnect();
        }

        _server!.Close();
        _server!.Dispose();
    }
}