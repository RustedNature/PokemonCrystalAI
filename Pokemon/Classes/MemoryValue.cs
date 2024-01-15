using PokeTorchAi.Pokemon.Enums;

namespace PokeTorchAi.Pokemon.Classes
{
    public class MemoryValue
    {
        public MemoryAddresses MemAddress { get; set; }
        public int MemValue { get; set; }

        public MemoryValue(int memAddress, int memValue)
        {
            MemAddress = (MemoryAddresses)memAddress;
            MemValue = memValue;
        }
    }
}