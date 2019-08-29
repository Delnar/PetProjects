using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GameLibraryLib
{
    public abstract class GameLibrary
    {
        public DisplaySurface displaySurface = null;
        public InputBuffer inputBuffer = null;

        public string AppName { get; set; } = "";

        public bool AtomActive = false;

        public int ScreenWidth { get => displaySurface.ScreenWidth; }
        public int ScreenHeight { get => displaySurface.ScreenHeight; }
        public int ScreenSize { get => displaySurface.ScreenSize; }

        public GameLibrary()
        {            
        }

        public void Start()
        {
            if (displaySurface == null) throw new Exception("Display Surface has not been set");
            if (inputBuffer == null) throw new Exception("Input Buffer has not been set");

            AtomActive = true;
            var th1 = new Thread(GameLoop);
            th1.Start();
            th1.Join();

        }

        public void GameLoop()
        {
            if (OnUserCreate() == false) {
                AtomActive = false;
            }

            var tp1 = DateTime.Now;

            while(AtomActive) {
                var tp2 = DateTime.Now;
                var elapsedTime = tp2 - tp1;
                tp1 = tp2;

                var title = $"OneLoneCoder.com - Console Game Engine - {AppName} - FPS: {1.0f / elapsedTime.Ticks}";
                displaySurface.SetTitle(title);

                inputBuffer.Process();

                if (OnUserUpdate(elapsedTime) == false) {
                    AtomActive = false;
                }


                displaySurface.DrawSurface();                
            }

            OnUserDestroy();
        }

        public abstract bool OnUserCreate();
        public abstract bool OnUserUpdate(TimeSpan ElapsedTime);
        public abstract void OnUserDestroy();
    }
}
