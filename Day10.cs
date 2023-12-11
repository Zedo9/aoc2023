namespace AoC2023;

public class Day10 : IDay<IEnumerable<string>, int>
{
    public static int SolvePart1(IEnumerable<string> input)
    {
        string[] map = input.ToArray();

        var startingPoint = FindStartingPoint(map);
        var traversedTiles = BuildLoop(map, startingPoint);

        return traversedTiles.Count / 2;
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        string[] map = input.ToArray();

        var startingPoint = FindStartingPoint(map);
        var traversedTiles = BuildLoop(map, startingPoint);

        List<(int, int)> enclosedTiles = [];
        for (int i = 0; i < map.Length; i++)
        {
            var enclosed = false;
            for (int j = 0; j < map[i].Length; j++)
            {
                if (traversedTiles.Contains((i, j)))
                {
                    // -> |J.F---.7
                    // -> |.7|F---J
                    // This doesn't work in all cases 🤔
                    if (map[i][j] is '|' or '7' or 'F')
                    {
                        enclosed = !enclosed;
                    }
                }
                else if (enclosed)
                {
                    enclosedTiles.Add((i, j));
                }
            }
        }

        DrawMap(map, enclosedTiles, traversedTiles);

        return enclosedTiles.Count;
    }

    private static (int, int) FindStartingPoint(string[] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            var idx = map[i].ToList().FindIndex(s => s == 'S');
            if (idx != -1)
            {
                return (i, idx);
            }
        }

        // Shouldn't happen
        return (0, 0);
    }

    private static void DrawMap(string[] map, List<(int, int)> enclosed, List<(int, int)> traversed)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (enclosed.Contains((i, j)))
                {
                    Console.Write('I');
                }
                else if (traversed.Contains((i, j)))
                {
                    // Console.Write(map[i][j]);
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
    }

    private static List<(int, int)> BuildLoop(string[] map, (int, int) startingPoint)
    {
        Queue<(int, int)> queue = [];
        queue.Enqueue(startingPoint);
        List<(int, int)> traversed = [startingPoint];

        while (queue.Any())
        {
            var (row, col) = queue.Dequeue();
            var ch = map[row][col];

            if (
                row >= 1
                && "SJ|L".Contains(ch)
                && "|F7".Contains(map[row - 1][col])
                && !traversed.Any(c => (c.Item1, c.Item2) == (row - 1, col))
            )
            {
                queue.Enqueue((row - 1, col));
                traversed.Add((row - 1, col));
            }
            if (
                row <= map.Length - 2
                && "S7F|".Contains(ch)
                && "|JL".Contains(map[row + 1][col])
                && !traversed.Any(c => c == (row + 1, col))
            )
            {
                queue.Enqueue((row + 1, col));
                traversed.Add((row + 1, col));
            }
            if (
                col >= 1
                && "S-7J".Contains(ch)
                && "LF-".Contains(map[row][col - 1])
                && !traversed.Any(c => c == (row, col - 1))
            )
            {
                queue.Enqueue((row, col - 1));
                traversed.Add((row, col - 1));
            }
            if (
                col <= map[0].Length - 2
                && "LF-S".Contains(ch)
                && "7J-".Contains(map[row][col + 1])
                && !traversed.Any(c => c == (row, col + 1))
            )
            {
                queue.Enqueue((row, col + 1));
                traversed.Add((row, col + 1));
            }
        }

        return traversed;
    }
}
