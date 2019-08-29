using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryLib.Graphic
{
    public class GraphicsScreenBuffer : ScreenBuffer
    {

        float fPixelX { get; set; } = 0;
        float fPixelY { get; set; } = 0;

        public GraphicsScreenBuffer(int width, int height, int pwidth, int pheight, GraphicsDisplaySurface displaySurface) : base(displaySurface)
        {
            this.ScreenWidth = width;
            this.ScreenHeight = height;
            this.PixelWidth = pwidth;
            this.PixelHeight = pheight;

            fPixelX = 2.0f / (float)(this.ScreenWidth);
            fPixelY = 2.0f / (float)(this.ScreenHeight);
        }

        public override void Draw(int x, int y, IPixel pixel = null)
        {
        }

        public override void DrawString(int x, int y, string c, int? COLOUR = null)
        {
        }

        public override void Fill(int x1, int y1, int x2, int y2, IPixel pixel = null)
        {
        }
    }
}
