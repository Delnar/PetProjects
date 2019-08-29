using ConsoleGameLibrary;
using GameLibraryLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    public class GameTester : GameLibrary
    {
        ConsoleDisplaySurface con { get => displaySurface as ConsoleDisplaySurface; }
        ConsoleScreenBuffer bufScreen { get => con.bufScreen; }

        public GameTester() : base()
        {
            AppName = "Game Tester";
            displaySurface = new ConsoleDisplaySurface();
            displaySurface.Construct(160, 100, 8, 8);
            inputBuffer = new ConsoleInputBuffer();
        }

        public override bool OnUserCreate()
        {
            return true;
        }

        public override void OnUserDestroy()
        {
            
        }

        public override bool OnUserUpdate(TimeSpan ElapsedTime)
        {
            bufScreen.DrawString(0, 0, "Hello World");
            return true;
        }
    }
}
