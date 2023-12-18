namespace AoC2023;

public class Day18 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var (currR, currC) = (0, 0);
        List<(long, long)> points = [(0, 0)];
        var dugPoints = 0;

        foreach (var line in input)
        {
            var lineArr = line.Split(' ');

            var dir = lineArr[0][0];
            var distance = int.Parse(lineArr[1]);
            dugPoints += distance;
            var (dR, dC) = GetDirectionP1(dir, distance);

            (currR, currC) = (currR + dR, currC + dC);
            points.Add((currR, currC));
        }

        var area = CalculateArea(points.ToArray());
        var pointsInside = CalculateInsidePoints(area, dugPoints);
        return dugPoints + pointsInside;
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var (currR, currC) = (0, 0);
        List<(long, long)> points = [(0, 0)];
        long dugPoints = 0;

        foreach (var line in input)
        {
            var lineArr = line.Split(' ');

            var code = lineArr[2][2..7];
            var distance = int.Parse(code, System.Globalization.NumberStyles.HexNumber);
            dugPoints += distance;
            var dir = lineArr[2][7];
            var (dR, dC) = GetDirectionP2(dir, distance);

            (currR, currC) = (currR + dR, currC + dC);
            points.Add((currR, currC));
        }

        var area = CalculateArea(points.ToArray());
        var pointsInside = CalculateInsidePoints(area, dugPoints);
        return dugPoints + pointsInside;
    }

    // Pick's Theorem
    public static long CalculateInsidePoints(long area, long boundary) => area - boundary / 2 + 1;

    // Shoelace Formula
    public static long CalculateArea((long, long)[] points)
    {
        long sum = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            var (r1, c1) = points[i];
            var (r2, c2) = points[i + 1];
            sum += ((c2 + c1) * (r1 - r2));
        }

        return Math.Abs(sum / 2);
    }

    public static (int, int) GetDirectionP1(char ch, int distance) =>
        ch switch
        {
            'R' => (0, distance),
            'L' => (0, -distance),
            'D' => (distance, 0),
            'U' => (-distance, 0),
            _ => throw new Exception($"OOPS: {ch} {distance}")
        };

    public static (int, int) GetDirectionP2(char ch, int distance) =>
        ch switch
        {
            '0' => (0, distance),
            '2' => (0, -distance),
            '1' => (distance, 0),
            '3' => (-distance, 0),
            _ => throw new Exception($"OOPS: {ch} {distance}")
        };

    // public static string[] BuildGridStr(Dictionary<int, Dictionary<int, char>> grid)
    // {
    //     List<string> gridStr = [];
    //     var maxR = grid.Keys.Max();
    //     var minR = grid.Keys.Min();
    //     var maxC = grid.Values.SelectMany(v => v.Keys).Max();
    //     var minC = grid.Values.SelectMany(v => v.Keys).Min();
    //
    //     var sum = 0;
    //     for (int r = minR; r < maxR + 1; r++)
    //     {
    //         var sb = new StringBuilder();
    //         var entered = 0;
    //         for (int c = minC; c < maxC + 1; c++)
    //         {
    //             if (!grid[r].ContainsKey(c))
    //             {
    //                 sb.Append('.');
    //                 if (entered % 2 != 0)
    //                 {
    //                     sum++;
    //                 }
    //             }
    //             else
    //             {
    //                 sb.Append('#');
    //                 if (c == 0 || c > 0 && !grid[r].ContainsKey(c - 1))
    //                 {
    //                     entered++;
    //                 }
    //                 sum++;
    //             }
    //         }
    //
    //         gridStr.Add(sb.ToString());
    //         sb.Clear();
    //     }
    //
    //     return gridStr.ToArray();
    // }
    //
    // public static Dictionary<int, Dictionary<int, char>> BuildGrid(IEnumerable<string> input)
    // {
    //     var (currR, currC) = (0, 0);
    //     Dictionary<int, Dictionary<int, char>> grid = new();
    //     grid[0] = new Dictionary<int, char>();
    //
    //     foreach (var line in input)
    //     {
    //         if (!grid.ContainsKey(currR))
    //         {
    //             grid[currR] = new();
    //         }
    //         grid[currR][currC] = '#';
    //
    //         var lineArr = line.Split(' ');
    //         var dir = lineArr[0][0];
    //         var distance = int.Parse(lineArr[1]);
    //
    //         var (dR, dC) = GetDirectionP1(dir, distance);
    //         if (dR == 0)
    //         {
    //             if (dC > 0)
    //             {
    //                 for (int i = 1; i < dC; i++)
    //                 {
    //                     grid[currR][currC + i] = '#';
    //                 }
    //             }
    //             else
    //             {
    //                 for (int i = -1; i > dC; i--)
    //                 {
    //                     grid[currR][currC + i] = '#';
    //                 }
    //             }
    //         }
    //
    //         if (dC == 0)
    //         {
    //             if (dR > 0)
    //             {
    //                 for (int i = 1; i < dR; i++)
    //                 {
    //                     if (!grid.ContainsKey(currR + i))
    //                     {
    //                         grid[currR + i] = new();
    //                     }
    //                     grid[currR + i][currC] = '#';
    //                 }
    //             }
    //             else
    //             {
    //                 for (int i = -1; i > dR; i--)
    //                 {
    //                     if (!grid.ContainsKey(currR + i))
    //                     {
    //                         grid[currR + i] = new();
    //                     }
    //                     grid[currR + i][currC] = '#';
    //                 }
    //             }
    //         }
    //
    //         currR += dR;
    //         currC += dC;
    //     }
    //
    //     return grid;
    // }
}
