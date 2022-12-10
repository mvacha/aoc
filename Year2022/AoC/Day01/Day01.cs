public class Day01 : IDay
{
    public int DayNumber => 1;

    private static IEnumerable<int> GetCaloriesSums(string file)
        => File.ReadAllText(file).Split("\n\n").Select(s => s.Split("\n").Select(int.Parse).Sum());
    
    public object SolveFirst(string inputFile)
        => GetCaloriesSums(inputFile).Max();

    public object SolveSecond(string inputFile)
        => GetCaloriesSums(inputFile).OrderDescending().Take(3).Sum();
}
