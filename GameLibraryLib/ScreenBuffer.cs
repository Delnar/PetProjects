using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryLib
{
    public abstract class ScreenBuffer
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public DisplaySurface displaySurface { get; set; }


        public ScreenBuffer(DisplaySurface displaySurface)
        {
            this.displaySurface = displaySurface;
        }

        public abstract void Draw(int x, int y, IPixel pixel = null);
        public abstract void Fill(int x1, int y1, int x2, int y2, IPixel pixel = null);
        public abstract void DrawString(int x, int y, string c, Int32? COLOUR = null);
        public virtual void DrawStringAlpha(int x, int y, string c, Int32? COLOUR = null)
        {

        }

        public void Clip(ref int x, ref int y)
        {
            if (x < 0) x = 0;
            if (x >= ScreenWidth) x = ScreenWidth;
            if (y < 0) y = 0;
            if (y >= ScreenHeight) y = ScreenHeight;
        }
    }
}
