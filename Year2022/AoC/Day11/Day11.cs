using System.Text.RegularExpressions;

public class Day11 : IDay
{
    public int DayNumber => 11;

    public record Monkey(int Id, Queue<long> Items, Func<long, long> Operation, int Divisor, int TrueMonkey, int FalseMonkey)
    {
        public long InspectedItems { get; private set; }

        public IEnumerable<(int target, long item)> InspectItems(Func<long, long> stressRelief)
        {
            while (Items.Any())
            {
                var item = Items.Dequeue();
                item = Operation(item);
                item = stressRelief(item);
                InspectedItems++;

                yield return (item % Divisor == 0 ? TrueMonkey : FalseMonkey, item);
            }
        }
    }

    public record PassingGame(IList<Monkey> Monkeys)
    {
        public void Run(int rounds, Func<long, long> stressRelief)
        {
            for (int i = 0; i < rounds; i++)
            foreach (var monkey in Monkeys)
            foreach (var (target, item) in monkey.InspectItems(stressRelief))
                Monkeys[target].Items.Enqueue(item);
        }

        public long MonkeyBusinessLevel()
            => Monkeys.OrderByDescending(m => m.InspectedItems).Take(2).Aggregate(1L, (acc, m) => acc * m.InspectedItems);
    }

    public static PassingGame ParseMonkeys(string input)
    {
        var pattern = "Monkey (\\d+):\\s*Starting items: (.*)\\s*Operation: new = old (.*)\\s*Test: divisible by (\\d+)\\s*If true: throw to monkey (\\d+)\\s*If false: throw to monkey (\\d+)";
        var regex = new Regex(pattern);
        var matches = regex.Matches(input);
        var monkeys = new List<Monkey>();

        foreach (Match match in matches)
        {
            var id = int.Parse(match.Groups[1].Value);
            var items = new Queue<long>(match.Groups[2].Value.Split(",").Select(long.Parse));

            Func<long, long> operation = match.Groups[3].Value.Trim().Split(' ') switch
            {
                ["*", "old"] => i => i * i,
                ["+", "old"] => i => i + i,
                ["*", var n] when int.TryParse(n, out var num) => i => i * num,
                ["+", var n] when int.TryParse(n, out var num) => i => i + num,
                _ => throw new NotImplementedException(),
            };

            var divisor = int.Parse(match.Groups[4].Value);
            var trueMonkey = int.Parse(match.Groups[5].Value);
            var falseMonkey = int.Parse(match.Groups[6].Value);

            monkeys.Add(new Monkey(id, items, operation, divisor, trueMonkey, falseMonkey));
        }

        return new(monkeys);
    }


    public object SolveFirst(string inputFile)
    {
        var game = ParseMonkeys(File.ReadAllText(inputFile));

        game.Run(20, stressRelief: i => i / 3);
        return game.MonkeyBusinessLevel();
    }

    public object SolveSecond(string inputFile)
    {
        var game = ParseMonkeys(File.ReadAllText(inputFile));

        var mod = game.Monkeys.Aggregate(1L, (acc, m) => acc * m.Divisor);
        game.Run(10000, stressRelief: i => i % mod);
        return game.MonkeyBusinessLevel();
    }
}