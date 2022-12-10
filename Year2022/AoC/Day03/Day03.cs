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
        => File.ReadAllLines(file).SelectThree((l1, l2, l3) => ScoreItem(l1.Intersect(l2).Intersect(l3).First())).Sum();
}

public static class LinqExtensions

{
    public static IEnumerable<TResult> SelectThree<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource, TResult> selector)
    {
        using var iterator = source.GetEnumerator();

        while (iterator.MoveNext())
        {
            var item1 = iterator.Current;
            iterator.MoveNext();
            var item2 = iterator.Current;
            iterator.MoveNext();
            var item3 = iterator.Current;

            yield return selector(item1, item2, item3);
        }
    }
}