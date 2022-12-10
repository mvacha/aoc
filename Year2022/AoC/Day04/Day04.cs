
public class Day04 : IDay
{
    public int DayNumber => 4;

    public record Assignments(int Start1, int End1, int Start2, int End2);

    public Assignments Parse(string sections)
    {
        var parts = sections.Split('-', ',').Select(int.Parse).ToArray();
        return new(parts[0], parts[1], parts[2], parts[3]);
    }

    public bool DoCompletelyOverlap(Assignments a)
        => (a.Start1 <= a.Start2 && a.End2 <= a.End1) || (a.Start2 <= a.Start1 && a.End1 <= a.End2);

    public bool DoOverlap(Assignments a) => Math.Max(a.Start1, a.Start2) <= Math.Min(a.End1, a.End2);

    public object SolveFirst(string file)
        => File.ReadAllLines(file).Select(Parse).Count(DoCompletelyOverlap);

    public object SolveSecond(string file)
        => File.ReadAllLines(file).Select(Parse).Count(DoOverlap);

}