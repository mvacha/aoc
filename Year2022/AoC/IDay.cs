public interface IDay
{
    int DayNumber { get; }
    object SolveFirst(string file);
    object SolveSecond(string file);
}