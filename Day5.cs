namespace AoC2023;

public class Day5 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var min = long.MaxValue;
        var inputArr = input.ToArray();

        var seedsTypes = inputArr[0].Split(": ")[1].Split(' ').Select(long.Parse);
        var (sourceToDestination, states) = BuildMapAndStatesList(inputArr);

        foreach (var seedType in seedsTypes)
        {
            var currentState = "seed";
            var currentType = seedType;

            long nextType = 0;
            for (int i = 0; i < states.Count - 1; i++)
            {
                var nextState = states[i + 1];
                var typeRanges = sourceToDestination[$"{currentState}-to-{nextState}"]!;

                var existingSourceRange = typeRanges.FirstOrDefault(
                    tr => currentType >= tr.Source && currentType <= tr.Source + tr.Count
                );
                if (existingSourceRange != default)
                {
                    nextType =
                        existingSourceRange.Destination + currentType - existingSourceRange.Source;
                }
                else
                {
                    nextType = currentType;
                }

                currentState = nextState;
                currentType = nextType;
            }
            if (nextType < min)
            {
                min = nextType;
            }
        }
        return min;
    }

    // 🤕!
    public static long SolvePart2(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();

        List<List<MapEntry>> maps = new();
        var seedRanges = inputArr[0].Split(": ")[1].Split(' ').Select(long.Parse);
        var seeds = seedRanges
            .Chunk(2)
            .Select(pair => new LongRange(pair[1], pair[0] + pair[1] - 1));

        for (int i = 2; i < inputArr.Length; i++)
        {
            var line = inputArr[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if (line.EndsWith(':'))
            {
                maps.Add(new List<MapEntry>());
                continue;
            }

            var dataStr = line.Split(' ');
            var destination = long.Parse(dataStr[0]);
            var source = long.Parse(dataStr[1]);
            var count = long.Parse(dataStr[2]);

            maps.Last().Add(new MapEntry(source, destination, count));
        }

        foreach (var item in maps)
        {
            Console.WriteLine(string.Join(',', item));
        }
        return long.MinValue;
    }

    public record struct MapEntry(long Source, long Destination, long Length) { }

    public record struct LongRange(long Min, long Max)
    {
        public static bool Contains(LongRange a, LongRange b) => a.Min <= b.Min && a.Max >= b.Max;

        public static bool Overlaps(LongRange a, LongRange b, out LongRange intersection)
        {
            var check = a.Max >= b.Min && a.Min <= b.Max;
            var limits = new[] { a.Min, a.Max, b.Min, b.Max }.Order().ToList();

            intersection = check ? new LongRange(limits[1], limits[2]) : default;

            return check;
        }
    }

    public record struct MapRule(long Source, long Destination, long Count);

    private static Tuple<Dictionary<string, List<MapRule>>, List<string>> BuildMapAndStatesList(
        string[] inputArr
    )
    {
        List<string> states = ["seed"];
        var sourceToDestination = new Dictionary<string, List<MapRule>>();
        var sourceToDestinationStr = "";
        for (int i = 2; i < inputArr.Length; i++)
        {
            var line = inputArr[i];
            if (string.IsNullOrEmpty(line))
            {
                sourceToDestinationStr = "";
                continue;
            }
            if (line.EndsWith(':'))
            {
                sourceToDestinationStr = line.Split(' ')[0];
                states.Add(sourceToDestinationStr.Split('-')[2]);
                continue;
            }

            var dataStr = line.Split(' ');
            var destination = long.Parse(dataStr[0]);
            var source = long.Parse(dataStr[1]);
            var count = long.Parse(dataStr[2]);
            var key = new MapRule(source, destination, count);
            if (sourceToDestination.TryGetValue(sourceToDestinationStr, out var existingList))
            {
                existingList.Add(key);
            }
            else
            {
                sourceToDestination[sourceToDestinationStr] = [key];
            }
        }

        return Tuple.Create(sourceToDestination, states);
    }
}
