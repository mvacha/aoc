public class Day09 : IDay
{
    public int DayNumber => 9;

    static (int x, int y) MoveSnake((int x, int y) start, char direction) => direction switch
    {
        'R' => (start.x + 1, start.y),
        'L' => (start.x - 1, start.y),
        'U' => (start.x, start.y + 1),
        'D' => (start.x, start.y - 1),
        _ => start
    };

    static (int x, int y) MoveTail((int x, int y) head, (int x, int y) tail)
    {
        if (Math.Abs(head.x - tail.x) > 1 || Math.Abs(head.y - tail.y) > 1)
            return (tail.x + Math.Sign(head.x - tail.x), tail.y + Math.Sign(head.y - tail.y));
        else
            return tail;
    }

    static int CountTailLocations(IEnumerable<string> lines, int snakeLength)
    {
        var snake = new (int x, int y)[snakeLength];
        var tailLocations = new HashSet<(int, int)>();

        foreach (var line in lines)
        {
            var count = int.Parse(line[2..]);

            for (int i = 0; i < count; i++)
            {
                snake[0] = MoveSnake(snake[0], line[0]);

                for (int j = 1; j < snakeLength; j++)
                    snake[j] = MoveTail(snake[j-1], snake[j]);

                tailLocations.Add(snake[^1]);
            }
        }

        return tailLocations.Count;
    }

    public object SolveFirst(string file) => CountTailLocations(File.ReadLines(file), 1+1);
    public object SolveSecond(string file) => CountTailLocations(File.ReadLines(file), 1+9);
}