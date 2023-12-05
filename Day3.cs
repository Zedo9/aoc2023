using System.Text;

namespace AoC2023;

public class Day3 : IDay<IEnumerable<string>, int>
{
    public static int SolvePart1(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();
        var sb = new StringBuilder();
        List<int> partNumbers = [];

        for (int row = 0; row < inputArr.Length; row++)
        {
            var line = inputArr[row];

            for (var col = 0; col < line.Length; col++)
            {
                var ch = line[col];

                if (char.IsDigit(ch))
                {
                    sb.Append(ch);
                }

                if (
                    (sb.Length > 0 && !char.IsDigit(ch))
                    || (col == line.Length - 1 && char.IsDigit(ch))
                )
                {
                    var isPartNumber = false;

                    var minRow = row - 1 > 0 ? row - 1 : row;
                    var maxRow = row + 1 < inputArr.Length ? row + 1 : row;
                    var minCol = col - sb.Length - 1 >= 0 ? col - sb.Length - 1 : col - sb.Length;
                    var maxCol = col;

                    for (var i = minRow; i <= maxRow; i++)
                    {
                        for (var j = minCol; j <= maxCol; j++)
                        {
                            if (IsSymbol(inputArr[i][j]))
                            {
                                isPartNumber = true;
                                break;
                            }
                        }

                        if (isPartNumber)
                        {
                            break;
                        }
                    }
                    if (isPartNumber)
                    {
                        partNumbers.Add(int.Parse(sb.ToString()));
                    }
                    sb.Clear();
                }
            }
        }

        return partNumbers.Sum();
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();
        var sb = new StringBuilder();
        Dictionary<(int, int), List<int>> starPositionToAdjacentPartNumbers = [];

        for (int row = 0; row < inputArr.Length; row++)
        {
            var line = inputArr[row];

            for (var col = 0; col < line.Length; col++)
            {
                var ch = line[col];

                if (char.IsDigit(ch))
                {
                    sb.Append(ch);
                }

                if (
                    (sb.Length > 0 && !char.IsDigit(ch))
                    || (col == line.Length - 1 && char.IsDigit(ch))
                )
                {
                    var minRow = row - 1 > 0 ? row - 1 : row;
                    var maxRow = row + 1 < inputArr.Length ? row + 1 : row;
                    var minCol = col - sb.Length - 1 >= 0 ? col - sb.Length - 1 : col - sb.Length;
                    var maxCol = col;

                    for (var i = minRow; i <= maxRow; i++)
                    {
                        for (var j = minCol; j <= maxCol; j++)
                        {
                            var el = inputArr[i][j];
                            if (el != '*')
                            {
                                continue;
                            }

                            (int, int) pos = (i, j);
                            List<int> existingPartNumbers;
                            var exists = starPositionToAdjacentPartNumbers.TryGetValue(
                                pos,
                                out existingPartNumbers!
                            );
                            if (!exists)
                            {
                                existingPartNumbers = [];
                            }
                            existingPartNumbers.Add(int.Parse(sb.ToString()));
                            starPositionToAdjacentPartNumbers[pos] = existingPartNumbers;
                        }
                    }
                    sb.Clear();
                }
            }
        }

        return starPositionToAdjacentPartNumbers
            .Values
            .Where(v => v.Count == 2)
            .Select(v => v.Aggregate(1, (acc, cur) => acc * cur))
            .Sum();
    }

    private static bool IsSymbol(char ch)
    {
        return !char.IsDigit(ch) && !(ch == '.');
    }
}
