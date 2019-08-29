using GameLibraryLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGameLibrary
{
    public class ConsoleScreenBuffer : ScreenBuffer
    {
        public CHAR_INFO[] bufScreen { get; set; }

        public ConsoleScreenBuffer(int Width, int Height, ConsoleDisplaySurface displaySurface) : base(displaySurface)
        {
            ScreenWidth = Width;
            ScreenHeight = Height;

            bufScreen = new CHAR_INFO[ScreenWidth * ScreenHeight];
        }

        public override void Draw(int x, int y, IPixel pixel = null)
        {
            if (pixel == null) pixel = new ConsolePixel();
            var p = pixel as ConsolePixel;
            if (x >= 0 && x < ScreenWidth && y >= 0 && y < ScreenHeight) {
                bufScreen[y * ScreenWidth + x].UnicodeChar = p.PIXEL_TYPE;
                bufScreen[y * ScreenWidth + x].Attributes = p.COLOUR;
            }
        }

        public override void Fill(int x1, int y1, int x2, int y2, IPixel pixel = null)
        {
            if (pixel == null) pixel = new ConsolePixel();
            Clip(ref x1, ref y1);
            Clip(ref x2, ref y2);
            for (int x = x1; x < x2; x++)
                for (int y = y1; y < y2; y++)
                    Draw(x, y, pixel);
        }

        public override void DrawString(int x, int y, string c, int? COLOUR = null)
        {
            if (COLOUR == null) COLOUR = ConsolePixel.BG_WHITE;
            for (int i = 0; i < c.Length; i++) {
                bufScreen[y * ScreenWidth + x + i].UnicodeChar = c[i];
                bufScreen[y * ScreenWidth + x + i].Attributes = (ushort)COLOUR.Value;
            }
        }

        public override void DrawStringAlpha(int x, int y, string c, int? COLOUR = null)
        {
            if (COLOUR == null) COLOUR = ConsolePixel.BG_WHITE;
            for (int i = 0; i < c.Length; i++) {
                if (c[i] != ' ') {
                    bufScreen[y * ScreenWidth + x + i].UnicodeChar = c[i];
                    bufScreen[y * ScreenWidth + x + i].Attributes = (ushort)COLOUR.Value;
                }
            }

        }
    }
}
