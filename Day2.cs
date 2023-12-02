using AoC2023;

public class Day2 : IDay<IEnumerable<string>, int>
{
    public static int SolvePart1(IEnumerable<string> input)
    {
        Dictionary<string, int> cubeCounts =
            new()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

        var sum = 0;

        foreach (var line in input)
        {
            var gameEliminated = false;
            var game = ParseGame(line);

            foreach (var cubeResult in game.GetAllCubeResults())
            {
                var maxCubeCount = cubeCounts[cubeResult.Type];

                if (cubeResult.Count > maxCubeCount)
                {
                    gameEliminated = true;
                    break;
                }
            }

            if (!gameEliminated)
            {
                sum += game.Id;
            }
        }

        return sum;
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        List<int> gamePowers = new();

        foreach (var line in input)
        {
            var game = ParseGame(line);

            var minCubesRequired = new Dictionary<string, int>();
            minCubesRequired.Add("red", 0);
            minCubesRequired.Add("blue", 0);
            minCubesRequired.Add("green", 0);

            foreach (var cubeResult in game.GetAllCubeResults())
            {
                var currentMinCount = minCubesRequired[cubeResult.Type];
                if (currentMinCount < cubeResult.Count)
                {
                    minCubesRequired[cubeResult.Type] = cubeResult.Count;
                }
            }

            gamePowers.Add(minCubesRequired.Values.Aggregate(1, (res, el) => res * el));
        }

        return gamePowers.Sum();
    }

    private static CubeResult ParseCubeResult(string str)
    {
        var split = str.Trim().Split(' ');
        var count = Int32.Parse(split[0].Trim());
        var type = split[1];

        return new CubeResult(count, type);
    }

    private static Game ParseGame(string str)
    {
        var lineSplit = str.Split(':');
        var gameId = Int32.Parse(lineSplit[0].Split(' ')[1]);
        var sets = lineSplit[1].Split(';');

        return new Game(gameId, GetSets(sets));
    }

    private static Set ParseSet(string str)
    {
        var cubeResultsStr = str.Split(',');

        return new Set(GetCubeResults(cubeResultsStr));
    }

    private static IEnumerable<CubeResult> GetCubeResults(string[] resultStrs)
    {
        foreach (var resultStr in resultStrs)
        {
            yield return ParseCubeResult(resultStr);
        }
    }

    private static IEnumerable<Set> GetSets(string[] setStrs)
    {
        foreach (var setStr in setStrs)
        {
            yield return ParseSet(setStr);
        }
    }

    public record CubeResult(int Count, string Type);

    public record Set(IEnumerable<CubeResult> Results);

    public record Game(int Id, IEnumerable<Set> Sets)
    {
        public IEnumerable<CubeResult> GetAllCubeResults() => Sets.SelectMany(S => S.Results);
    }
}
