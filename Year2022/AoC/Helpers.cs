public static class Helpers
{
    public static void PrintResults(IDay day, string file) => Console.WriteLine($"""
        DAY #{day.DayNumber} 
        First part: {day.SolveFirst(file)}
        Second part: {day.SolveSecond(file)}

        """);

    public static void CreateDay(int day)
    {
        var dayName = $"Day{day:00}";
        var rootDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!;
        var dayDir = Path.Combine(rootDir.FullName, dayName);

        if (Directory.Exists(dayDir)) return;

        //Create dir
        var dir = Directory.CreateDirectory(dayDir);

        //Create files
        var csFileContent = $$"""
        public class {{dayName}} : IDay
        {
            public int DayNumber => {{day}};

            public object SolveFirst(string inputFile)
            {
                return 0;
            }

            public object SolveSecond(string inputFile)
            {
                return 0;
            }
        }
        """;

        File.WriteAllText(Path.Combine(dayDir, dayName + ".cs"), csFileContent);
        File.WriteAllText(Path.Combine(dayDir, "input.txt"), "REPLACE ME");
        File.WriteAllText(Path.Combine(dayDir, "input.ref.txt"), "REPLACE ME");

        //Modify Program.cs
        var todayLine = $"PrintResults(new {dayName}(), \"./{dayName}/input.ref.txt\");\n";
        File.AppendAllText(Path.Combine(rootDir.FullName, "Program.cs"), todayLine);
    }


}
