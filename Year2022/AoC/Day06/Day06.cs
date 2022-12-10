using System.Data;
using System.Diagnostics;

public class Day06 : IDay
{
    public int DayNumber => 6;

    public static int FindStartOfPacketMarker(string input, int count)
        => input.IndexOf(input.Select((c, i) => input.Substring(i, count)).First(s => s.Distinct().Count() == count)) + count;

    public object SolveFirst(string file)
    {
        var assignment = File.ReadAllText(file);

        return FindStartOfPacketMarker(assignment, 4);
    }

    public object SolveSecond(string file)
    {
        var assignment = File.ReadAllText(file);

        return FindStartOfPacketMarker(assignment, 14);
    }
}