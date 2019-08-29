using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GameLibraryLib.Graphic
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GraphicsPixel: IPixel
    {
        [FieldOffset(0)]
        public int n;
        [FieldOffset(0)]
        public byte r;
        [FieldOffset(1)]
        public byte g;
        [FieldOffset(2)]
        public byte b;
        [FieldOffset(3)]
        public byte a;

        public GraphicsPixel(int n) : this()
        {
            this.n = n;
        }

        public GraphicsPixel(byte r, byte g, byte b, byte a = 255) : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public enum Mode { NORMAL, MASK, ALPHA, CUSTOM };
    }

    public class GraphicPixel
    {
        static public GraphicsPixel WHITE = new GraphicsPixel(255, 255, 255);
        static public GraphicsPixel GREY = new GraphicsPixel(192, 192, 192);
        static public GraphicsPixel DARK_GREY = new GraphicsPixel(128, 128, 128);
        static public GraphicsPixel VERY_DARK_GREY = new GraphicsPixel(64, 64, 64);
        static public GraphicsPixel RED = new GraphicsPixel(255, 0, 0);
        static public GraphicsPixel DARK_RED = new GraphicsPixel(128, 0, 0);
        static public GraphicsPixel VERY_DARK_RED = new GraphicsPixel(64, 0, 0);
        static public GraphicsPixel YELLOW = new GraphicsPixel(255, 255, 0);
        static public GraphicsPixel DARK_YELLOW = new GraphicsPixel(128, 128, 0);
        static public GraphicsPixel VERY_DARK_YELLOW = new GraphicsPixel(64, 64, 0);
        static public GraphicsPixel GREEN = new GraphicsPixel(0, 255, 0);
        static public GraphicsPixel DARK_GREEN = new GraphicsPixel(0, 128, 0);
        static public GraphicsPixel VERY_DARK_GREEN = new GraphicsPixel(0, 64, 0);
        static public GraphicsPixel CYAN = new GraphicsPixel(0, 255, 255);
        static public GraphicsPixel DARK_CYAN = new GraphicsPixel(0, 128, 128);
        static public GraphicsPixel VERY_DARK_CYAN = new GraphicsPixel(0, 64, 64);
        static public GraphicsPixel BLUE = new GraphicsPixel(0, 0, 255);
        static public GraphicsPixel DARK_BLUE = new GraphicsPixel(0, 0, 128);
        static public GraphicsPixel VERY_DARK_BLUE = new GraphicsPixel(0, 0, 64);
        static public GraphicsPixel MAGENTA = new GraphicsPixel(255, 0, 255);
        static public GraphicsPixel DARK_MAGENTA = new GraphicsPixel(128, 0, 128);
        static public GraphicsPixel VERY_DARK_MAGENTA = new GraphicsPixel(64, 0, 64);
        static public GraphicsPixel BLACK = new GraphicsPixel(0, 0, 0);
        static public GraphicsPixel BLANK = new GraphicsPixel(0, 0, 0, 0);
    }
}
