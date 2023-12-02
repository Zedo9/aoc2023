using System.Text;

namespace AoC2023;

public class Day1 : IDay<IEnumerable<string>, int>
{
    private static readonly List<string> _words =
        new() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    private static readonly int _minWordLength = _words.Min(w => w.Length);

    public static int SolvePart1(IEnumerable<string> input)
    {
        return input.Aggregate(
            0,
            (sum, line) =>
            {
                var digits = line.Select(c => (int)char.GetNumericValue(c))
                    .Where(n => n != -1)
                    .ToList();

                return sum + digits.First() * 10 + digits.Last();
            }
        );
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        int sum = 0;

        foreach (var line in input)
        {
            int firstDigit = 0;
            int lastDigit = 0;

            int number = 0;

            var lineCopy = new StringBuilder(line);

            while (lineCopy.Length > 0)
            {
                if (char.IsNumber(lineCopy[0]))
                {
                    number = (int)char.GetNumericValue(lineCopy[0]);
                }
                else if (lineCopy.Length >= _minWordLength)
                {
                    var index = _words.FindIndex(w => lineCopy.ToString().StartsWith(w));

                    if (index != -1)
                    {
                        number = index + 1;
                    }
                }

                if (number > 0)
                {
                    lastDigit = number;
                    if (firstDigit == 0)
                    {
                        firstDigit = number;
                    }
                }

                lineCopy = lineCopy.Remove(0, 1);
            }

            sum += firstDigit * 10 + lastDigit;
        }

        return sum;
    }
}
