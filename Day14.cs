using System.Text;

namespace AoC2023;

public class Day14 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var rows = input.ToArray();
        var cols = Rotate(rows);

        cols = Tilt(cols);
        return CalculateLoad(Rotate(cols), cols);
    }

    private static string[] Rotate(string[] rows)
    {
        var cols = new string[rows[0].Length];

        var sb = new StringBuilder();
        for (int c = 0; c < rows[0].Length; c++)
        {
            for (int r = 0; r < rows.Length; r++)
            {
                sb.Append(rows[r][c]);
            }
            cols[c] = sb.ToString();
            sb.Clear();
        }
        return cols;
    }

    private static void Print(string[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Console.WriteLine(elements[i]);
        }
        Console.WriteLine();
    }

    private static int CalculateLoad(string[] rows, string[] cols)
    {
        var sum = 0;
        for (int r = 0; r < rows.Length; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                if (cols[c][r] == 'O')
                {
                    sum += (rows.Length - r);
                }
            }
        }

        return sum;
    }

    private static string[] Tilt(string[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            var arr = elements[i].Split('#').ToArray();
            var newArr = new string[arr.Length];
            for (int j = 0; j < arr.Length; j++)
            {
                var roundedRocksIndices = arr[j]
                    .Select((c, index) => new { Character = c, Index = index })
                    .Where(item => item.Character == 'O')
                    .Select(item => item.Index)
                    .ToArray();

                var sb = new StringBuilder();
                for (int k = 0; k < roundedRocksIndices.Length; k++)
                {
                    sb.Append('O');
                    arr[j] = arr[j].Remove(roundedRocksIndices[k] - k, 1);
                }
                arr[j] = sb.ToString() + arr[j];
                newArr[j] = arr[j];
                sb.Clear();
            }

            elements[i] = string.Join('#', newArr);
        }

        return elements;
    }

    // Something is wrong here 🥱
    public static long SolvePart2(IEnumerable<string> input)
    {
        var rows = input.ToArray();

        Dictionary<string, int> cache = [];

        Print(rows);
        const int iterationsCount = 1_000_000_000;
        int i = 0;

        while (i < iterationsCount)
        {
            i++;
            // North
            var cols = Tilt(Rotate(rows)); // -> cols
            rows = Rotate(cols);

            // West
            cols = Tilt(rows); // -> rows
            rows = cols;

            // South
            cols = Tilt(Rotate(rows).Reverse().ToArray()); // -> reversed cols
            rows = Rotate(cols.Reverse().ToArray());

            // East
            cols = Tilt(rows.Reverse().ToArray()); // -> reversed row
            rows = cols.Reverse().ToArray();

            var key = string.Join('\n', rows);
            if (cache.TryGetValue(key, out var cycleStart))
            {
                var distanceFromStart = i - cycleStart;
                var cycleCount = (iterationsCount - i) / distanceFromStart;
                i += cycleCount * distanceFromStart;
            }

            cache[key] = i;
        }

        Print(rows);
        return CalculateLoad(rows, Rotate(rows));
    }
}
