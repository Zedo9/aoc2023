namespace AoC2023;

public class Day4 : IDay<IEnumerable<string>, int>
{
    public static int SolvePart1(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var line in input)
        {
            var cards = line.Split(": ")[1].Split(" | ");
            var counter = 0;

            var winningCards = ParseCards(cards[0]);
            var myCards = ParseCards(cards[1]);

            foreach (var card in myCards)
            {
                var exists = winningCards.Any((c) => c == card);
                if (exists)
                {
                    counter++;
                }
            }
            if (counter != 0)
            {
                sum += (int)Math.Pow(2, counter - 1);
            }
        }

        return sum;
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        Dictionary<int, int> cardIdToCount = [];
        var lines = input.ToArray();

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var cards = line.Split(": ")[1].Split(" | ");
            var counter = 0;

            var winningCards = ParseCards(cards[0]);
            var myCards = ParseCards(cards[1]);

            if (!cardIdToCount.TryGetValue(i + 1, out _))
            {
                cardIdToCount[i + 1] = 1;
            }

            foreach (var card in myCards)
            {
                var exists = winningCards.Any((c) => c == card);
                if (exists)
                {
                    counter++;
                }
            }

            for (int j = i + 2; j < i + 2 + counter; j++)
            {
                if (cardIdToCount.TryGetValue(j, out _))
                {
                    cardIdToCount[j] = cardIdToCount[j] + cardIdToCount[i + 1];
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
        str.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s));
}
