using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryLib
{
    public class GraphicsDisplaySurface : DisplaySurface
    {
        public override void Construct(int width, int height, int pixelw, int pixelh)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            PixelWidth = pixelw;
            PixelHeight = pixelh;

            if (PixelWidth == 0 || PixelHeight == 0 || ScreenWidth == 0 || ScreenHeight == 0)
                throw new Exception("Failed to construct");


            screenBuffer = new Graphic.GraphicsScreenBuffer(width, height, pixelw, pixelh, this);
        }

        public override void DrawSurface()
        {
            
        }

        public override void SetTitle(string Title)
        {
            
        }
    }
}
