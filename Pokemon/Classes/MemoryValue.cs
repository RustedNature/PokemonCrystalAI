using PokeTorchAi.Pokemon.Enums;

namespace PokeTorchAi.Pokemon.Classes
{
    public class MemoryValue
    {
        public MemoryAddresses MemAddress { get; set; }
        public int MemValue { get; set; }

        public MemoryValue()
        {
            
        }
        public MemoryValue(MemoryAddresses memAddress, int memValue = 0)
        {
            MemAddress = memAddress;
            MemValue = memValue;
        }
    }
}