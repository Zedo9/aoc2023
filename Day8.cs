namespace AoC2023;

public class Day8 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();

        char[] pattern;
        Dictionary<string, Tuple<string, string>> map = [];

        BuildMapAndPattern(input, out map, out pattern, out _);

        return FindStepsRequired("AAA", node => node == "ZZZ", map, pattern);
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();

        char[] pattern;
        Dictionary<string, Tuple<string, string>> map = [];
        List<string> nodesEndingWithA = [];

        BuildMapAndPattern(input, out map, out pattern, out nodesEndingWithA);

        // 🤔 For some reason, the input is cyclic, so this works
        return LCM(
            nodesEndingWithA
                .Select(n => FindStepsRequired(n, node => node.EndsWith('Z'), map, pattern))
                .ToArray()
        );
    }

    private static void BuildMapAndPattern(
        IEnumerable<string> input,
        out Dictionary<string, Tuple<string, string>> map,
        out char[] pattern,
        out List<string> nodesEndingWithA
    )
    {
        var inputArr = input.ToArray();
        pattern = inputArr[0].ToArray();
        nodesEndingWithA = new List<string>();
        map = [];

        for (int i = 2; i < inputArr.Length; i++)
        {
            var lineArr = inputArr[i].Split('=');
            var source = lineArr[0].Trim();
            var destinations = lineArr[1].Split(',');
            var destinationLeft = string.Join("", destinations[0].Skip(2));
            var destinationRight = string.Join("", destinations[1].Skip(1).Take(3));

            map.Add(source, Tuple.Create(destinationLeft, destinationRight));

            if (source.EndsWith('A'))
            {
                nodesEndingWithA.Add(source);
            }
        }
    }

    private static long FindStepsRequired(
        string start,
        Func<string, bool> hasReachedDestination,
        Dictionary<string, Tuple<string, string>> map,
        char[] pattern
    )
    {
        char direction;
        var currLocation = start;
        var patternPointer = 0;
        var jumps = 0;

        while (!hasReachedDestination(currLocation))
        {
            direction = pattern[patternPointer];
            var (left, right) = map[currLocation];
            currLocation = direction switch
            {
                'L' => left,
                _ => right
            };

            jumps++;
            patternPointer++;

            if (patternPointer >= pattern.Length)
            {
                patternPointer = 0;
            }
        }
        return jumps;
    }

    private static long LCM(long[] numbers)
    {
        long result = numbers[0];

        for (int i = 1; i < numbers.Length; i++)
        {
            result = LCM(result, numbers[i]);
        }

        return result;
    }

    private static long LCM(long a, long b)
    {
        return (a * b) / GCD(a, b);
    }

    private static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var tmp = b;
            b = a % b;
            a = tmp;
        }
        return a;
    }
}
