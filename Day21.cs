namespace AoC2023;

public class Day21 : IDay<IEnumerable<string>, long>
{
    private static readonly (int, int)[] PossibleMoves = [(1, 0), (-1, 0), (0, 1), (0, -1)];

    public static long SolvePart1(IEnumerable<string> input)
    {
        var grid = input.ToArray();

        var start = FindStartPosition(grid);

        const int maxSteps = 64;

        return FindCount(grid, start, maxSteps);
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var grid = input.ToArray();

        var start = FindStartPosition(grid);
        (long h, long w) = (grid.Length, grid[0].Length);
        const long maxSteps = 26501365;

        Console.WriteLine($"Height and Width : ({h}, {w})"); // (131, 131)
        Console.WriteLine($"Start {start}"); // Right in the middle of the grid (65, 65)
        // 26501365 % 131 = 65
        // => 26501365 = 131 * 202300 + 65
        //
        // 65  = 131 * 0 + 65
        // 196 = 131 * 1 + 65
        // 327 = 131 * 2 + 65
        // 458 = 131 * 3 + 65 ...

        long c1 = FindCount(grid, start, 0 * w + maxSteps % w, true);
        long c2 = FindCount(grid, start, 1 * w + maxSteps % w, true);
        long c3 = FindCount(grid, start, 2 * w + maxSteps % w, true);
        long c4 = FindCount(grid, start, 3 * w + maxSteps % w, true);
        long c5 = FindCount(grid, start, 4 * w + maxSteps % w, true);
        Console.WriteLine($"{c1} {c2} {c3} {c4} {c5}");

        long firstDiff1 = c2 - c1;
        long firstDiff2 = c3 - c2;
        long firstDiff3 = c4 - c3;
        long firstDiff4 = c5 - c4;
        Console.WriteLine($"First Diffs: {firstDiff1} {firstDiff2} {firstDiff3} {firstDiff4}");

        long secondDiff1 = firstDiff2 - firstDiff1;
        long secondDiff2 = firstDiff3 - firstDiff2;
        long secondDiff3 = firstDiff4 - firstDiff3;
        Console.WriteLine($"Second Diffs : {secondDiff1} == {secondDiff2} == {secondDiff3}");
        // => ^ Quadratic sequence

        // u(n) = an**2 + bn + c
        // - 2a = 2nd difference
        // - 3a + b = u(2) - u(1)
        // - a + b + c = u(1)
        long a = secondDiff1 / 2;
        long b = firstDiff1 - (3 * a);
        long c = c1 - a - b;
        Console.WriteLine($"a={a} b={b} c={c}");

        Func<long, long> f = (long n) => a * n * n + b * n + c;

        // One of these 2 should work, depending on the output of division
        // return f(maxSteps / w);
        return f((long)Math.Ceiling((double)maxSteps / w));
    }

    private static long FindCount(
        string[] grid,
        (int, int) start,
        long maxSteps,
        bool infinite = false
    )
    {
        var (startR, startC) = start;

        var rCount = grid.Length;
        var cCount = grid[0].Length;

        HashSet<(int, int)> visited = [(startR, startC)];
        for (long step = 0; step < maxSteps; step++)
        {
            HashSet<(int, int)> next = [];
            foreach (var pos in visited)
            {
                foreach (var m in PossibleMoves)
                {
                    var p = (pos.Item1 + m.Item1, pos.Item2 + m.Item2);
                    var lookupP = infinite ? (Modulo(p.Item1, rCount), Modulo(p.Item2, cCount)) : p;

                    if (
                        (!infinite && IsOutsideGrid(lookupP, grid))
                        || grid[lookupP.Item1][lookupP.Item2] is '#'
                    )
                    {
                        continue;
                    }

                    next.Add(p);
                }
            }

            visited = next;
        }

        return visited.Count;
    }

    private static int Modulo(int a, int b) => a % b < 0 ? a % b + b : a % b;

    private static (int, int) FindStartPosition(string[] grid)
    {
        (int startR, int startC) = (0, 0);

        for (int r = 0; r < grid.Length; r++)
        {
            var c = grid[r].IndexOf('S');
            if (c != -1)
            {
                (startR, startC) = (r, c);
                break;
            }
        }

        return (startR, startC);
    }

    private static bool IsOutsideGrid((int, int) pos, string[] grid)
    {
        var (r, c) = pos;
        return r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length;
    }
}
