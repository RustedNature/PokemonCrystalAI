import json

from PyPipeClient import PyPipeClient
from MemoryValue import MemoryValue
from MemoryAddresses import MemoryAddresses
from pyboy import PyBoy, WindowEvent



memory_value_list = [MemoryValue(MemoryAddresses.MapBank),
                     MemoryValue(MemoryAddresses.MapNumber),
                     MemoryValue(MemoryAddresses.LevelOfPokemon1),
                     MemoryValue(MemoryAddresses.HpOfPokemon1Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon1Byte2),
                     MemoryValue(MemoryAddresses.LevelOfPokemon2),
                     MemoryValue(MemoryAddresses.HpOfPokemon2Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon2Byte2),
                     MemoryValue(MemoryAddresses.LevelOfPokemon3),
                     MemoryValue(MemoryAddresses.HpOfPokemon3Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon3Byte2),
                     MemoryValue(MemoryAddresses.LevelOfPokemon4),
                     MemoryValue(MemoryAddresses.HpOfPokemon4Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon4Byte2),
                     MemoryValue(MemoryAddresses.LevelOfPokemon5),
                     MemoryValue(MemoryAddresses.HpOfPokemon5Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon5Byte2),
                     MemoryValue(MemoryAddresses.LevelOfPokemon6),
                     MemoryValue(MemoryAddresses.HpOfPokemon6Byte1),
                     MemoryValue(MemoryAddresses.HpOfPokemon6Byte2),
                     MemoryValue(MemoryAddresses.NumberOfPokemonInTeam),
                     MemoryValue(MemoryAddresses.BattleType),
                     MemoryValue(MemoryAddresses.MinutesPlayTimeInGame),
                     MemoryValue(MemoryAddresses.HoursPlayTimeInGameByte1),
                     MemoryValue(MemoryAddresses.HoursPlayTimeInGameByte2)]


def read_memory():
    for memval in memory_value_list:
        memval.set_mem_value(pyboy.get_memory_value(memval.memAddress.value))


def convert_image_to_bytes():
    img = pyboy.screen_image().tobytes()
    return img


def move(movement, pyboym: PyBoy):
    pyboym.send_input(WindowEvent.RELEASE_ARROW_UP)
    pyboym.send_input(WindowEvent.RELEASE_ARROW_DOWN)
    pyboym.send_input(WindowEvent.RELEASE_ARROW_LEFT)
    pyboym.send_input(WindowEvent.RELEASE_ARROW_RIGHT)
    pyboym.send_input(WindowEvent.RELEASE_BUTTON_A)
    pyboym.send_input(WindowEvent.RELEASE_BUTTON_B)
    pyboym.send_input(WindowEvent.RELEASE_BUTTON_START)
    pyboym.send_input(WindowEvent.RELEASE_BUTTON_SELECT)
    if movement == 0:
        pyboym.send_input(WindowEvent.PRESS_ARROW_UP)
        #print("Up")
    elif movement == 1:
        pyboym.send_input(WindowEvent.PRESS_ARROW_DOWN)
        #print("Down")
    elif movement == 2:
        pyboym.send_input(WindowEvent.PRESS_ARROW_LEFT)
        #print("Left")
    elif movement == 3:
        pyboym.send_input(WindowEvent.PRESS_ARROW_RIGHT)
        #print("Right")
    elif movement == 4:
        pyboym.send_input(WindowEvent.PRESS_BUTTON_A)
        #print("A")
    elif movement == 5:
        pyboym.send_input(WindowEvent.PRESS_BUTTON_B)
        #print("B")
    elif movement == 6:
        pyboym.send_input(WindowEvent.PRESS_BUTTON_START)
        #print("Start")
    elif movement == 7:
        pyboym.send_input(WindowEvent.PRESS_BUTTON_SELECT)
        #print("Select")


def get_encoded_mem_vals():
    mem_val_dict = [mem_val.to_dict() for mem_val in memory_value_list]
    json_str = json.dumps(mem_val_dict, indent=4)
    return json_str.encode()


if __name__ == "__main__":
    while True:
        try:
            pipe = PyPipeClient()
            pyboy = PyBoy(r".\ROM\Pokemon - Kristall-Edition (Germany).gbc")
            pyboy.set_emulation_speed(0)
            with open(r".\ROM\Pokemon - Kristall-Edition (Germany).gbc.state", "rb") as state_file:
                pyboy.load_state(state_file)
            while True:
                pyboy.tick()
                if pyboy.frame_count % 4 == 0:
                    read_memory()
                    pipe.send_image_to_cs(convert_image_to_bytes())
                    pipe.send_mem_vals_to_cs(get_encoded_mem_vals())
                    move(pipe.read_movement_from_cs(), pyboy)
                    reset = pipe.read_reset_from_cs()
                    if reset:
                        with open(r".\ROM\Pokemon - Kristall-Edition (Germany).gbc.state", "rb") as state_file:
                            pyboy.load_state(state_file)
                  
        except :
            pass
        
        
