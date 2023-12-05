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
                if (existingSourceRange is not null)
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
    public static long SolvePart2(IEnumerable<string> input) => long.MinValue;

    public record MapRule(long Source, long Destination, long Count);

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
