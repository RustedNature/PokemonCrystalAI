using PokeTorchAi.Pokemon.Classes;
using PokeTorchAi.Pokemon.Enums;

namespace PokeTorchAi.Ai;

public class RewardManager
{
    private List<MemoryValue> _currentMemoryValues;
   
    private int _levelOfPokemon1 = 0;
    private byte _hpOfPokemon1Byte1 = 0;
    private byte _hpOfPokemon1Byte2 = 0;
    private int _hpOfPokemon1 = 0;
    
    private int _levelOfPokemon2 = 0;
    private byte _hpOfPokemon2Byte1 = 0;
    private byte _hpOfPokemon2Byte2 = 0;
    private int _hpOfPokemon2 = 0;
    
    private int _levelOfPokemon3 = 0;
    private byte _hpOfPokemon3Byte1 = 0;
    private byte _hpOfPokemon3Byte2 = 0;
    private int _hpOfPokemon3 = 0;
    
    private int _levelOfPokemon4 = 0;
    private byte _hpOfPokemon4Byte1 = 0;
    private byte _hpOfPokemon4Byte2 = 0;
    private int _hpOfPokemon4 = 0;
    
    private int _levelOfPokemon5 = 0;
    private byte _hpOfPokemon5Byte1 = 0;
    private byte _hpOfPokemon5Byte2 = 0;
    private int _hpOfPokemon5 = 0;
    
    private int _levelOfPokemon6 = 0;
    private byte _hpOfPokemon6Byte1 = 0;
    private byte _hpOfPokemon6Byte2 = 0;
    private int _hpOfPokemon6 = 0;
    
    private int _numberOfPokemonInTeam = 0;
    
    private int _mapBank = 0;
    private int _mapNumber = 0;
    
    private int _battleType = 0;
    
    private int _minutesPlayTimeInGame = 0;
    private int _hoursPlayTimeInGameByte1 = 0;
    private int _hoursPlayTimeInGameByte2 = 0;
    private int _hoursPlayTimeInGame = 0;
    
    private bool _gotNeuborkiaLivingRoomReward = false;
    private bool _gotNeuborkiaOutsideReward = false;
    private bool _gotNeuborkiaRoute29Reward = false;
    private bool _gotNeuborkiaProfReward = false;

    public RewardManager()
    {
        
    }

    public void RefreshMemory(List<MemoryValue> memoryValues)
    {
        Console.WriteLine($"Rewards:\n" +
                          $"LivingRoom: {_gotNeuborkiaLivingRoomReward}\n" +
                          $"Outside: {_gotNeuborkiaOutsideReward}");
        _currentMemoryValues = memoryValues;
        ReadMemoryValues();
        Calculate2ByteValues();
    }

    private void Calculate2ByteValues()
    {
        var hpPokemon1Hp = new byte[] { _hpOfPokemon1Byte1, _hpOfPokemon1Byte2 };
        _hpOfPokemon1 = BitConverter.ToInt16(hpPokemon1Hp, 0);
        
        var hpPokemon2Hp = new byte[] { _hpOfPokemon2Byte1, _hpOfPokemon2Byte2 };
        _hpOfPokemon2 = BitConverter.ToInt16(hpPokemon2Hp, 0);
        
        var hpPokemon3Hp = new byte[] { _hpOfPokemon3Byte1, _hpOfPokemon3Byte2 };
        _hpOfPokemon3 = BitConverter.ToInt16(hpPokemon3Hp, 0);
        
        var hpPokemon4Hp = new byte[] { _hpOfPokemon4Byte1, _hpOfPokemon4Byte2 };
        _hpOfPokemon4 = BitConverter.ToInt16(hpPokemon4Hp, 0);
        
        var hpPokemon5Hp = new byte[] { _hpOfPokemon5Byte1, _hpOfPokemon5Byte2 };
        _hpOfPokemon5 = BitConverter.ToInt16(hpPokemon5Hp, 0);
        
        var hpPokemon6Hp = new byte[] { _hpOfPokemon6Byte1, _hpOfPokemon6Byte2 };
        _hpOfPokemon6 = BitConverter.ToInt16(hpPokemon6Hp, 0);
        
        var hoursPlayTime = new byte[] { _hpOfPokemon6Byte1, _hpOfPokemon6Byte2 };
        _hoursPlayTimeInGame = BitConverter.ToInt16(hoursPlayTime, 0);
    }

    public float GetReward()
    {
        float reward = 0;
        reward += CalculatePositiveReward();
        reward += CalculateNegativeReward();
        
        return reward;
    }

    private float CalculateNegativeReward()
    {
        float negativeReward = 0;
        

        return negativeReward;
    }

    private float CalculatePositiveReward()
    {
        float PositiveReward = 0;
        PositiveReward += OneTimeRewards();
        return PositiveReward;
    }

    private float OneTimeRewards()
    {
        float oneTimeReward = 0;
        oneTimeReward += NeuborkiaLivingRoomReward();
        oneTimeReward += NeuborkiaOutsideReward();
        oneTimeReward += NeuborkiaProfHouseReward();
        oneTimeReward += NeuborkiaRoute29Reward();

        return oneTimeReward;
    }

    private float NeuborkiaRoute29Reward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.Route29 && _gotNeuborkiaRoute29Reward is false )
        {
            _gotNeuborkiaRoute29Reward = true;
            return 2;
        }

        return 0;
    }

    private float NeuborkiaProfHouseReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.Outside && _gotNeuborkiaProfReward is false )
        {
            _gotNeuborkiaProfReward = true;
            return 2;
        }

        return 0;
    }

    private float NeuborkiaOutsideReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.Outside && _gotNeuborkiaOutsideReward is false )
        {
            _gotNeuborkiaOutsideReward = true;
            return 2;
        }

        return 0;
    }

    private float NeuborkiaLivingRoomReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.LivingRoom && _gotNeuborkiaLivingRoomReward is false)
        {
            _gotNeuborkiaLivingRoomReward = true;
            return 2;
        }

        return 0;
    }

    private void ReadMemoryValues()
    {
        foreach (var memoryValue in _currentMemoryValues)
        {
            switch (memoryValue.MemAddress)
            {
                case MemoryAddresses.BattleType:
                    _battleType = memoryValue.MemValue;
                    break;
                case MemoryAddresses.MapBank:
                    _mapBank = memoryValue.MemValue;
                    break;
                case MemoryAddresses.MapNumber:
                    _mapNumber = memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon1Byte1:
                    _hpOfPokemon1Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon1Byte2:
                    _hpOfPokemon1Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon2Byte1:
                    _hpOfPokemon2Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon2Byte2:
                    _hpOfPokemon2Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon3Byte1:
                    _hpOfPokemon3Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon3Byte2:
                    _hpOfPokemon3Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon4Byte1:
                    _hpOfPokemon4Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon4Byte2:
                    _hpOfPokemon4Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon5Byte1:
                    _hpOfPokemon5Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon5Byte2:
                    _hpOfPokemon5Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon6Byte1:
                    _hpOfPokemon6Byte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HpOfPokemon6Byte2:
                    _hpOfPokemon6Byte2 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon1:
                    _levelOfPokemon1 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon2:
                    _levelOfPokemon2 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon3:
                    _levelOfPokemon3 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon4:
                    _levelOfPokemon4 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon5:
                    _levelOfPokemon5 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon6:
                    _levelOfPokemon6 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.NumberOfPokemonInTeam:
                    _numberOfPokemonInTeam = memoryValue.MemValue;
                    break;
                case MemoryAddresses.MinutesPlayTimeInGame:
                    _minutesPlayTimeInGame = memoryValue.MemValue;
                    break;
                case MemoryAddresses.HoursPlayTimeInGameByte1:
                    _hoursPlayTimeInGameByte1 = (byte)memoryValue.MemValue;
                    break;
                case MemoryAddresses.HoursPlayTimeInGameByte2:
                    _hoursPlayTimeInGameByte2 = (byte)memoryValue.MemValue;
                    break;
                default:
                    break;
            }
        }
    }
}