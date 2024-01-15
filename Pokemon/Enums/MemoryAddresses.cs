namespace PokeTorchAi.Pokemon.Enums;

public enum MemoryAddresses
{
    HpOfPokemon1Byte1 = 0xDD01,
    HpOfPokemon1Byte2 = 0xDD02,
    HpOfPokemon2Byte1 = 0xDD31,
    HpOfPokemon2Byte2 = 0xDD32,
    HpOfPokemon3Byte1 = 0xDD61,
    HpOfPokemon3Byte2 = 0xDD62,
    HpOfPokemon4Byte1 = 0xDD91,
    HpOfPokemon4Byte2 = 0xDD92,
    HpOfPokemon5Byte1 = 0xDDB1,
    HpOfPokemon5Byte2 = 0xDDB2,
    HpOfPokemon6Byte1 = 0xDDE1,
    HpOfPokemon6Byte2 = 0xDDE2,

    LevelOfPokemon1 = 0xDCFE,
    LevelOfPokemon2 = 0xDD2E,
    LevelOfPokemon3 = 0xDD5E,
    LevelOfPokemon4 = 0xDD8E,
    LevelOfPokemon5 = 0xDDBE,
    LevelOfPokemon6 = 0xDDEE,

    NumberOfPokemonInTeam = 0xDCD7,

    MapBank = 0xDCB5,
    MapNumber = 0xDCB6,

    BattleType = 0xD22D,

    MinutesPlayTimeInGame = 0xD4C6,
    HoursPlayTimeInGameByte1 = 0xD4C4,
    HoursPlayTimeInGameByte2 = 0xD4C5,
}