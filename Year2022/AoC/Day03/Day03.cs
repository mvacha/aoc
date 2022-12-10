using System.Linq;

public class Day03 : IDay
{
    public int DayNumber => 3;

    public static char GetDuplicatedItem(string rucksack)
        => rucksack[..(rucksack.Length / 2)].Intersect(rucksack[(rucksack.Length / 2)..]).First();

    public static int ScoreItem(char item) => item switch
    {
        >= 'a' and <= 'z' => item - 'a' + 1,
        >= 'A' and <= 'Z' => item - 'A' + 27,
        _ => 0
    };

    public object SolveFirst(string file)
        => File.ReadAllLines(file).Select(GetDuplicatedItem).Select(ScoreItem).Sum();

    public object SolveSecond(string file)
        => File.ReadAllLines(file).Chunk(3).Select(group => ScoreItem(group[0].Intersect(group[1]).Intersect(group[2]).First())).Sum();
}