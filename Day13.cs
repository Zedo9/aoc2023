using System.Text;

namespace AoC2023;

public class Day13 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var patterns = string.Join('\n', input).Split("\n\n");

        var colsScore = 0;
        var rowsScore = 0;

        foreach (var pattern in patterns)
        {
            var lines = pattern.Split('\n');
            var cols = BuildCols(lines);

            colsScore += GetReflectionScore(cols, 0);
            rowsScore += GetReflectionScore(lines, 0);
        }

        return rowsScore * 100 + colsScore;
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var patterns = string.Join('\n', input).Split("\n\n");

        var colsScore = 0;
        var rowsScore = 0;

        foreach (var pattern in patterns)
        {
            var lines = pattern.Split('\n');
            var cols = BuildCols(lines);

            colsScore += GetReflectionScore(cols, 1);
            rowsScore += GetReflectionScore(lines, 1);
        }

        return rowsScore * 100 + colsScore;
    }

    private static string[] BuildCols(string[] lines)
    {
        var arr = new string[lines[0].Length];
        var sb = new StringBuilder();

        for (int c = 0; c < lines[0].Length; c++)
        {
            for (int r = 0; r < lines.Length; r++)
            {
                sb.Append(lines[r][c]);
            }
            var col = sb.ToString();
            arr[c] = col;
            sb.Clear();
        }

        return arr;
    }

    private static int GetSmudgesCount(string str1, string str2)
    {
        var smudges = 0;

        for (int i = 0; i < str1.Length; i++)
        {
            if (str1[i] != str2[i])
            {
                smudges++;
            }
        }

        return smudges;
    }

    private static int GetReflectionScore(string[] elements, int requiredSmudges)
    {
        for (int i = 0; i < elements.Length - 1; i++)
        {
            var usedSmudges = GetSmudgesCount(elements[i], elements[i + 1]);
            if (usedSmudges > requiredSmudges)
            {
                continue;
            }

            var left = i - 1;
            var right = i + 2;

            while (true)
            {
                if ((left < 0 || right > elements.Length - 1))
                {
                    if (usedSmudges == requiredSmudges)
                    {
                        return i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if ((left == 0 || right == elements.Length - 1))
                {
                    if (
                        GetSmudgesCount(elements[left], elements[right]) + usedSmudges
                        == requiredSmudges
                    )
                    {
                        return i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                usedSmudges += GetSmudgesCount(elements[left], elements[right]);

                if (usedSmudges > requiredSmudges)
                {
                    break;
                }

                left--;
                right++;
            }
        }

        return 0;
    }
}
