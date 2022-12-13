using System.Text.Json.Nodes;

public class Day13 : IDay
{
    public int DayNumber => 13;

    public int Compare(JsonNode left, JsonNode right) => (left, right) switch
    {
        (JsonArray l, JsonArray r)
            => l.Zip(r, Compare).FirstOrDefault(v => v != 0, 0) switch
            {
                0 => l.Count - r.Count,
                var i => i
            },

        (JsonValue l, JsonValue r) => l.GetValue<int>() - r.GetValue<int>(),
        (JsonArray l, JsonValue r) => Compare(l, new JsonArray(r.GetValue<int>())),
        (JsonValue l, JsonArray r) => Compare(new JsonArray(l.GetValue<int>()), r),
        _ => 0,
    };

    public IEnumerable<JsonNode> ParsePackets(string input) =>
        input.Split("\n")
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => JsonNode.Parse(line)!);

    public object SolveFirst(string inputFile)
    {
        var pairs = ParsePackets(File.ReadAllText(inputFile))
            .Chunk(2)
            .Select((pair, i) => Compare(pair[0], pair[1]) < 0 ? i + 1 : 0)
            .ToList();

        return pairs.Sum();
    }

    public object SolveSecond(string inputFile)
    {
        var dividers = ParsePackets("[[2]]\n[[6]]").ToList();
        var packets = ParsePackets(File.ReadAllText(inputFile)).Concat(dividers).ToList();

        packets.Sort(Compare);

        return (packets.IndexOf(dividers[0]) + 1) * (packets.IndexOf(dividers[1]) + 1);
    }
}