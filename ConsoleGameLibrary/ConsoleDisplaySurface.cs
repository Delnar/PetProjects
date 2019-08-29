using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using GameLibraryLib;

namespace ConsoleGameLibrary
{
    public class ConsoleDisplaySurface : DisplaySurface
    {

        #region pInvokes
        private const byte DEFAULT_CHARSET = 1;
        private const byte SHIFTJIS_CHARSET = 128;
        private const byte JOHAB_CHARSET = 130;
        private const byte EASTEUROPE_CHARSET = 238;
        private const byte DEFAULT_PITCH = 0;
        private const byte FIXED_PITCH = 1;
        private const byte VARIABLE_PITCH = 2;
        private const byte FF_DONTCARE = (0 << 4);
        private const byte FF_ROMAN = (1 << 4);
        private const byte FF_SWISS = (2 << 4);
        private const byte FF_MODERN = (3 << 4);
        private const byte FF_SCRIPT = (4 << 4);
        private const byte FF_DECORATIVE = (5 << 4);

        public enum FontWeight : int
        {
            FW_DONTCARE = 0,
            FW_THIN = 100,
            FW_EXTRALIGHT = 200,
            FW_LIGHT = 300,
            FW_NORMAL = 400,
            FW_MEDIUM = 500,
            FW_SEMIBOLD = 600,
            FW_BOLD = 700,
            FW_EXTRABOLD = 800,
            FW_HEAVY = 900,
        }

        [Flags]
        private enum ConsoleOutputModes : uint
        {
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
            DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
            ENABLE_LVB_GRID_WORLDWIDE = 0x0010
        }

        const int STD_OUTPUT_HANDLE = -11;
        const int STD_INPUT_HANDLE = -10;
        const int STD_ERROR_HANDLE = -12;
        IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("Kernel32.dll")]
        static extern IntPtr CreateConsoleScreenBuffer(
            UInt32 dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr secutiryAttributes,
            UInt32 flags,
            IntPtr screenBufferData
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleWindowInfo(
            IntPtr hConsoleOutput,
            bool bAbsolute,
            [In] ref SMALL_RECT lpConsoleWindow
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleScreenBufferSize(
            IntPtr hConsoleOutput,
            COORD dwSize
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleActiveScreenBuffer(
                IntPtr hConsoleOutput
                );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr ConsoleOutput,
            bool MaximumWindow,
            CONSOLE_FONT_INFOEX ConsoleCurrentFontEx
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleScreenBufferInfo(
            IntPtr hConsoleOutput,
            out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo
            );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleTitle(
            string lpConsoleTitle
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern COORD GetLargestConsoleWindowSize(
            IntPtr hConsoleOutput
            );
        #endregion

        IntPtr hConsole { get; set; } = default;
        public ConsoleScreenBuffer bufScreen { get => screenBuffer as ConsoleScreenBuffer; set => screenBuffer = value; }

        SMALL_RECT rectWindow = new SMALL_RECT(0, 0, 1, 1);

        public override void Construct(int width, int height, int fontw, int fonth)
        {
            hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

            if (hConsole == INVALID_HANDLE_VALUE) throw new Exception("Bad Handle");
            ScreenWidth = width;
            ScreenHeight = height;
            PixelWidth = fontw;
            PixelHeight = fonth;

            Console.CursorVisible = false;


            rectWindow = new SMALL_RECT(0, 0, 1, 1);

            if (!SetConsoleWindowInfo(hConsole, true, ref rectWindow)) {
                // You need to call GetLastError
                // https://docs.microsoft.com/en-us/windows/console/setconsolewindowinfo
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetConsoleWindowInfo {ErrorId}");
            }
            var coord = new COORD((short)ScreenWidth, (short)ScreenHeight);

            if (!SetConsoleScreenBufferSize(hConsole, coord)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetConsoleScreenBufferSize {ErrorId}");
            }

            if (!SetConsoleActiveScreenBuffer(hConsole)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetConsoleActiveScreenBuffer {ErrorId}");
            }

            var cfi = new CONSOLE_FONT_INFOEX(0, new COORD((short)fontw, (short)fonth), FF_DONTCARE, (int)FontWeight.FW_NORMAL, "Consolas");

            if (!SetCurrentConsoleFontEx(hConsole, false, cfi)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetCurrentConsoleFontEx {ErrorId}");
            }

            var csbi = new CONSOLE_SCREEN_BUFFER_INFO();
            if (!GetConsoleScreenBufferInfo(hConsole, out csbi)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"GetConsoleScreenBufferInfo {ErrorId}");
            }
            if (ScreenHeight > csbi.dwMaximumWindowSize.Y) {
                throw new Exception("Screen Height / Font Height Too Big");
            }

            if (ScreenWidth > csbi.dwMaximumWindowSize.X) {
                throw new Exception("Screen Width / Font Width Too Big");
            }

            // var LargestSize = new COORD();
            // LargestSize = GetLargestConsoleWindowSize(hConsole);

            rectWindow.Bottom = (short)(ScreenHeight - 1);
            rectWindow.Right = (short)(ScreenWidth - 1);

            if (!SetConsoleWindowInfo(hConsole, true, ref rectWindow)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetConsoleWindowInfo #2 {ErrorId}");
            }

            screenBuffer = new ConsoleScreenBuffer(ScreenWidth, ScreenHeight, this);
        }

        public override void DrawSurface()
        {
            WriteConsoleOutput(hConsole, bufScreen.bufScreen, new COORD((short)ScreenWidth, (short)ScreenHeight), new COORD(0, 0), ref rectWindow);
        }

        public override void SetTitle(string title)
        {            
            SetConsoleTitle(title);
        }
    }
}
