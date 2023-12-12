using System.Text;

namespace AoC2023;

public class Day11 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var universe = input.ToList();

        var emptyRows = FindEmptyRows(universe);
        var emptyCols = FindEmptyCols(universe);

        universe = ExpandUniverse(universe, emptyRows, emptyCols);

        var galaxies = FindGalaxies(universe);

        var total = 0;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var steps = FindShortestPathManhattan(galaxies[i], galaxies[j]).Count;
                total += steps;
            }
        }

        return total;
    }

    private static List<(int, int)> FindGalaxies(List<string> universe)
    {
        List<(int, int)> galaxies = [];

        for (int row = 0; row < universe.Count; row++)
        {
            for (int col = 0; col < universe[row].Length; col++)
            {
                if (universe[row][col] == '#')
                {
                    galaxies.Add((row, col));
                }
            }
        }

        return galaxies;
    }

    private static List<string> ExpandUniverse(
        List<string> universe,
        List<int> emptyRows,
        List<int> emptyCols
    )
    {
        for (int i = 0; i < emptyCols.Count; i++)
        {
            for (int row = 0; row < universe.Count; row++)
            {
                universe[row] = universe[row].Insert(emptyCols[i] + i, ".");
            }
        }

        var sb = new StringBuilder();
        for (int i = 0; i < universe[0].Length; i++)
        {
            sb.Append('.');
        }
        var str = sb.ToString();
        for (int i = 0; i < emptyRows.Count; i++)
        {
            universe.Insert(emptyRows[i] + i, str);
        }

        return universe;
    }

    private static List<int> FindEmptyRows(List<string> universe)
    {
        List<int> emptyRows = [];

        for (int i = 0; i < universe.Count; i++)
        {
            if (!universe[i].Contains('#'))
            {
                emptyRows.Add(i);
            }
        }

        return emptyRows;
    }

    private static List<int> FindEmptyCols(List<string> universe)
    {
        List<int> emptyCols = [];

        for (int col = 0; col < universe[0].Length; col++)
        {
            var foundGalaxy = false;
            for (int row = 0; row < universe.Count; row++)
            {
                if (universe[row][col] == '#')
                {
                    foundGalaxy = true;
                    break;
                }
            }
            if (!foundGalaxy)
            {
                emptyCols.Add(col);
            }
        }

        return emptyCols;
    }

    private static List<(int, int)> FindShortestPathManhattan((int, int) start, (int, int) end)
    {
        var current = start;
        List<(int, int)> path = [];

        while (current != end)
        {
            path.Add(current);
            var nextMove = GetNextMove(current, end);
            current = nextMove;
        }

        return path;
    }

    private static (int, int) GetNextMove((int, int) current, (int, int) destination)
    {
        var (r1, c1) = current;
        var (r2, c2) = destination;

        if (r1 < r2)
        {
            return (r1 + 1, c1);
        }
        else if (r1 > r2)
        {
            return (r1 - 1, c1);
        }
        else if (c1 < c2)
        {
            return (r1, c1 + 1);
        }
        else if (c1 > c2)
        {
            return (r1, c1 - 1);
        }
        // Shouldn't happen;
        return (0, 0);
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var universe = input.ToList();

        var emptyRows = FindEmptyRows(universe);
        var emptyCols = FindEmptyCols(universe);

        var galaxies = FindGalaxies(universe);

        long total = 0;
        var scale = 1_000_000;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            var (r1, c1) = galaxies[i];

            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var (r2, c2) = galaxies[j];

                for (int r = Math.Min(r1, r2); r < Math.Max(r1, r2); r++)
                {
                    total += emptyRows.Contains(r) ? scale : 1;
                }

                for (int c = Math.Min(c1, c2); c < Math.Max(c1, c2); c++)
                {
                    total += emptyCols.Contains(c) ? scale : 1;
                }
            }
        }

        return total;
    }
}
