using SuperLinq;

public class Day12 : IDay
{
    public int DayNumber => 12;

    public record Coord(int x, int y);
    public record Map(char[][] Heights, Coord Start, Coord End)
    {
        public IEnumerable<Coord> GetNeighbors(Coord point) => new[] { (-1, 0), (1, 0), (0, 1), (0, -1) }
                .Select(dir => new Coord(point.x + dir.Item1, point.y + dir.Item2))
                .Where(p => 0 <= p.x && p.x < Heights[0].Length && //inside map x
                            0 <= p.y && p.y < Heights.Length && //inside map y
                            Heights[point.y][point.x] - Heights[p.y][p.x] <= 1); //max 1 step up

        public IEnumerable<Coord> GetLowestPoints()
        {
            for (int i = 0; i < Heights[0].Length; i++)
                for (int j = 0; j < Heights.Length; j++)
                    if (Heights[j][i] == 'a')
                        yield return new (i, j);
        }
    }

    public Map ParseMap(IEnumerable<string> lines)
    {
        Coord start = default!;
        Coord end = default!;

        var heights = lines.Select((l,row) =>
        {
            var startX = l.IndexOf('S');
            if (startX != -1)
                start = new(startX, row);

            var endX = l.IndexOf('E');
            if (endX != -1)
                end = new(endX, row);
            
            return l.ToCharArray();
        }).ToArray();

        heights[start.y][start.x] = 'a';
        heights[end.y][end.x] = 'z';

        return new(heights, start, end);
    }

    public object SolveFirst(string inputFile)
    {
        var map = ParseMap(File.ReadAllLines(inputFile));
        var cost = SuperEnumerable.GetShortestPathCost<Coord, int>(
                start: map.End,
                end: map.Start,
                getNeighbors: (point, cost) => map.GetNeighbors(point).Select(p => (p, cost + 1)));

        return cost;
    }

    public object SolveSecond(string inputFile)
    {
        var map = ParseMap(File.ReadAllLines(inputFile));

        var paths = SuperEnumerable.GetShortestPaths<Coord, int>(
                start: map.End,
                getNeighbors: (point, cost) => map.GetNeighbors(point).Select(p => (p, cost + 1)));

        var lowest = map.GetLowestPoints().ToList();
        var minCost = lowest.Where(paths.ContainsKey).Select(start => paths[start].cost).Min();

        return minCost;
    }
}

public static class LinqExtensions
{
    public static IEnumerable<T> Select<T>(this T[][] values) =>
        values.SelectMany(v => v.AsEnumerable());
} 