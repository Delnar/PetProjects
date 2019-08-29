using GameLibraryLib;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleGameLibrary
{
    public class ConsoleInputBuffer : InputBuffer
    {
        private const byte FOCUS_EVENT = 0x0010;
        private const byte KEY_EVENT = 0x0001;
        private const byte MENU_EVENT = 0x0008;
        private const byte MOUSE_EVENT = 0x0002;
        private const byte WINDOW_BUFFER_SIZE_EVENT = 0x0004;

        private const byte SINGLE_CLICK = 0x0;
        private const byte DOUBLE_CLICK = 0x0002;
        private const byte MOUSE_HWHEELED = 0x0008;
        private const byte MOUSE_MOVED = 0x0001;
        private const byte MOUSE_WHEELED = 0x0004;

        #region pInvoke

        [Flags]
        private enum ConsoleInputModes : uint
        {
            ENABLE_PROCESSED_INPUT = 0x0001,
            ENABLE_LINE_INPUT = 0x0002,
            ENABLE_ECHO_INPUT = 0x0004,
            ENABLE_WINDOW_INPUT = 0x0008,
            ENABLE_MOUSE_INPUT = 0x0010,
            ENABLE_INSERT_MODE = 0x0020,
            ENABLE_QUICK_EDIT_MODE = 0x0040,
            ENABLE_EXTENDED_FLAGS = 0x0080,
            ENABLE_AUTO_POSITION = 0x0100
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Int32 vKey);

        [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
        static extern bool ReadConsoleInput(
            IntPtr hConsoleInput,
            [Out] INPUT_RECORD[] lpBuffer,
            uint nLength,
            out uint lpNumberOfEventsRead
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetNumberOfConsoleInputEvents(
            IntPtr hConsoleInput,
            out uint lpcNumberOfEvents
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            uint dwMode
            );

        const int STD_OUTPUT_HANDLE = -11;
        const int STD_INPUT_HANDLE = -10;
        const int STD_ERROR_HANDLE = -12;
        IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        #endregion

        public bool ConsoleIsFocus = true;
        public IntPtr hConsoleIn { get; set; } = default;

       
        public ConsoleInputBuffer()
        {
            hConsoleIn = GetStdHandle(STD_INPUT_HANDLE);

            if (!SetConsoleMode(hConsoleIn, (uint)ConsoleInputModes.ENABLE_EXTENDED_FLAGS |
                                (uint)ConsoleInputModes.ENABLE_WINDOW_INPUT |
                                (uint)ConsoleInputModes.ENABLE_MOUSE_INPUT)) {
                var ErrorId = Marshal.GetLastWin32Error();
                throw new Exception($"SetConsoleMode {ErrorId}");
            }
        }

        public ConsoleInputBuffer(IntPtr hConsoleIn)
        {
            this.hConsoleIn = hConsoleIn;
        }


        public override void Process()
        {

            for (int i = 0; i < 256; i++) {
                keyNewState[i] = GetAsyncKeyState(i);
                m_keys[i].bPressed = false;
                m_keys[i].bReleased = false;

                if (keyNewState[i] != keyOldState[i]) {
                    if ((keyNewState[i] & 0x8000) != 0) {
                        m_keys[i].bPressed = !m_keys[i].bHeld;
                        m_keys[i].bHeld = true;
                    } else {
                        m_keys[i].bReleased = true;
                        m_keys[i].bHeld = false;
                    }
                }
                keyOldState[i] = keyNewState[i];
            }

            INPUT_RECORD[] inBuf = new INPUT_RECORD[32];
            GetNumberOfConsoleInputEvents(hConsoleIn, out uint events);

            if (events > 0) {
                ReadConsoleInput(hConsoleIn, inBuf, events, out events);
            }

            for (int i = 0; i < events; i++) {
                switch (inBuf[i].EventType) {
                    case FOCUS_EVENT:
                        ConsoleIsFocus = inBuf[i].FocusEvent.bSetFocus > 0;
                        break;

                    case MOUSE_EVENT:
                        switch (inBuf[i].MouseEvent.dwEventFlags) {
                            case MOUSE_MOVED: {
                                    mousePosX = inBuf[i].MouseEvent.dwMousePosition.X;
                                    mousePosY = inBuf[i].MouseEvent.dwMousePosition.Y;
                                }
                                break;

                            case SINGLE_CLICK: {
                                    for (int m = 0; m < 5; m++)
                                        mouseNewState[m] = (inBuf[i].MouseEvent.dwButtonState & (1 << m)) > 0;

                                }
                                break;

                            default:
                                break;
                        }
                        break;
                    default:  // We don't care just at the moment
                        break;
                }

                for (int m = 0; m < 5; m++) {
                    m_mouse[m].bPressed = false;
                    m_mouse[m].bReleased = false;

                    if (mouseNewState[m] != mouseOldState[m]) {
                        if (mouseNewState[m]) {
                            m_mouse[m].bPressed = true;
                            m_mouse[m].bHeld = true;
                        } else {
                            m_mouse[m].bReleased = true;
                            m_mouse[m].bHeld = false;
                        }
                    }
                    mouseOldState[m] = mouseNewState[m];
                }
            }
        }

    }
}
