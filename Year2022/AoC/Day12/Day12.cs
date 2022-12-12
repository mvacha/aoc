using static SuperLinq.SuperEnumerable;

public class Day12 : IDay
{
    public int DayNumber => 12;

    private static (int X, int Y)[] Directions = new[] { (-1, 0), (1, 0), (0, 1), (0, -1) };
    private const char LOWEST = 'a';
    private const char HIGHEST = 'z';

    public record Coord(int X, int Y);
    public record Map(Dictionary<Coord, char> Heights, Coord Start, Coord End)
    {
        public IEnumerable<Coord> GetNeighbors(Coord start) => Directions
                .Select(d => new Coord(start.X + d.X, start.Y + d.Y))
                .Where(Heights.ContainsKey);

        public IEnumerable<Coord> GetLowestPoints()
            => Heights.Where(p => p.Value == LOWEST).Select(p => p.Key);
    }

    public Map ParseMap(IList<string> lines)
    {
        var heights = new Dictionary<Coord, char>();

        for (int y = 0; y < lines.Count; y++)
            for (int x = 0; x < lines[y].Length; x++)
                heights.Add(new(x, y), lines[y][x]);

        var start = heights.First(p => p.Value == 'S').Key;
        var end = heights.First(p => p.Value == 'E').Key;

        heights[start] = LOWEST;
        heights[end] = HIGHEST;

        return new(heights, start, end);
    }

    public object SolveFirst(string inputFile)
    {
        var map = ParseMap(File.ReadAllLines(inputFile));

        var neighborsUp = (Coord start)
            => map.GetNeighbors(start).Where(p => map.Heights[p] - map.Heights[start] <= 1);

        var path = GetShortestPath<Coord, int>(
                start: map.Start, 
                end: map.End,
                getNeighbors: (p, c) => neighborsUp(p).Select(p => (p, c + 1)));

        return path.Last().cost;
    }

    public object SolveSecond(string inputFile)
    {
        var map = ParseMap(File.ReadAllLines(inputFile));

        var neighborsDown = (Coord start)
            => map.GetNeighbors(start).Where(p => map.Heights[start] - map.Heights[p] <= 1); //different compared to neighborsUp

        var paths = GetShortestPaths<Coord, int>(
            start: map.End,
            getNeighbors: (p, c) => neighborsDown(p).Select(p => (p, c + 1)));

        var minCost = map.GetLowestPoints()
            .Where(paths.ContainsKey)
            .Select(start => paths[start].cost)
            .Min();

        return minCost;
    }
}