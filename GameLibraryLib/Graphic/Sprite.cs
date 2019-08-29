using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameLibraryLib.Graphic
{
    public class Sprite
    {
        public enum Mode { NORMAL, PERIODIC };
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public string FileName { get; set; } = "";
        public GraphicsPixel[] pColData { get; set; } = null;

        public Mode modeSample { get; set; } = Mode.NORMAL;

        public Sprite()
        {

        }

        public Sprite(string FileName)
        {
            LoadFromFile(FileName);
        }

        public Sprite(int w, int h)
        {
            var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Width = w;
            Height = h;

            pColData = new GraphicsPixel[Width * Height];
            for(int x=0; x<pColData.Length; x++) {
                pColData[x] = GraphicPixel.BLANK;
            }
        }

        public void LoadFromFile(string FileName)
        {
            this.FileName = FileName;
            
            var bmp = (Bitmap)Bitmap.FromFile(this.FileName);
            Width = bmp.Width;
            Height = bmp.Height;

            pColData = new GraphicsPixel[Width * Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++) {
                    var c = bmp.GetPixel(x,y);
                    SetPixel(x, y, new GraphicsPixel(c.R, c.G, c.B, c.A));
                }
        }


        public GraphicsPixel GetPixel(int x, int y)
        {
            if (modeSample == Mode.NORMAL) {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                    return pColData[y * Width + x];
                else
                    return GraphicPixel.BLANK;
            } else {
                return pColData[Math.Abs(y % Height) * Width + Math.Abs(x % Width)];
            }
        }

        public bool SetPixel(int x, int y, GraphicsPixel p)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height) {
                pColData[y * Width + x] = p;
                return true;
            } else
                return false;
        }

        public GraphicsPixel Sample(float x, float y)
        {
            int sx = (int)Math.Min(x * (float)Width, (float)Width - 1.0f);
            int sy = (int)Math.Min(y * (float)Height, (float)Height - 1.0f);
            return GetPixel(sx, sy);
        }

        public GraphicsPixel SampleBL(float u, float v)
        {
            u = u * Width - 0.5f;
            v = v * Height - 0.5f;
            int x = (int)Math.Floor(u); // cast to int rounds toward zero, not downward
            int y = (int)Math.Floor(v); // Thanks @joshinils
            float u_ratio = u - x;
            float v_ratio = v - y;
            float u_opposite = 1 - u_ratio;
            float v_opposite = 1 - v_ratio;

            GraphicsPixel p1 = GetPixel(Math.Max(x, 0), Math.Max(y, 0));
            GraphicsPixel p2 = GetPixel(Math.Min(x + 1, Width - 1), Math.Max(y, 0));
            GraphicsPixel p3 = GetPixel(Math.Max(x, 0), Math.Min(y + 1, Height - 1));
            GraphicsPixel p4 = GetPixel(Math.Min(x + 1, Width - 1), Math.Min(y + 1, Height - 1));


            return new GraphicsPixel(
                (byte)((p1.r * u_opposite + p2.r * u_ratio) * v_opposite + (p3.r * u_opposite + p4.r * u_ratio) * v_ratio),
                (byte)((p1.g * u_opposite + p2.g * u_ratio) * v_opposite + (p3.g * u_opposite + p4.g * u_ratio) * v_ratio),
                (byte)((p1.b * u_opposite + p2.b * u_ratio) * v_opposite + (p3.b * u_opposite + p4.b * u_ratio) * v_ratio));
        }

    }
}
