using static Day14;

public class Day14 : IDay
{
    public int DayNumber => 14;
    private const char ROCK = '#';
    private const char SAND = 'o';

    private static Coord DOWN = new Coord(0, 1);
    private static Coord DOWN_LEFT = new Coord(-1, 1);
    private static Coord DOWN_RIGHT = new Coord(1, 1);

    public record Coord(int X, int Y)
    {
        public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
        public static Coord operator -(Coord a, Coord b) => new(a.X - b.X, a.Y - b.Y);
        public static Coord Parse(string coords) => coords.Split(",") switch
        {
            [var x, var y] => new Coord(int.Parse(x), int.Parse(y)),
            _ => throw new Exception("Cannot parse")
        };
    }

    public record Rocks(Dictionary<Coord, char> Map, bool HasFloor = false)
    {
        public Lazy<int> MaxDepth = new Lazy<int>(Map.Max(p => p.Key.Y));

        private bool AddGrain(Coord location)
        {
            while (true)
            {
                if (location.Y == MaxDepth.Value + 1) //floor location MaxDepth + 2
                {
                    if (HasFloor) Map.Add(location, SAND); //place on the floor

                    return HasFloor; //if doesn't have floor -> falling through
                }

                if (!Map.ContainsKey(location + DOWN)) //try down
                    location = location + DOWN;

                else if (!Map.ContainsKey(location + DOWN_LEFT)) //try down left
                    location = location + DOWN_LEFT;

                else if (!Map.ContainsKey(location + DOWN_RIGHT)) //try down right
                    location = location + DOWN_RIGHT;

                else //no direction possible => place here
                    return Map.TryAdd(location, SAND);
            }
        }

        public int PourSand(Coord location)
        {
            while (!Map.ContainsKey(location) && AddGrain(location)); //filled w. sand or sand falling through

            return Map.Count(p => p.Value == SAND);
        }
    }

    public static Rocks ParseMap(IEnumerable<string> lines)
    {
        var map = new Dictionary<Coord, char>();

        foreach (var line in lines)
        {
            var positions = line.Split("->").Select(Coord.Parse).ToArray();
            for (int i = 1; i < positions.Length; i++)
            {
                var from = positions[i - 1];
                var to = positions[i];
                var dir = new Coord(Math.Sign(to.X - from.X), Math.Sign(to.Y - from.Y));

                for (var p = from; p != to + dir; p+= dir)
                    map[p] = ROCK;
            }
        }

        return new(map);
    }

    public object SolveFirst(string inputFile)
    {
        var rocks = ParseMap(File.ReadAllLines(inputFile));
        var sandGrains = rocks.PourSand(new(500, 0));

        return sandGrains;
    }

    public object SolveSecond(string inputFile)
    {
        var rocks = ParseMap(File.ReadAllLines(inputFile)) with { HasFloor = true };
        var sandGrains = rocks.PourSand(new(500, 0));

        return sandGrains;
    }
}