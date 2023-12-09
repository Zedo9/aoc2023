namespace AoC2023;

public class Day9 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input) =>
        input.Aggregate(
            (long)0,
            (acc, line) =>
            {
                var numbers = line.Split(' ').Select(long.Parse).ToList();
                return acc += Extrapolate(BuildHistory(numbers), ExtrapolateDirection.Right);
            }
        );

    public static long SolvePart2(IEnumerable<string> input) =>
        input.Aggregate(
            (long)0,
            (acc, line) =>
            {
                var numbers = line.Split(' ').Select(long.Parse).ToList();
                return acc += Extrapolate(BuildHistory(numbers), ExtrapolateDirection.Left);
            }
        );

    private static List<long> ParseLine(string line) => line.Split(' ').Select(long.Parse).ToList();

    private static Stack<List<long>> BuildHistory(List<long> numbers)
    {
        Stack<List<long>> history = [];
        var lastDiffs = numbers;

        while (lastDiffs.Any(d => d != 0))
        {
            history.Push(lastDiffs);
            List<long> newDiffs = [];
            for (int i = 0; i < lastDiffs.Count - 1; i++)
            {
                var diff = lastDiffs[i + 1] - lastDiffs[i];
                newDiffs.Add(diff);
            }
            lastDiffs = newDiffs;
        }

        return history;
    }

    private static long Extrapolate(Stack<List<long>> history, ExtrapolateDirection direction)
    {
        long lastNum = 0;

        while (history.Count > 0)
        {
            var el = direction switch
            {
                ExtrapolateDirection.Right => history.Pop().Last(),
                ExtrapolateDirection.Left => history.Pop().First(),
                _ => throw new Exception("NEVER")
            };
            lastNum = el + (long)direction * lastNum;
        }

        return lastNum;
    }

    enum ExtrapolateDirection
    {
        Left = -1,
        Right = 1
    }
}
