using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryLib
{
    public abstract class DisplaySurface
    {
        public int ScreenWidth { get; set; } = 0;
        public int ScreenHeight { get; set; } = 0;

        public int PixelWidth { get; set; } = 0;
        public int PixelHeight { get; set; } = 0;
        public int ScreenSize { get => ScreenWidth * ScreenHeight; }
        public string Title { get; set; } = "";

        public ScreenBuffer screenBuffer { get; set; } = null;

        public abstract void Construct(int width, int height, int pixelw, int pixelh);
        public abstract void DrawSurface();
        public abstract void SetTitle(string Title);
    }
}
