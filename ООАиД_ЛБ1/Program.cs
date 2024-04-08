using System;

namespace ООАиД_ЛБ1
{
    using System.Timers;
    using static System.Console;
    using System.Collections.Generic;

    static class Tetris
    {
        enum CellStates { Empty, Dead, Alive }
        enum Movement { Left, Down, Right, RotLeft, RotRight }

        static float SpeedUp = 0.8f;
        static float Delay = 1000;
        const int Height = 22;
        const int Width = 10;

        static CellStates[,] World = new CellStates[Width, Height];
        static Random Rand = new Random();
        static Timer Timer = new Timer();

        static int[,] Pieces = new int[,] { { 0, 1, 1, 1, 2, 1, 3, 1 }, { 0, 0, 0, 1, 1, 1, 2, 1 }, { 0, 1, 1, 1, 2, 0, 2, 1 },
        { 1, 0, 2, 0, 1, 1, 2, 1 }, { 0, 1, 1, 0, 1, 1, 2, 0 }, { 0, 1, 1, 0, 1, 1, 2, 1 }, { 0, 0, 1, 0, 1, 1, 2, 1 } };

        public static void ddd()
        {
            CursorVisible = false;
            Title = "TETRIS";
            SetWindowSize(Width * 2, Height + 1);

            System.Threading.Tasks.Task.Run((Action)PlaySong);

            SpawnPiece();

            Draw();

            Timer.Elapsed += Tick;
            Timer.Interval = Delay;
            Timer.Start();

            ConsoleKeyInfo cki;
            while ((cki = ReadKey(true)) != null)
                Input(cki.Key);
        }

        static void Input(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Q:
                    break;
                case ConsoleKey.E:
                    break;
                case ConsoleKey.A:
                    MovePiece(Movement.Left);
                    break;
                case ConsoleKey.S:
                    Tick(null, null);
                    break;
                case ConsoleKey.D:
                    MovePiece(Movement.Right);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(Environment.ExitCode);
                    break;
            }
        }

        static void SpawnPiece()
        {
            int rand = Rand.Next(7);
            bool exit = false;

            for (int i = 0; i < 8; i += 2)
            {
                if (World[Pieces[rand, i] + Width / 2 - 2, Pieces[rand, i + 1]] != CellStates.Empty)
                    exit = true;
                else
                    World[Pieces[rand, i] + Width / 2 - 2, Pieces[rand, i + 1]] = CellStates.Alive;
            }

            Draw();

            if (exit) Environment.Exit(Environment.ExitCode);
        }

        static void Draw()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    sb.Append(World[x, y] == CellStates.Empty ? "░░" : "██");
                sb.AppendLine();
            }

            Clear();
            Write(sb.ToString());
        }

        static void KillAll()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (World[x, y] == CellStates.Alive)
                        World[x, y] = CellStates.Dead;

            RemoveLines();
            SpawnPiece();
        }

        static void RemoveLines()
        {
            for (int y = 0; y < Height; y++)
            {
                int filled = 0;
                for (int x = 0; x < Width; x++)
                    if (World[x, y] != CellStates.Empty)
                        filled++;

                if (filled == Width)
                {
                    Delay *= SpeedUp;

                    for (int i = y - 1; i >= 0; i--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            if (World[x, i] != CellStates.Empty)
                                World[x, i + 1] = CellStates.Dead;
                            World[x, i] = CellStates.Empty;
                        }
                    }
                }

            }
        }

        static void Tick(object sender, ElapsedEventArgs e)
        {
            MovePiece(Movement.Down);

            Timer.Interval = Delay;
            Timer.Start();
        }

        static void MovePiece(Movement direction)
        {
            HashSet<Point> toUpdate = new HashSet<Point>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (World[x, y] == CellStates.Alive)
                    {
                        if (y == Height - 1 || World[x, y + 1] == CellStates.Dead)
                        {
                            KillAll();
                            toUpdate = new HashSet<Point>();
                            break;
                        }
                        else
                        {
                            toUpdate.Add(new Point(x, y));
                        }
                    }
                }
            }

            int xOff = 0;
            int yOff = 0;

            switch (direction)
            {
                case Movement.Left:
                    xOff = -1;
                    break;
                case Movement.Down:
                    yOff = 1;
                    break;
                case Movement.Right:
                    xOff = 1;
                    break;

            }

            bool valid = true;
            foreach (Point point in toUpdate)
            {
                valid = point.x + xOff >= 0 && point.x + xOff < Width && point.y + yOff >= 0 && point.y + yOff < Height;
                if (!valid) break;
            }

            if (valid)
            {
                foreach (Point point in toUpdate)
                    World[point.x, point.y] = CellStates.Empty;
                foreach (Point point in toUpdate)
                    World[point.x + xOff, point.y + yOff] = CellStates.Alive;

                Draw();
            }
        }

        //https://gist.github.com/XeeX/6220067
        static void PlaySong()
        {
            Beep(1320, 500); Beep(990, 250); Beep(1056, 250); Beep(1188, 250); Beep(1320, 125); Beep(1188, 125); Beep(1056, 250); Beep(990, 250); Beep(880, 500); Beep(880, 250); Beep(1056, 250); Beep(1320, 500); Beep(1188, 250); Beep(1056, 250); Beep(990, 750); Beep(1056, 250); Beep(1188, 500); Beep(1320, 500); Beep(1056, 500); Beep(880, 500); Beep(880, 500); System.Threading.Thread.Sleep(250); Beep(1188, 500); Beep(1408, 250); Beep(1760, 500); Beep(1584, 250); Beep(1408, 250); Beep(1320, 750); Beep(1056, 250); Beep(1320, 500); Beep(1188, 250); Beep(1056, 250); Beep(990, 500); Beep(990, 250); Beep(1056, 250); Beep(1188, 500); Beep(1320, 500); Beep(1056, 500); Beep(880, 500); Beep(880, 500); System.Threading.Thread.Sleep(500); PlaySong();
        }

        struct Point
        {
            public int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
    class Program
    {
        private const int TIME_OF_SLEEP = 20;
        static void Main(string[] args)
        {
            string someMes = "Loremus muchachas. Here come a lot of churkas. We must run!!";
            Print(someMes);
            Thread.Sleep(1000);
            Delete();
            Print();
            Tetris.ddd();
        }
        static async void Print(string input = "Hello, World!")
        {
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
                Thread.Sleep(TIME_OF_SLEEP);
            }
        }
        static void Delete()
        {
            while (Console.CursorLeft != 0)
            {
                Thread.Sleep(TIME_OF_SLEEP / 2);
                Console.Write("\b ");
                Console.CursorLeft = --Console.CursorLeft;
            }
        }
    }
}
