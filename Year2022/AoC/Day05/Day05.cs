using System.Data;

public class Day05 : IDay
{
    public int DayNumber => 5;

    public record Move(int Count, int From, int To);

    public record Assignment(Dictionary<int, Stack<char>> Stacks, List<Move> Moves)
    {
        public void EvaluateMovesSingleBox()
        {
            foreach (var move in Moves)
            {
                for (int i = 0; i < move.Count; i++)
                {
                    Stacks[move.To].Push(Stacks[move.From].Pop());
                }
            }
        }

        public void EvaluateMovesMultipleBoxes()
        {
            var tmp = new Stack<char>();

            foreach (var move in Moves)
            {
                for (int i = 0; i < move.Count; i++)
                    tmp.Push(Stacks[move.From].Pop());

                foreach (var box in tmp)
                    Stacks[move.To].Push(box);

                tmp.Clear();
            }
        }

        public static Assignment Parse(string inputFile)
        {
            var parts = inputFile.Split("\n\n");

            var stacks = ParseStacks(parts[0].Split('\n')[..^1]);
            var moves = parts[1].Split('\n').Select(ParseMove).ToList();

            return new(stacks, moves);
        }

        private static Move ParseMove(string move)
        {
            var parts = move.Split(' ');
            return new Move(int.Parse(parts[1]), int.Parse(parts[3]), int.Parse(parts[5]));
        }

        private static Dictionary<int, Stack<char>> ParseStacks(string[] stacksRaw)
        {
            var numStacks = (stacksRaw[0].Length + 1) / 4;
            var stacks = Enumerable.Range(1, numStacks).ToDictionary(i => i, i => new Stack<char>());

            foreach (var stack in stacksRaw.Reverse())
            {
                for (int i = 0; i < numStacks; i++)
                {
                    var box = stack[i * 4 + 1];

                    if (box != ' ')
                        stacks[i + 1].Push(box);
                }
            }

            return stacks;
        }

        public string GetTopCrates() => string.Concat(Stacks.Select(s => s.Value.Peek()));
    }

    public object SolveFirst(string file)
    {
        var assignment = Assignment.Parse(File.ReadAllText(file));
        assignment.EvaluateMovesSingleBox();

        return assignment.GetTopCrates();
    }

    public object SolveSecond(string file)
    {
        var assignment = Assignment.Parse(File.ReadAllText(file));
        assignment.EvaluateMovesMultipleBoxes();

        return assignment.GetTopCrates();
    }
}