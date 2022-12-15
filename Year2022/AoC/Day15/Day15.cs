using static Day15;

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

    public record Signal(Coord Left, Coord Top, Coord Right, Coord Bottom)
    {
        public static Signal FromSensor(Coord loc, int distance)
            => new(loc + (distance * LEFT), loc + (distance * UP), loc + (distance * RIGHT), loc + (distance * DOWN));
    }

    public record Reading(Coord sensor, Coord beacon, int Distance)
    {
        public bool InRange(Coord pos) => Coord.ManhattanDist(sensor, pos) <= Distance;
    }

    public object SolveFirst(string inputFile)
    {
        var signals = new List<Signal>();
        var readings = new List<Reading>();

        foreach (var line in File.ReadAllLines(inputFile))
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            var sensor = new Coord(parts[0], parts[1]);
            var beacon = new Coord(parts[2], parts[3]);
            var reading = new Reading(sensor, beacon, Coord.ManhattanDist(sensor, beacon));
            var signal = Signal.FromSensor(reading.sensor, reading.Distance);

            readings.Add(reading);
            signals.Add(signal);
        }

        var left = signals.Min(s => s.Left.X);
        var right = signals.Max(s => s.Right.X);

        int y = (inputFile == "input.txt") ? 2000000 : 10;
        int count = 0;
        for (int x = left; x <= right; x++)
        {
            var pos = new Coord(x, y);
            if (readings.Any(r => pos != r.beacon && r.InRange(pos)))
                count++;
        }

        return count;
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

    public Rect? FindUncovered(IList<Reading> readings, Rect rect)
    {
        foreach (var r in rect.SplitToQuarters().Where(r => r.Height > 0 && r.Width > 0))
        {
            if (readings.Any(s => r.Corners.All(s.InRange))) //all corners are covered by a single signal -> end this square
                continue;
            if (r.Height == 1 && r.Width == 1) //uncovered
                return r;

            var uncovered = FindUncovered(readings, r);
            if (uncovered != null)
                return uncovered;
        }

        return null;
    }

    public object SolveSecond(string inputFile)
    {
        var signals = new List<Signal>();
        var readings = new List<Reading>();

        foreach (var line in File.ReadAllLines(inputFile))
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            var sensor = new Coord(parts[0], parts[1]);
            var beacon = new Coord(parts[2], parts[3]);
            var reading = new Reading(sensor, beacon, Coord.ManhattanDist(sensor, beacon));
            var signal = Signal.FromSensor(reading.sensor, reading.Distance);

            readings.Add(reading);
            signals.Add(signal);
        }

        int limit = inputFile.EndsWith("input.txt") ? 4000000 : 20;
        var rect = FindUncovered(readings, new(new(0, 0), limit, limit));
        
        
        return rect.TopLeft.X * 4000000L + rect.TopLeft.Y;
    }
}