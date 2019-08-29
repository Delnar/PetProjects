using GameLibraryLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGameLibrary
{
    public class ConsolePixel : IPixel
    {
        public const char PIXEL_SOLID = (char)0x2588;
        public const char PIXEL_THREEQUARTERS = (char)0x2593;
        public const char PIXEL_HALF = (char)0x2592;
        public const char PIXEL_QUARTER = (char)0x2591;

        public const ushort FG_BLACK = 0x0000;
        public const ushort FG_DARK_BLUE = 0x0001;
        public const ushort FG_DARK_GREEN = 0x0002;
        public const ushort FG_DARK_CYAN = 0x0003;
        public const ushort FG_DARK_RED = 0x0004;
        public const ushort FG_DARK_MAGENTA = 0x0005;
        public const ushort FG_DARK_YELLOW = 0x0006;
        public const ushort FG_GREY = 0x0007; 
        public const ushort FG_DARK_GREY = 0x0008;
        public const ushort FG_BLUE = 0x0009;
        public const ushort FG_GREEN = 0x000A;
        public const ushort FG_CYAN = 0x000B;
        public const ushort FG_RED = 0x000C;
        public const ushort FG_MAGENTA = 0x000D;
        public const ushort FG_YELLOW = 0x000E;
        public const ushort FG_WHITE = 0x000F;
        public const ushort BG_BLACK = 0x0000;
        public const ushort BG_DARK_BLUE = 0x0010;
        public const ushort BG_DARK_GREEN = 0x0020;
        public const ushort BG_DARK_CYAN = 0x0030;
        public const ushort BG_DARK_RED = 0x0040;
        public const ushort BG_DARK_MAGENTA = 0x0050;
        public const ushort BG_DARK_YELLOW = 0x0060;
        public const ushort BG_GREY = 0x0070;
        public const ushort BG_DARK_GREY = 0x0080;
        public const ushort BG_BLUE = 0x0090;
        public const ushort BG_GREEN = 0x00A0;
        public const ushort BG_CYAN = 0x00B0;
        public const ushort BG_RED = 0x00C0;
        public const ushort BG_MAGENTA = 0x00D0;
        public const ushort BG_YELLOW = 0x00E0;
        public const ushort BG_WHITE = 0x00F0;

        public char PIXEL_TYPE { get; set; } = ConsolePixel.PIXEL_SOLID;
        public ushort COLOUR { get; set; } = ConsolePixel.FG_GREY;

        public ConsolePixel()
        {

        }

        public ConsolePixel(char pPIXEL_TYPE, ushort pCOLOUR)
        {
            PIXEL_TYPE = pPIXEL_TYPE;
            COLOUR = pCOLOUR;
        }
    }
}
