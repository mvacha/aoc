using System.Diagnostics;

public class Day15 : IDay
{
    public int DayNumber => 15;

    private static Coord UP = new Coord(0, -1);
    private static Coord DOWN = new Coord(0, 1);
    private static Coord LEFT = new Coord(-1, 0);
    private static Coord RIGHT = new Coord(1, 0);

    public record Coord(int X, int Y)
    {
        public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
        public static Coord operator -(Coord a, Coord b) => new(a.X - b.X, a.Y - b.Y);
        public static Coord operator *(int s, Coord a) => new(s * a.X, s * a.Y);

        public static Coord Parse(string coords) => coords.Split(",") switch
        {
            [var x, var y] => new Coord(int.Parse(x), int.Parse(y)),
            _ => throw new Exception("Cannot parse")
        };

        public static int ManhattanDist(Coord a, Coord b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    public record Reading(Coord Sensor, Coord Beacon, int Distance)
    {
        public bool InRange(Coord pos) => Coord.ManhattanDist(Sensor, pos) <= Distance;
    }
    
    public record Rect(Coord TopLeft, int Height, int Width)
    {
        public Coord TopRight = TopLeft + (Width - 1) * RIGHT;
        public Coord BottomLeft = TopLeft + (Height - 1) * DOWN;
        public Coord BottomRight = TopLeft + (Height - 1) * DOWN + (Width - 1) * RIGHT;
        public Coord[] Corners => new[] { TopLeft, TopRight, BottomLeft, BottomRight };

        public IEnumerable<Rect> SplitToQuarters()
        {
            var newH1 = Height / 2;
            var newW1 = Width / 2;
            var newH2 = Height - newH1;
            var newW2 = Width - newW1;

            yield return new(TopLeft, newH1, newW1); //top left
            yield return new(TopLeft + newW1 * RIGHT, newH1, newW2); //top right
            yield return new(TopLeft + newH1 * DOWN, newH2, newW1); //bottom right
            yield return new(TopLeft + newH1 * DOWN + newW1 * RIGHT, newH2, newW2); //bottom right
        }

    }

    public static List<Reading> ParseReading(IEnumerable<string> lines)
    {
        var readings = new List<Reading>();

        foreach (var line in lines)
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            var sensor = new Coord(parts[0], parts[1]);
            var beacon = new Coord(parts[2], parts[3]);
            var reading = new Reading(sensor, beacon, Coord.ManhattanDist(sensor, beacon));

            readings.Add(reading);
        }

        return readings;
    }

    public object SolveFirst(string inputFile)
    {
        var readings = ParseReading(File.ReadAllLines(inputFile));

        var leftSensor = readings.MinBy(s => s.Sensor.X)!;
        var rightSensor = readings.MaxBy(s => s.Sensor.X)!;

        var left = leftSensor.Sensor.X - leftSensor.Distance;
        var right = rightSensor.Sensor.X + rightSensor.Distance;

        int y = (inputFile == "input.txt") ? 2000000 : 10;
        int count = 0;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int x = left; x <= right; x++)
        {
            var pos = new Coord(x, y);
            if (readings.Any(r => pos != r.Beacon && r.InRange(pos)))
                count++;
        }

        stopwatch.Stop();

        Console.WriteLine($"First part took: {stopwatch.Elapsed.Milliseconds}ms.");
        return count;
    }

    public Rect? FindUncovered(IList<Reading> readings, Rect rect)
    {
        foreach (var r in rect.SplitToQuarters().Where(r => r.Height > 0 && r.Width > 0))
        {
            if (readings.Any(s => r.Corners.All(s.InRange))) //all corners are covered by a single signal -> end this square
                continue;

            if (r.Height == 1 && r.Width == 1) //uncovered (we know it's 1 by 1)
                return r;

            var uncovered = FindUncovered(readings, r);
            if (uncovered != null)
                return uncovered; //not-null returned from recursion -> uncovered rect found
        }

        return null;
    }

    public object SolveSecond(string inputFile)
    {
        var readings = ParseReading(File.ReadAllLines(inputFile));

        int limit = inputFile.EndsWith("input.txt") ? 4000000 : 20;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var rect = FindUncovered(readings, new(new(0, 0), limit, limit))!;

        stopwatch.Stop();

        Console.WriteLine($"Second part took: {stopwatch.Elapsed.Milliseconds}ms.");

        return rect.TopLeft.X * 4000000L + rect.TopLeft.Y;
    }
}