namespace AoC2023;

public class Day12 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input) =>
        input
            .Parse()
            .Sum(
                r =>
                    GetArrangmentsCount(
                        r.Format,
                        r.Counts,
                        new Dictionary<(string, string), long>()
                    )
            );

    public static long SolvePart2(IEnumerable<string> input) =>
        input
            .Parse(true)
            .Sum(
                f =>
                    GetArrangmentsCount(
                        f.Format,
                        f.Counts,
                        new Dictionary<(string, string), long>()
                    )
            );

    #region DP

    private static long GetArrangmentsCount(
        string format,
        int[] counts,
        Dictionary<(string, string), long> cache
    )
    {
        long count;
        var key = (format, string.Join(',', counts));
        if (cache.ContainsKey(key))
        {
            return cache[key];
        }
        else if (format.Length == 0)
        {
            count = counts.Length == 0 ? 1 : 0;
        }
        else if (format[0] == '.')
        {
            count = GetArrangmentsCount(format[1..], counts, cache);
        }
        else if (format[0] == '#')
        {
            if (counts.Length == 0)
            {
                count = 0;
            }
            else if (format.Length < counts[0])
            {
                count = 0;
            }
            else if (format[..counts[0]].Any(c => c == '.'))
            {
                count = 0;
            }
            else if (counts.Length > 1)
            {
                if (
                    format.Length < counts[0] + 1
                    || format.Length > counts[0] && format[counts[0]] == '#'
                )
                {
                    count = 0;
                }
                else
                {
                    count = GetArrangmentsCount(format[(counts[0] + 1)..], counts[1..], cache);
                }
            }
            else
            {
                count = GetArrangmentsCount(format[counts[0]..], counts[1..], cache);
            }
        }
        else
        {
            // if (currChar == '?')
            count =
                GetArrangmentsCount('#' + format[1..], counts, cache)
                + GetArrangmentsCount('.' + format[1..], counts, cache);
        }

        cache.Add(key, count);
        return count;
    }
    #endregion

    #region Brute Force
    private static bool Matches(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length)
        {
            return false;
        }

        var i = 0;
        while (i < arr1.Length)
        {
            if (arr1[i] != arr2[i])
            {
                return false;
            }
            i++;
        }

        return true;
    }

    private static int[] GetContiguousHashesCounts(string input)
    {
        List<int> counts = new();
        int currentStreak = 0;

        foreach (char c in input)
        {
            if (c == '#')
            {
                currentStreak++;
            }
            else if (currentStreak > 0)
            {
                counts.Add(currentStreak);
                currentStreak = 0;
            }
        }

        if (currentStreak > 0)
        {
            counts.Add(currentStreak);
        }

        return counts.ToArray();
    }

    private static List<string> GenerateAllCombinations(string input)
    {
        List<string> combinations = new List<string>();

        int index = input.IndexOf('?');

        if (index == -1)
        {
            combinations.Add(input);
        }
        else
        {
            string withHash = input.Substring(0, index) + '#' + input.Substring(index + 1);
            string withDot = input.Substring(0, index) + '.' + input.Substring(index + 1);

            combinations.AddRange(GenerateAllCombinations(withHash));
            combinations.AddRange(GenerateAllCombinations(withDot));
        }

        return combinations;
    }
    #endregion
}

file readonly record struct Row(string Format, int[] Counts);

file static class Extensions
{
    public static IEnumerable<Row> Parse(this IEnumerable<string> input, bool unfold = false) =>
        input.Select(r =>
        {
            var arr = r.Split(' ');
            var format = arr[0];
            var counts = arr[1].Split(',').Select(int.Parse).ToArray();

            if (unfold)
            {
                format = string.Concat(format, '?', format, '?', format, '?', format, '?', format);
                counts = [..counts, ..counts, ..counts, ..counts, ..counts];
            }

            return new Row(format, counts);
        });
}
