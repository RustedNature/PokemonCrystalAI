using PokeTorchAi.Pokemon.Classes;
using PokeTorchAi.Pokemon.Enums;

namespace PokeTorchAi.Ai;

public class RewardManager
{
    private const float DefaultReward = 10;
    private const int MaxPokemonLevel = 100;
    private List<MemoryValue>? _currentMemoryValues;

    private int _lastLevelOfPokemon1 = 0;
    private int _currentLevelOfPokemon1 = 0;
    private byte _hpOfPokemon1Byte1 = 0;
    private byte _hpOfPokemon1Byte2 = 0;
    private int _hpOfPokemon1 = 0;

    private int _lastLevelOfPokemon2 = 0;
    private int _currentLevelOfPokemon2 = 0;
    private byte _hpOfPokemon2Byte1 = 0;
    private byte _hpOfPokemon2Byte2 = 0;
    private int _hpOfPokemon2 = 0;

    private int _lastLevelOfPokemon3 = 0;
    private int _currentLevelOfPokemon3 = 0;
    private byte _hpOfPokemon3Byte1 = 0;
    private byte _hpOfPokemon3Byte2 = 0;
    private int _hpOfPokemon3 = 0;

    private int _lastLevelOfPokemon4 = 0;
    private int _currentLevelOfPokemon4 = 0;
    private byte _hpOfPokemon4Byte1 = 0;
    private byte _hpOfPokemon4Byte2 = 0;
    private int _hpOfPokemon4 = 0;

    private int _lastLevelOfPokemon5 = 0;
    private int _currentLevelOfPokemon5 = 0;
    private byte _hpOfPokemon5Byte1 = 0;
    private byte _hpOfPokemon5Byte2 = 0;
    private int _hpOfPokemon5 = 0;

    private int _lastLevelOfPokemon6 = 0;
    private int _currentLevelOfPokemon6 = 0;
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

    public int GetHoursPlayed()
    {
        return _hoursPlayTimeInGame;
    }
    public int GetMinutesPlayed()
    {
        return _minutesPlayTimeInGame;
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

    public void ResetRewards()
    {
        _gotNeuborkiaLivingRoomReward = false;
        _gotNeuborkiaOutsideReward = false;
        _gotNeuborkiaRoute29Reward = false;
        _gotNeuborkiaProfReward = false;
        _lastLevelOfPokemon1 = 0;
        _lastLevelOfPokemon2 = 0;
        _lastLevelOfPokemon3 = 0;
        _lastLevelOfPokemon4 = 0;
        _lastLevelOfPokemon5 = 0;
        _lastLevelOfPokemon6 = 0;
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

    private float PokemonInTeamLevelSum()
    {
        float sum = 0;
        if (_numberOfPokemonInTeam == 0) return sum;

        if (_currentLevelOfPokemon1 > _lastLevelOfPokemon1 && _currentLevelOfPokemon1 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 1 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon1 = _currentLevelOfPokemon1;
            sum += _currentLevelOfPokemon1;
            Logging.UpdateLogContent($"Pokemon 1 Level: {_currentLevelOfPokemon1} and got {_currentLevelOfPokemon1} reward points");
        }
        if (_currentLevelOfPokemon2 > _lastLevelOfPokemon2 && _currentLevelOfPokemon2 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 2 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon2 = _currentLevelOfPokemon2;
            sum += _currentLevelOfPokemon2;
            Logging.UpdateLogContent($"Pokemon 2 Level: {_currentLevelOfPokemon2} and got {_currentLevelOfPokemon2} reward points");
        }
        if (_currentLevelOfPokemon3 > _lastLevelOfPokemon3 && _currentLevelOfPokemon3 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 3 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon3 = _currentLevelOfPokemon3;
            sum += _currentLevelOfPokemon3;
            Logging.UpdateLogContent($"Pokemon 3 Level: {_currentLevelOfPokemon3} and got {_currentLevelOfPokemon3} reward points");
        }
        if (_currentLevelOfPokemon4 > _lastLevelOfPokemon4 && _currentLevelOfPokemon4 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 4 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon4 = _currentLevelOfPokemon4;
            sum += _currentLevelOfPokemon4;
            Logging.UpdateLogContent($"Pokemon 4 Level: {_currentLevelOfPokemon4} and got {_currentLevelOfPokemon4} reward points");
        }
        if (_currentLevelOfPokemon5 > _lastLevelOfPokemon5 && _currentLevelOfPokemon5 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 5 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon5 = _currentLevelOfPokemon5;
            sum += _currentLevelOfPokemon5;
            Logging.UpdateLogContent($"Pokemon 5 Level: {_currentLevelOfPokemon5} and got {_currentLevelOfPokemon5} reward points");
        }
        if (_currentLevelOfPokemon6 > _lastLevelOfPokemon6 && _currentLevelOfPokemon6 <= MaxPokemonLevel && _numberOfPokemonInTeam >= 6 && _numberOfPokemonInTeam <= 6)
        {
            _lastLevelOfPokemon6 = _currentLevelOfPokemon6;
            sum += _currentLevelOfPokemon6;
            Logging.UpdateLogContent($"Pokemon 6 Level: {_currentLevelOfPokemon6} and got {_currentLevelOfPokemon6} reward points");
        }

        return sum;
    }

    private float OneTimeRewards()
    {
        float oneTimeReward = 0;
        oneTimeReward += NeuborkiaLivingRoomReward();
        oneTimeReward += NeuborkiaOutsideReward();
        oneTimeReward += NeuborkiaProfHouseReward();
        oneTimeReward += NeuborkiaRoute29Reward();
        oneTimeReward += PokemonInTeamLevelSum();

        return oneTimeReward;
    }

    private float NeuborkiaRoute29Reward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.Route29 && _gotNeuborkiaRoute29Reward is false)
        {
            _gotNeuborkiaRoute29Reward = true;
            Logging.UpdateLogContent($"Vistited {MapBank.Neuborkia} {NeuborkiaMapNumber.Route29} first time and got {DefaultReward} reward points");
            return DefaultReward;
        }

        return 0;
    }

    private float NeuborkiaProfHouseReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.ProfHouse && _gotNeuborkiaProfReward is false)
        {
            _gotNeuborkiaProfReward = true;
            Logging.UpdateLogContent($"Vistited {MapBank.Neuborkia} {NeuborkiaMapNumber.ProfHouse} first time and got {DefaultReward} reward points");
            return DefaultReward;
        }

        return 0;
    }

    private float NeuborkiaOutsideReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.Outside && _gotNeuborkiaOutsideReward is false)
        {
            _gotNeuborkiaOutsideReward = true;
            Logging.UpdateLogContent($"Vistited {MapBank.Neuborkia} {NeuborkiaMapNumber.Outside} first time and got {DefaultReward} reward points");
            return DefaultReward;
        }

        return 0;
    }

    private float NeuborkiaLivingRoomReward()
    {
        if (_mapBank == (int)MapBank.Neuborkia && _mapNumber == (int)NeuborkiaMapNumber.LivingRoom && _gotNeuborkiaLivingRoomReward is false)
        {
            _gotNeuborkiaLivingRoomReward = true;
            Logging.UpdateLogContent($"Vistited {MapBank.Neuborkia} {NeuborkiaMapNumber.LivingRoom} first time and got {DefaultReward} reward points");
            return DefaultReward;
        }

        return 0;
    }

    private void ReadMemoryValues()
    {
        foreach (var memoryValue in _currentMemoryValues!)
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
                    _currentLevelOfPokemon1 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon2:
                    _currentLevelOfPokemon2 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon3:
                    _currentLevelOfPokemon3 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon4:
                    _currentLevelOfPokemon4 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon5:
                    _currentLevelOfPokemon5 = memoryValue.MemValue;
                    break;
                case MemoryAddresses.LevelOfPokemon6:
                    _currentLevelOfPokemon6 = memoryValue.MemValue;
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