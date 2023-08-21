using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Program
    {
        static ConsoleKeyInfo key;
        static int posX = 6;
        static int posY = 0;
        static Shape activeShape = new Shape();
        static Map map = new Map();
        static int speed = 250;
        static bool isGameOver = false;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            while (true)
            {
                bool isPossible = false;
                activeShape = new Shape();
                map = new Map();
                map.drawView(posY, posX, activeShape);
                Thread inputThread = new Thread(Input);
                Thread downThread = new Thread(LetDown);
                inputThread.Start();
                downThread.Start();
                isGameOver = false;

                while (!isGameOver)
                {
                    if (key.Key != 0)
                    {
                        InputHandler();
                        key = new ConsoleKeyInfo(); // Reset input var
                    }
                    isPossible = map.isPossibleDraw(posY, posX, activeShape);
                    if (!isPossible)
                    {
                        map.fixMap();
                        activeShape = new Shape();
                        posX = 6; posY = 0;
                        isGameOver = !map.isPossibleDraw(posY, posX, activeShape);
                    }
                    else
                    {
                        map.drawView(posY, posX, activeShape);
                        map.renderMap();
                    }
                    Thread.Sleep(20);
                }
                inputThread.Abort();
                downThread.Abort();
                Console.Clear();
                Console.WriteLine("Press Enter to restart!");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void LetDown()
        {
            while (true)
            {
                posY++;
                Thread.Sleep(speed);
                if (speed < 250)
                {
                    speed+=50;
                }
            }
        }

        static void Input()
        {
            while (true)
            {
                if(key.Key == 0)
                    key = Console.ReadKey(true);
            }
        }

        static bool InputHandler()
        {
            if (key.Key == ConsoleKey.D && map.isPossibleDraw(posY, posX+1, activeShape))
            {
                posX++;
            }
            if (key.Key == ConsoleKey.A && map.isPossibleDraw(posY, posX-1, activeShape))
            {
                posX--;
            }
            if (key.Key == ConsoleKey.W)
            {
                int[,] tempShape = activeShape.rotate90();
                Shape newShape = new Shape(); 
                newShape.shape = tempShape;
                newShape.calculateOffsets();
                if (map.isPossibleDraw(posY,posX, newShape))
                {
                    activeShape.shape = (int[,])tempShape.Clone();
                    activeShape.calculateOffsets();
                }
            }
            if (key.Key == ConsoleKey.S)
            {
                speed = 0;
            }
            return key.Key != 0;
        }
    }
}
