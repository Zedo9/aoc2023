namespace AoC2023;

public class Day4 : IDay<IEnumerable<string>, int>
{
    public static int SolvePart1(IEnumerable<string> input) =>
        input
            .Select(GetPoints)
            .Where(points => points != 0)
            .Select(points => (int)Math.Pow(2, points - 1))
            .Sum();

    public static int SolvePart2(IEnumerable<string> input)
    {
        Dictionary<int, int> cardIdToCount = [];
        var lines = input.ToArray();

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var counter = GetPoints(line);

            if (!cardIdToCount.ContainsKey(i + 1))
            {
                cardIdToCount[i + 1] = 1;
            }

            for (int j = i + 2; j < i + 2 + counter; j++)
            {
                if (cardIdToCount.TryGetValue(j, out int value))
                {
                    cardIdToCount[j] = value + cardIdToCount[i + 1];
                }
                else
                {
                    cardIdToCount[j] = 1 + cardIdToCount[i + 1];
                }
            }
        }

        return cardIdToCount.Values.Sum();
    }

    private static IEnumerable<int> ParseCards(string str) =>
        str.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse);

    private static int GetPoints(string line)
    {
        var cards = line.Split(": ")[1].Split(" | ");
        var winningCards = ParseCards(cards[0]).ToHashSet();
        var myCards = ParseCards(cards[1]).ToHashSet();

        return myCards.Intersect(winningCards).Count();
    }
}
