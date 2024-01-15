import time

import win32file
import threading


class PyPipeClient:
    def __init__(self):
        self.handle = win32file.CreateFile(
            r"\\.\pipe\PokePipe",  # Pipe name
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,  # Desired access
            0,  # No sharing
            None,  # Default security
            win32file.OPEN_EXISTING,  # Opens existing pipe
            0,  # Default attributes
            None  # No template file
        )
        self.buffer_size = 1

    def read_movement_from_cs(self) -> int:
        (code, movement_bytes) = win32file.ReadFile(self.handle, self.buffer_size)
        return int(movement_bytes.decode())

    def send_image_to_cs(self, image):
        win32file.WriteFile(self.handle, image)
        win32file.FlushFileBuffers(self.handle)

    def send_mem_vals_to_cs(self, mem_vals):
        win32file.WriteFile(self.handle, mem_vals)
        win32file.FlushFileBuffers(self.handle)
