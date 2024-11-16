using System;
namespace Sokoban
{
    class Program
    {
        static char[,] level =
        {
            {'┌','─','─','─','─','─','─','─','─','─','─','─','─','┐'}, // xxxxxxx
            {'|',' ',' ',' ',' ','|',' ',' ',' ',' ',' ',' ',' ','|'}, // xxxxxxx
            {'|',' ',' ','B',' ','|','B',' ',' ',' ',' ',' ',' ','|'}, // xxxxxxx
            {'|',' ',' ','─','─','┘','P',' ',' ',' ',' ','┌','─','|'}, // xxxxxxx
            {'|',' ',' ',' ',' ',' ',' ',' ',' ','|',' ','|','O','|'}, // xxxxxxx
            {'|','─',' ','─','┐',' ',' ',' ',' ','|',' ','|',' ','|'}, // xxxxxxx
            {'|',' ',' ',' ','|',' ',' ','┌','─','┘',' ',' ',' ','|'}, // xxxxxxx
            {'|','O',' ',' ','|',' ',' ','|',' ',' ',' ',' ',' ','|'}, // xxxxxxx
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|'}, // xxxxxxx
            {'└','─','─','─','─','─','─','─','─','─','─','─','─','┘'}  // xxxxxxx
        };
            //yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
        static int playerX = 6;
        static int playerY = 3;
        static char[,] originalLevel = (char[,])level.Clone();
        static int originalplayerX = 6;
        static int originalplayerY = 3;
        static int counter = 0;
        static void Main()
        {
            Console.WindowWidth = 60;
            Console.BufferWidth = 60;
            Console.WindowHeight = 25;
            Console.BufferHeight = 25;
            bool playing = true;
            while (playing)
            {
                Console.Clear();
                DrawLevel();
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W:
                        MovePlayer(0, -1); // вгору
                        break;
                    case ConsoleKey.S:
                        MovePlayer(0, 1);  // вниз
                        break;
                    case ConsoleKey.A:
                        MovePlayer(-1, 0); // вліво
                        break;
                    case ConsoleKey.D:
                        MovePlayer(1, 0);  // вправо
                        break;
                    case ConsoleKey.R:
                        ResetGame(); // рестарт
                        counter = 0;
                        break;
                    case ConsoleKey.Spacebar: // кінець
                        playing = false;
                        break;
                }
                if (LevelCompleted())
                {
                    Console.Clear();
                    DrawLevel();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ви перемогли!");
                    Console.ResetColor();
                    playing = false;
                }
            }
        }
        static void DrawLevel()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    Console.Write(level[x, y]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("P - player, B - box, O - place for box\nХодити:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("W - вгору\nS - вниз\nA - вліво\nD - вправо");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("R - перезапустити рівень");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Пробіл - завершити гру");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Кількість ходів:{counter}");
            Console.ResetColor();
        }
        static void MovePlayer(int dx, int dy)
        {
            int newX = playerX + dx;
            int newY = playerY + dy;
            if (level[newY, newX] == ' ' || level[newY, newX] == 'O')
            {
                RestoreOriginalPos(playerX, playerY); // бекап
                playerX = newX;
                playerY = newY;
                level[playerY, playerX] = 'P';
                counter++;
            }
            else if (level[newY, newX] == 'B')
            {
                // мувмент бокс
                int boxnewX = newX + dx;
                int boxnewY = newY + dy;
                if (level[boxnewY, boxnewX] == ' ' || level[boxnewY, boxnewX] == 'O')
                {
                    RestoreOriginalPos(playerX, playerY);
                    level[boxnewY, boxnewX] = 'B'; // бокс мув
                    playerX = newX;
                    playerY = newY;
                    level[playerY, playerX] = 'P';
                    counter++;
                }
            }
        }
        static void RestoreOriginalPos(int x, int y)
        {
            if (originalLevel[y, x] == 'O')
                level[y, x] = 'O'; 
            else
                level[y, x] = ' '; 
        }
        static bool LevelCompleted()
        {
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    if (originalLevel[x, y] == 'O' && level[x, y] != 'B')
                        return false;
                }
            }
            return true;
        }
        static void ResetGame()
        {
            level = (char[,])originalLevel.Clone();
            playerX = originalplayerX;
            playerY = originalplayerY;
        }
    }
}