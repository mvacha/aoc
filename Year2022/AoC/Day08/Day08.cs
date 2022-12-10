public class Day08 : IDay
{
    public int DayNumber => 8;

    int countTreesVisible(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        var left = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            int tallest = -1;

            for (int j = 0; j < cols; j++)
            {
                if (grid[i,j] > tallest)
                {
                    tallest = grid[i,j];
                    left[i,j] = 1;
                }
            }
        }


        var top = new int[rows, cols];
        for (int j = 0; j < cols; j++)
        {
            int tallest = -1;

            for (int i = 0; i < rows; i++)
            {
                if (grid[i, j] > tallest)
                {
                    tallest = grid[i, j];
                    top[i, j] = 1;
                }
            }
        }

        var right = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            int tallest = -1;

            for (int j = cols - 1; j >= 0; j--)
            {
                if (grid[i, j] > tallest)
                {
                    tallest = grid[i, j];
                    right[i, j] = 1;
                }
            }
        }

        var bottom = new int[rows, cols];
        for (int j = 0; j < cols; j++)
        {
            int tallest = -1;

            for (int i = rows - 1; i >= 0; i--)
            {
                if (grid[i, j] > tallest)
                {
                    tallest = grid[i, j];
                    bottom[i, j] = 1;
                }
            }
        }

        var count = 0;
        var visible = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (top[i, j] == 1 || right[i, j] == 1 || bottom[i, j] == 1 || left[i, j] == 1)
                {
                    count++;
                }
            }
        }

        return count;
    }

    int maxVisibilityScore(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        var maxVisibility = 0;
        for (int i = 1; i < rows - 1; i++)
        {
            for (int j = 1; j < cols - 1; j++)
            {
                var right = 0;
                for (int r = j + 1; r < cols; r++)
                {
                    if (grid[i, r] >= grid[i, j]) {
                        right++;
                        break;
                    }

                    right++;
                }

                var left = 0;

                 

                for (int l = j - 1; l >= 0; l--)
                {
                    if (grid[i, l] >= grid[i, j])
                    {
                        left++;
                        break;
                    }

                    left++;
                }

                var top = 0;
                for (int t = i - 1; t >= 0; t--)
                {
                    if (grid[t, j] >= grid[i, j])
                    {
                        top++;
                        break;
                    }

                    top++;
                }

                var bottom = 0;
                for (int b = i + 1; b < rows; b++)
                {
                    if (grid[b, j] >= grid[i, j])
                    {
                        bottom++;
                        break;
                    }

                    bottom++;
                }

                var visibility = right * left * top * bottom;
                
                if (visibility > maxVisibility)
                    maxVisibility = visibility;

            }
        }



        return maxVisibility;
    }

    public int [,] ParseInput(IEnumerable<string> lines)
    {
        var side = lines.First().Length;
        var grid = new int[side, side];

        int i = 0;
        foreach(var line in lines)
        {
            for (int j = 0; j < side; j++)
            {
                grid[i, j] = int.Parse(line[j].ToString());
            }
            i++;
        }

        return grid;
    }

    public object SolveFirst(string file)
    {
        var grid = ParseInput(File.ReadLines(file).ToList());
        int count = countTreesVisible(grid);

        return count;
    }

    public object SolveSecond(string file)
    {
        var grid = ParseInput(File.ReadLines(file).ToList());
        int score = maxVisibilityScore(grid);

        return score;
    }
}