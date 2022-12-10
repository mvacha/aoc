public class Day02 : IDay
{
    public int DayNumber => 2;

    // ROCK, PAPER, SCIZORS
    private static int EvalMove(string str) => (str[0], str[2]) switch
    {
        (var op, 'X') => 1 + op switch { 'A' => 3, 'B' => 0, _ => 6 },
        (var op, 'Y') => 2 + op switch { 'A' => 6, 'B' => 3, _ => 0 },
        (var op, 'Z') => 3 + op switch { 'A' => 0, 'B' => 6, _ => 3 },
        _ => throw new InvalidOperationException()
    };

    // ROCK, PAPER, SCIZORS
    // LOOSE, DRAW, WIN
    private static int EvalMoveDecr(string str) => (str[0], str[2]) switch
    {
        (var op, 'X') => 0 + op switch { 'A' => 3, 'B' => 1, _ => 2 },
        (var op, 'Y') => 3 + op switch { 'A' => 1, 'B' => 2, _ => 3 },
        (var op, 'Z') => 6 + op switch { 'A' => 2, 'B' => 3, _ => 1 },
        _ => throw new InvalidOperationException()
    };

    public object SolveFirst(string file)
        => File.ReadAllLines(file).Select(EvalMove).Sum();

    public object SolveSecond(string file)
        => File.ReadAllLines(file).Select(EvalMoveDecr).Sum();
}
