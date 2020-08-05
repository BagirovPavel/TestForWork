using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectLife
{
    public class CalcForProjectLife
    {
        public static bool AnalizeGame(bool[,] current, bool[,] last, int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                    if (current[i,j] != last[i, j])
                        return false;
            
            return true;
        }

        public static string ArrayToString(bool[,] arr, int columns, int rows)
        {
            string result = "";
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                    if (arr[i, j])
                        result += "1";
                    else
                        result += "0";
            }

            return result;
        }

        public static bool[,] StringToArray(string str, int columns, int rows)
        {
            bool[,] result = new bool[columns, rows];
            int k = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (str[k] == '1')
                        result[i, j] = true;
                    else
                        result[i, j] = false;

                    k++;
                }
            }

            return result;
        }

        public static int CheckNeighbours(bool[,] arrIn, bool universeIsLocked, int x, int y, int columns, int rows)
        {
            bool isSelf;
            bool hasLife;
            int col;
            int row;
            int count = 0;
            if (!universeIsLocked)
            {
                //int xmin = Math.Max(x - 1, 0),
                //    xmax = Math.Min(x + 2, columns),
                //    ymin = Math.Max(y - 1, 0),
                //    ymax = Math.Min(y + 2, rows);

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        col = x + i;
                        row = y + j;

                        isSelf = col == x && row == y;
                        if (col < 0 || col >= columns || row < 0 || row >= rows)
                            hasLife = false;
                        else
                            hasLife = arrIn[col, row];

                        if (hasLife && !isSelf)
                            count++;
                    }
                }
            }
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        col = (x + i + columns) % columns;
                        row = (y + j + rows) % rows;

                        isSelf = col == x && row == y;
                        hasLife = arrIn[col, row];

                        if (hasLife && !isSelf)
                            count++;
                    }
                }
            }
            return count;
        }

        public static bool CheckIsAlive(int neighbours, bool isAlive)
        {
            bool result;

            if (!isAlive && neighbours == 3)
                result = true;
            else if (isAlive && (neighbours < 2 || neighbours > 3))
                result = false;
            else
                result = isAlive;

            return result;
        }
        public static Rectangle GetRectangle(bool isAlive, int resolution)
        {
            Rectangle rect = new Rectangle();
            if (isAlive)
            {
                rect.Height = resolution;
                rect.Width = resolution;
                rect.Fill = Brushes.Green;
                rect.Stroke = Brushes.Green;
            }
            else
            {
                rect.Height = resolution;
                rect.Width = resolution;
                rect.Fill = Brushes.Black;
                rect.Stroke = Brushes.Black;
            }
            return rect;
        }
    }
}
