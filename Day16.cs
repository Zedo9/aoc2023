namespace AoC2023;

public class Day16 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input) =>
        GetEnergizedCount((0, 0), input.ToArray(), Direction.Right);

    public static int GetEnergizedCount((int, int) pos, string[] grid, Direction direction)
    {
        Queue<((int, int), Direction)> queue = [];
        HashSet<((int, int), Direction)> visited = [];

        queue.Enqueue((pos, direction));

        while (queue.Any())
        {
            var ((currR, currC), dir) = queue.Dequeue();

            if (IsOutsideGrid((currR, currC), grid))
            {
                continue;
            }

            visited.Add(((currR, currC), dir));

            var currTile = grid[currR][currC];
            foreach (var item in FindNextTilesToVisit(currTile, currR, currC, dir))
            {
                if (visited.Contains(item))
                {
                    continue;
                }
                queue.Enqueue(item);
            }
        }

        return visited.Select((item) => item.Item1).ToHashSet().Count();
    }

    public enum Direction
    {
        Right,
        Left,
        Top,
        Bottom
    }

    public static IEnumerable<((int, int), Direction)> FindNextTilesToVisit(
        char currTile,
        int currR,
        int currC,
        Direction dir
    ) =>
        (dir, currTile) switch
        {
            (Direction.Right or Direction.Left, '|')
                => [((currR - 1, currC), Direction.Top), ((currR + 1, currC), Direction.Bottom)],
            (Direction.Top or Direction.Bottom, '-')
                => [((currR, currC + 1), Direction.Right), ((currR, currC - 1), Direction.Left)],
            (Direction.Left, '-') => [((currR, currC - 1), Direction.Left)],
            (Direction.Right, '-') => [((currR, currC + 1), Direction.Right)],
            (Direction.Top, '|') => [((currR - 1, currC), Direction.Top)],
            (Direction.Bottom, '|') => [((currR + 1, currC), Direction.Bottom)],
            (Direction.Top, '/') => [((currR, currC + 1), Direction.Right)],
            (Direction.Top, '\\') => [((currR, currC - 1), Direction.Left)],
            (Direction.Bottom, '/') => [((currR, currC - 1), Direction.Left)],
            (Direction.Bottom, '\\') => [((currR, currC + 1), Direction.Right)],
            (Direction.Right, '/') => [((currR - 1, currC), Direction.Top)],
            (Direction.Right, '\\') => [((currR + 1, currC), Direction.Bottom)],
            (Direction.Left, '/') => [((currR + 1, currC), Direction.Bottom)],
            (Direction.Left, '\\') => [((currR - 1, currC), Direction.Top)],
            (Direction.Left, '.') => [((currR, currC - 1), Direction.Left)],
            (Direction.Right, '.') => [((currR, currC + 1), Direction.Right)],
            (Direction.Top, '.') => [((currR - 1, currC), Direction.Top)],
            (Direction.Bottom, '.') => [((currR + 1, currC), Direction.Bottom)],
            (_, _) => throw new Exception($"OOPS: ({currR}, {currC}) : [{currTile}] : {dir}")
        };

    public static bool IsOutsideGrid((int, int) pos, string[] grid)
    {
        var (r, c) = pos;
        var maxR = grid.Length - 1;
        var maxC = grid[0].Length - 1;
        return r < 0 || r > maxR || c < 0 || c > maxC;
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var grid = input.ToArray();

        var rowsToTry = Enumerable.Range(0, grid.Length);
        var colsToTry = Enumerable.Range(0, grid[0].Length);
        var maxC = grid[0].Length - 1;
        var maxR = grid.Length - 1;
        IEnumerable<int> PossibleEnergizedCounts =
        [
            ..rowsToTry.Select((r) => GetEnergizedCount((r, 0), grid, Direction.Right)),
            ..rowsToTry.Select((r) => GetEnergizedCount((r, maxC), grid, Direction.Left)),
            ..colsToTry.Select((c) => GetEnergizedCount((0, c), grid, Direction.Bottom)),
            ..colsToTry.Select((c) => GetEnergizedCount((maxR, c), grid, Direction.Top))
        ];

        return PossibleEnergizedCounts.Max();
    }
}
