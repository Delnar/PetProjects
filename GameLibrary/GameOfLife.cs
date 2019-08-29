using ConsoleGameLibrary;
using GameLibraryLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    public class GameOfLife : GameLibrary
    {
        int[] Output;
        int[] State;

        ConsoleDisplaySurface con { get => displaySurface as ConsoleDisplaySurface; }
        ConsoleScreenBuffer bufScreen { get => con.bufScreen; }

        ConsolePixel WhitePixel { get; set; } = new ConsolePixel(ConsolePixel.PIXEL_SOLID, ConsolePixel.FG_WHITE);
        ConsolePixel BlackPixel { get; set; } = new ConsolePixel(ConsolePixel.PIXEL_SOLID, ConsolePixel.FG_BLACK);

        public GameOfLife(): base()
        {
            AppName = "Game of Life";
            displaySurface = new ConsoleDisplaySurface();
            displaySurface.Construct(160, 100, 8, 8);
            inputBuffer = new ConsoleInputBuffer();

        }

        public override bool OnUserCreate()
        {
            var r = new Random();
            Output = new int[ScreenSize];
            State = new int[ScreenSize];

            for(int i=0; i< ScreenSize; i++) {
                // State[i] = r.Next() % 1;
                State[i] = 0;
            }

            // SetState(80, 50, "  ## ");
            // SetState(80, 51, " ##  ");
            // SetState(80, 52, "  #  ");

            // Infinite Growth
            SetState(20, 50, "########.#####...###......#######.#####");

            return true;
        }

        public override void OnUserDestroy()
        {
            
        }

        public override bool OnUserUpdate(TimeSpan ElapsedTime)
        {
            for (int i = 0; i < ScreenSize; i++)
                Output[i] = State[i];
            for (int x = 1; x < ScreenWidth - 1; x++) {
                for (int y = 1; y < ScreenHeight - 1; y++) {
                    // The secret of artificial life =================================================
                    int nNeighbours = cell(x - 1, y - 1) + cell(x - 0, y - 1) + cell(x + 1, y - 1) +
                                        cell(x - 1, y + 0) + 0 + cell(x + 1, y + 0) +
                                        cell(x - 1, y + 1) + cell(x + 0, y + 1) + cell(x + 1, y + 1);

                    if (cell(x, y) == 1)
                        State[y * ScreenWidth + x] = nNeighbours == 2 || nNeighbours == 3 ? 1 : 0;
                    else
                        State[y * ScreenWidth + x] = nNeighbours == 3 ? 1 : 0;
                    // ===============================================================================

                    // bufScreen.Fill(20, 20, 30, 30, PIXEL_TYPE.PIXEL_SOLID, COLOUR.BG_DARK_GREEN);

                    if (cell(x, y) == 1)
                        bufScreen.Draw(x, y, WhitePixel);
                    else
                        bufScreen.Draw(x, y, BlackPixel);
                }
            }

            return true;
        }

        private void SetState(int x, int y, string s)
        {
            for (int i = 0;i <s.Length; i++) {
                State[y * ScreenWidth + x + i] = s[i] == '#' ? 1 : 0;
            }
        }

        private int cell(int x, int y)
        {
            return Output[y * ScreenWidth + x];
        }

        private void SetupGun(int x, int y)
        {
            SetState(0 + x, y + 0, "........................#............");
            SetState(0 + x, y + 1, "......................#.#............");
            SetState(0 + x, y + 2, "............##......##............##.");
            SetState(0 + x, y + 3, "...........#...#....##............##.");
            SetState(0 + x, y + 4, "##........#.....#...##...............");
            SetState(0 + x, y + 5, "##........#...#.##....#.#............");
            SetState(0 + x, y + 6, "..........#.....#.......#............");
            SetState(0 + x, y + 7, "...........#...#.....................");
            SetState(0 + x, y + 8, "............##.......................");
        }
    }
}
