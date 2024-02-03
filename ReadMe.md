# Switched to python and baselines3 because its easier for me  
 [NewRepo](https://github.com/RustedNature/PokemonCrystalAIPy)

# Welcome on my PokeAi page.
I've grown up with Pokemon the second Gen, in my memories it was one   
of the first games I've ever played, so my choice is Pokemon Crystal as candidate for to be Played on an AI.  
I've played Pokemon Gold version.

# How I've become the idea
Iam fascinated of AI's that playing games by (mostly) itself. On YouTube I've found various games played by AI.  
One day I've seen a video of an AI that plays Pokemon Red. And my eye's got shiny.  
That video I mentioned is [this video](https://youtu.be/DcYLT37ImBY?si=5z2TVmkCj7bYP7Dh) 
and the [GitHub Link](https://github.com/PWhiddy/PokemonRedExperiments) 

# Why TorchSharp?
 I used the PyTorch Wrapper [TorchSharp](https://github.com/dotnet/TorchSharp)  
So why not directly code in Python with PyTorch you maybe ask?  
Well there are some simple arguments that i have for preferring C# over Python:
- Explicit variables
- looks much more cleaner
- It's C#


# Progress

Currently implemented:
- ExperienceReplay
- Basic Agent
- Basic Cnn-Model
- ~~Random data for the model~~ Acutal Pokemon image data
- ~~SharedMemory for [PyBoy](https://github.com/Baekalfen/PyBoy) (a great Game Boy emulator) for reading memory values and 
as emulator. If this is to slow i would swap to [BizHawk](https://github.com/TASEmulators/BizHawk) or another emulator~~ 
I using now Pipes and PyBoy
- Implementation for a training loop 
- MemoryReader for keeping track of important values(PyBoy and BizHawk have a MemoryReader, would be the easiest way)
- RewardManager (not fully)

# What's coming? (maybe)
- More variable train loops(Only random actions, predict -> random action)
- Well that's planned for now xD, maybe I get some more ideas as iam coding
