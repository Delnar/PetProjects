using GameLibraryLib;
using System;
using System.Runtime.InteropServices;

namespace ConsoleTest
{




    public class Program
    {
        static void Main(string[] args)
        {
            // var gl = new GameTester();
            var gl = new GameOfLife();
            gl.Start();
        }
    }
}
