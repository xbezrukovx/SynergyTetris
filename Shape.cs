using System;
using System.Drawing.Text;

public class Shape
{
    public int[,] shape = new int[4, 4]; // Массив для хранения падающей фигурки (для каждого блока 2 координаты [0, i] и [1, i]
    public int leftOffsetMax = 0;
    public int rightOffsetMax = 0;
    public int topOffsetMax = 0;
    public int bottomOffsetMax = 0;
    public Shape()
	{
        Random x = new Random(DateTime.Now.Millisecond);
        switch (x.Next(7))
        { // Рандомно выбираем 1 из 7 возможных фигурок
            case 0: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 }
            }; break;
            case 1: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 1, 0, 0 }, 
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }; break;
            case 2: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 1, 1, 0, 0 }, 
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }; break;
            case 3: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 }, 
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }; break;
            case 4: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 0, 1, 1 }, 
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }; break;
            case 5: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 1, 0, 0, 0 }, 
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }; break;
            case 6: shape = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 1 }, 
                { 0, 1, 1, 1 },
                { 0, 0, 0, 0 }
            }; break;
        }
        calculateOffsets();
    }

    public void calculateOffsets()
    {
        leftOffsetMax = findLeftOffsetMax();
        rightOffsetMax = findRightOffsetMax();
        topOffsetMax = findTopOffsetMax();
        bottomOffsetMax = findBottomOffsetMax();
    }

    private int findLeftOffsetMax()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (shape[j,i] == 1)
                    return i;
        return 0;
    }

    private int findBottomOffsetMax()
    {
        for (int i = 3; i >= 0; i--)
            for (int j = 0; j < 4; j++)
                if (shape[i, j] == 1)
                    return 3-i;
        return 0;
    }

    private int findRightOffsetMax()
    {
        for (int i = 3; i >= 0; i--)
            for (int j = 0; j <= 3; j++)
                if (shape[j,i] == 1)
                    return 3-i;
        return 0;
    }

    private int findTopOffsetMax()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (shape[i, j] == 1)
                    return i;
        return 0;
    }

    public int[,] rotate90()
    {
        int[,] tempShape = (int[,])shape.Clone();
        int N = 4;
        for (int i = 0; i < N / 2; i++)
        {
            for (int j = i; j < N - i - 1; j++)
            {
                int temp = tempShape[i, j];
                tempShape[i, j] = tempShape[N - 1 - j, i];
                tempShape[N - 1 - j, i] = tempShape[N - 1 - i, N - 1 - j];
                tempShape[N - 1 - i, N - 1 - j] = tempShape[j, N - 1 - i];
                tempShape[j, N - 1 - i] = temp;
            }
        }
        return tempShape;
    }
}
