public class Day10 : IDay
{
    public int DayNumber => 10;

    private IEnumerable<int> ExecuteInstructions(IEnumerable<string> instructions)
    {
        var x = 1;

        foreach (var instruction in instructions)
        {
            var name = instruction[..4];
            if (name == "noop")
            {
                yield return x;
            }
            else if (name == "addx")
            {
                yield return x;
                yield return x;
                x += int.Parse(instruction[4..]);
            }
        }
    }

    public object SolveFirst(string file)
    {
        var samples = new[] { 20, 60, 100, 140, 180, 220 };
        var signalSum = ExecuteInstructions(File.ReadLines(file))
            .Select((x, i) => samples.Contains(i + 1) ? x * (i + 1) : 0) //not sampled cycle => return 0
            .Sum();

        return signalSum;
    }

    public object SolveSecond(string file)
    {
        int pixel = 0;
        foreach (var x in ExecuteInstructions(File.ReadLines(file)))
        {
            Console.Write((x - 1) <= pixel && pixel <= (x + 1) ? '#' : '.');
            pixel++;

            if (pixel == 40)
            {
                pixel = 0;
                Console.WriteLine();
            }
        }

        return "";
    }
}