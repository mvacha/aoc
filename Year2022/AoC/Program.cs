using System.Reflection;

//PrintResults(new Day01(), "./Day01/input.txt");
//PrintResults(new Day02(), "./Day02/input.txt");
//PrintResults(new Day03(), "./Day03/input.txt");
//PrintResults(new Day04(), "./Day04/input.txt");
//PrintResults(new Day05(), "./Day05/input.txt");
//PrintResults(new Day06(), "./Day06/input.txt");
//PrintResults(new Day07(), "./Day07/input.txt");
//PrintResults(new Day08(), "./Day08/input.txt");
PrintResults(new Day09(), "./Day09/input.txt");



void PrintResults(IDay day, string file) => Console.WriteLine($"""
    DAY #{day.DayNumber} 
    First part: {day.SolveFirst(file)}
    Second part: {day.SolveSecond(file)}

    """);

public interface IDay
{
    int DayNumber { get; }
    object SolveFirst(string file);
    object SolveSecond(string file);
}