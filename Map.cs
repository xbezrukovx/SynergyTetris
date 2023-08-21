using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Activation;
using System.Data;

namespace Tetris
{
    internal class Map
    {
        public static int width = 15, height = 20; // Размеры поля и размер клетки в пикселях
        public static int[,] field = new int[height, width]; // Массив для хранения поля
        public static int[,] fieldView = new int[height, width]; // Массив для хранения поля
        public static int spawnPosX = 0;
        public static int spawnPosY = 0;
        public Map() {
            initMap();
        }

        private void initMap()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[i, j] = 0;
                    fieldView[i, j] = 0;
                }
            }
        }

        public void renderMap()
        {
            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (fieldView[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(fieldView[i, j]);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }

        public bool isPossibleDraw(int px, int py, Shape shape)
        {
            lock (field)
            {
                if (py < 0) return false;
                if (py + 3 - shape.leftOffsetMax - shape.rightOffsetMax >= width) return false;
                if (px + 3 - shape.topOffsetMax - shape.bottomOffsetMax >= height) return false;


                for (int i = px; i <= px + 3 - shape.topOffsetMax - shape.bottomOffsetMax; i++)
                {
                    for (int j = py; j <= py + 3 - shape.leftOffsetMax - shape.rightOffsetMax; j++)
                    {
                        if (field[i, j] + shape.shape[i + shape.topOffsetMax - px, j + shape.leftOffsetMax - py] == 2) return false;
                    }
                }
            }
            return true;
        }

        public void drawView(int px, int py, Shape shape)
        {
            lock (field)
            {
                fieldView = (int[,])field.Clone();

                if (isPossibleDraw(px, py, shape))
                {
                    for (int i = px; i <= px + 3 - shape.topOffsetMax - shape.bottomOffsetMax; i++)
                    {
                        for (int j = py; j <= py + 3 - shape.leftOffsetMax - shape.rightOffsetMax; j++)
                        {
                            fieldView[i, j] += shape.shape[i + shape.topOffsetMax - px, j + shape.leftOffsetMax - py];
                        }
                    }
                }
            }
        }

        public void fixMap()
        {
            field = (int[,])fieldView.Clone();
            for (int i = 0; i < height; i++)
            {
                int sumLine = 0;
                for (int j = 0; j < width; j++)
                {
                    sumLine += field[i, j];
                }
                if (sumLine == width) destroyLine(i);
            }
        }

        public void destroyLine(int lineLevel)
        {
            for(int i = lineLevel; i > 0; i--)
            {
                for(int j = 0; j < width; j++)
                {
                    field[i,j] = field[i - 1,j];
                }
            }
        }
    }
}
