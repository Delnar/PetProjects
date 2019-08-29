using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryLib
{
    public struct sKeyState
    {
        public bool bPressed;
        public bool bReleased;
        public bool bHeld;
    };

    public abstract class InputBuffer
    {
        public sKeyState[] m_keys = new sKeyState[256];
        public sKeyState[] m_mouse = new sKeyState[5];

        public short mousePosX = 0;
        public short mousePosY = 0;

        public short[] keyNewState = new short[256];
        public short[] keyOldState = new short[256];

        public bool[] mouseOldState = new bool[5];
        public bool[] mouseNewState = new bool[5];

        public abstract void Process();
    }
}
