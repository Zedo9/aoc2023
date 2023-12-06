namespace AoC2023;

public class Day6 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var inputArr = input.ToArray();
        var distances = ParseLineP1(inputArr[1]).ToArray();
        return ParseLineP1(inputArr[0])
            .Select((time, idx) => FindNumberOfWays(time, distances[idx]))
            .Aggregate((long)1, (acc, el) => el * acc);
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        var inputArr = input.Select(ParseLineP2).ToArray();
        return FindNumberOfWays(inputArr[0], inputArr[1]);
    }

    private static long FindNumberOfWays(long recordTime, long distance)
    {
        var ways = 0;
        var speed = 1;

        while (speed < recordTime)
        {
            var traveledDistance = (recordTime - speed) * speed;
            if (traveledDistance > distance)
            {
                ways++;
            }
            speed++;
        }

        return ways;
    }

    private static IEnumerable<long> ParseLineP1(string str) =>
        str.Split(':')[1].Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(long.Parse);

    private static long ParseLineP2(string str) =>
        long.Parse(string.Join("", str.Split(':')[1].Where(s => s != ' ')));
}
