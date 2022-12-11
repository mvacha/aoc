using static Helpers;

if (DateTime.Today is { Month: 12, Day: <= 25 })
    CreateDay(DateTime.Today.Day);

//PrintResults(new Day01(), "./Day01/input.txt");
//PrintResults(new Day02(), "./Day02/input.txt");
//PrintResults(new Day03(), "./Day03/input.txt");
//PrintResults(new Day04(), "./Day04/input.txt");
//PrintResults(new Day05(), "./Day05/input.txt");
//PrintResults(new Day06(), "./Day06/input.txt");
//PrintResults(new Day07(), "./Day07/input.txt");
//PrintResults(new Day08(), "./Day08/input.txt");
//PrintResults(new Day09(), "./Day09/input.txt");
//PrintResults(new Day10(), "./Day10/input.txt");
PrintResults(new Day11(), "./Day11/input.txt");
