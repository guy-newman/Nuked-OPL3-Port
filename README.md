# Nuked-OPL3-Port
A C# port of the Nuked-OPL3 library.

The original library is available here: https://github.com/nukeykt/Nuked-OPL3/tree/master

The library is a quick conversion of the above C library into C#, and carries the same Lesser GNU Public License.

## How to Use it
You'll need to provide your own driver to set the registers of the OPL3 chip - that isn't included in this project. Once you have this:
1. Create an Opl3_Chip object.
2. For stereo output, call the `Opl3.OPL3_GenerateStream` method to recover the output data from the OPL3 chip. The output is interleaved stereo stored in shorts.
