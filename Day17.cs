namespace AoC2023;

public class Day17 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        string[] grid = input.ToArray();

        var startingPoint = new Position(0, 0);
        var startingDir = new Direction(0, 0);
        var destination = new Position(grid.Length - 1, grid[0].Length - 1);
        const int maxConsecutiveStepsSameDirection = 3;

        return MinEnergyDijkstra(
            grid,
            startingPoint,
            destination,
            maxConsecutiveStepsSameDirection
        );
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        string[] grid = input.ToArray();

        var startingPoint = new Position(0, 0);
        var startingDir = new Direction(0, 0);
        var destination = new Position(grid.Length - 1, grid[0].Length - 1);
        const int maxConsecutiveStepsSameDirection = 10;
        const int minConsecutiveStepsSameDirection = 4;

        return MinEnergyDijkstra(
            grid,
            startingPoint,
            destination,
            maxConsecutiveStepsSameDirection,
            minConsecutiveStepsSameDirection
        );
    }

    public static int MinEnergyDijkstra(
        string[] grid,
        Position start,
        Position dest,
        int maxConsecutiveStepsSameDirection,
        int minConsecutiveStepsSameDirection = -1
    )
    {
        Direction[] directions =
        [
            new Direction(1, 0),
            new Direction(-1, 0),
            new Direction(0, 1),
            new Direction(0, -1)
        ];

        // (Position, Direction, DirectionCount, Heat) -> Heat
        var pq = new PriorityQueue<State, int>();
        var startDir = new Direction(0, 0);
        var visitedStates = new HashSet<(Position, Direction, int)>();
        pq.Enqueue(new State(start, startDir, 0, 0), 0);

        while (pq.Count > 0)
        {
            var state = pq.Dequeue();

            var (pos, dir, dirCount, heat) = state;
            // Console.WriteLine(state);

            if (pos == dest && dirCount >= minConsecutiveStepsSameDirection)
            {
                return heat;
            }

            if (visitedStates.Contains((pos, dir, dirCount)))
            {
                continue;
            }

            visitedStates.Add((pos, dir, dirCount));

            if (dirCount < maxConsecutiveStepsSameDirection && dir != startDir)
            {
                var newPos = pos.Move(dir);
                if (!IsOutsideGrid(newPos, grid))
                {
                    var newHeat = heat + (int.Parse(grid[newPos.R][newPos.C].ToString()));
                    pq.Enqueue(new State(newPos, dir, dirCount + 1, newHeat), newHeat);
                }
            }

            if (dirCount < minConsecutiveStepsSameDirection && dir != startDir)
            {
                continue;
            }

            foreach (
                var d in directions.Where(
                    d => d != dir && (d.DirR, d.DirC) != (-1 * dir.DirR, -1 * dir.DirC)
                )
            )
            {
                var newPos = pos.Move(d);
                if (!IsOutsideGrid(newPos, grid))
                {
                    var newHeat = heat + (int.Parse(grid[newPos.R][newPos.C].ToString()));
                    pq.Enqueue(new State(newPos, d, 1, newHeat), newHeat);
                }
            }
        }

        return 0;
    }

    public static bool IsOutsideGrid(Position pos, string[] grid)
    {
        return pos.R < 0 || pos.R >= grid.Length || pos.C < 0 || pos.C >= grid[0].Length;
    }

    public readonly record struct State(
        Position Position,
        Direction Direction,
        int DirectionCount,
        int Heat
    );

    public readonly record struct Direction(int DirR, int DirC);

    public record struct Position(int R, int C)
    {
        public Position Move(Direction dir)
        {
            return new Position(R + dir.DirR, C + dir.DirC);
        }
    };
}
