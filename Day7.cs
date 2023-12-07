namespace AoC2023;

public class Day7 : IDay<IEnumerable<string>, int>
{
    private static readonly List<char> CardStrengthP1 =
    [
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'T',
        'J',
        'Q',
        'K',
        'A'
    ];

    private static readonly List<char> CardStrengthP2 = CardStrengthP1
        .Where(c => c != 'J')
        .Prepend('J')
        .ToList();

    public static int SolvePart1(IEnumerable<string> input)
    {
        var hands = input
            .Select(s => s.Split(' '))
            .Select(e => new Entry(e[0], int.Parse(e[1])))
            .ToArray();

        Dictionary<int, HandType> handIndexToType = [];
        List<int> sortedHandIndices = [];

        for (int i = 0; i < hands.Length; i++)
        {
            var entry = hands[i];
            Dictionary<char, int> cardLabelsToCount = entry
                .Hand
                .GroupBy(card => card)
                .ToDictionary(group => group.Key, group => group.Count());

            var handType = GetHandType(cardLabelsToCount);

            handIndexToType[i] = handType;

            if (sortedHandIndices.Count == 0)
            {
                sortedHandIndices.Add(i);
            }
            else
            {
                for (int j = 0; j < sortedHandIndices.Count; j++)
                {
                    if (handIndexToType[sortedHandIndices[j]] < handType)
                    {
                        sortedHandIndices.Insert(j, i);
                        break;
                    }
                    else if (handIndexToType[sortedHandIndices[j]] == handType)
                    {
                        for (int k = j; k < sortedHandIndices.Count; k++)
                        {
                            if (
                                handIndexToType[sortedHandIndices[k]] < handType
                                || handIndexToType[sortedHandIndices[k]] == handType
                                    && IsBetterHandOrder(
                                        hands[i].Hand.ToArray(),
                                        hands[sortedHandIndices[k]].Hand.ToArray(),
                                        CardStrengthP1
                                    )
                            )
                            {
                                sortedHandIndices.Insert(k, i);
                                break;
                            }
                            else if (k == sortedHandIndices.Count - 1)
                            {
                                sortedHandIndices.Add(i);
                                break;
                            }
                        }
                        break;
                    }
                    else if (sortedHandIndices.Count - 1 == j)
                    {
                        sortedHandIndices.Add(i);
                        break;
                    }
                }
            }
        }

        return sortedHandIndices
            .Select(
                (sortedHandIndex, index) =>
                    hands[sortedHandIndex].Bid * (sortedHandIndices.Count - index)
            )
            .Sum();
    }

    private static bool IsBetterHandOrder(char[] hand1, char[] hand2, List<char> cardStrengths)
    {
        for (int i = 0; i < hand1.Length; i++)
        {
            var strength1 = cardStrengths.FindIndex(c => c == hand1[i]);
            var strength2 = cardStrengths.FindIndex(c => c == hand2[i]);

            if (strength1 == strength2)
            {
                continue;
            }
            if (strength1 > strength2)
            {
                return true;
            }
            return false;
        }

        return true;
    }

    private static HandType GetHandType(Dictionary<char, int> labelsToCount) =>
        (
            labelsToCount.Keys.Count(),
            labelsToCount.Values.All(v => v == 1 || v == 4),
            labelsToCount.Values.All(v => v == 2 || v == 3),
            labelsToCount.Values.All(v => v == 3 || v == 1),
            labelsToCount.Values.All(v => v == 2 || v == 1)
        ) switch
        {
            (1, _, _, _, _) => HandType.FiveOfAKind,
            (5, _, _, _, _) => HandType.HighCard,
            (2, true, _, _, _) => HandType.FourOfAKind,
            (2, _, true, _, _) => HandType.FullHouse,
            (3, _, _, true, _) => HandType.ThreeOfAKind,
            (4, _, _, _, true) => HandType.OnePair,
            _ => HandType.TwoPair
        };

    private static HandType GetBestHandType(Dictionary<char, int> cardLabelsToCount, int jokerCount)
    {
        var regularHandType = GetHandType(cardLabelsToCount);

        if (jokerCount == 0)
        {
            return regularHandType;
        }

        if (
            regularHandType == HandType.FiveOfAKind
            || regularHandType == HandType.FourOfAKind && jokerCount == 1
        )
        {
            return HandType.FiveOfAKind;
        }

        var bestHandType = regularHandType;
        cardLabelsToCount.Remove('J');

        for (int k = 1; k < CardStrengthP2.Count; k++)
        {
            var exists = cardLabelsToCount.TryGetValue(CardStrengthP2[k], out var oldVal);
            if (exists)
            {
                cardLabelsToCount[CardStrengthP2[k]] += jokerCount;
            }

            var potentialHandType = GetHandType(cardLabelsToCount);
            bestHandType = (HandType)Math.Max((int)potentialHandType, (int)bestHandType);

            if (exists)
            {
                cardLabelsToCount[CardStrengthP2[k]] = oldVal;
            }
        }

        return bestHandType;
    }

    public static int SolvePart2(IEnumerable<string> input)
    {
        var hands = input
            .Select(s => s.Split(' '))
            .Select(e => new Entry(e[0], int.Parse(e[1])))
            .ToArray();

        Dictionary<int, HandType> handIndexToType = [];
        List<int> sortedHandIndices = [];

        for (int i = 0; i < hands.Length; i++)
        {
            var entry = hands[i];

            Dictionary<char, int> cardLabelsToCount = entry
                .Hand
                .GroupBy(card => card)
                .ToDictionary(group => group.Key, group => group.Count());

            var jokerCount = entry.Hand.Count(a => a == 'J');
            HandType handType = GetBestHandType(cardLabelsToCount, jokerCount);

            handIndexToType[i] = handType;

            if (sortedHandIndices.Count == 0)
            {
                sortedHandIndices.Add(i);
            }
            else
            {
                for (int j = 0; j < sortedHandIndices.Count; j++)
                {
                    if (handIndexToType[sortedHandIndices[j]] < handType)
                    {
                        sortedHandIndices.Insert(j, i);
                        break;
                    }
                    else if (handIndexToType[sortedHandIndices[j]] == handType)
                    {
                        for (int k = j; k < sortedHandIndices.Count; k++)
                        {
                            if (
                                handIndexToType[sortedHandIndices[k]] < handType
                                || handIndexToType[sortedHandIndices[k]] == handType
                                    && IsBetterHandOrder(
                                        hands[i].Hand.ToArray(),
                                        hands[sortedHandIndices[k]].Hand.ToArray(),
                                        CardStrengthP2
                                    )
                            )
                            {
                                sortedHandIndices.Insert(k, i);
                                break;
                            }
                            else if (k == sortedHandIndices.Count - 1)
                            {
                                sortedHandIndices.Add(i);
                                break;
                            }
                        }
                        break;
                    }
                    else if (sortedHandIndices.Count - 1 == j)
                    {
                        sortedHandIndices.Add(i);
                        break;
                    }
                }
            }
        }

        return sortedHandIndices
            .Select(
                (sortedHandIndex, index) =>
                    hands[sortedHandIndex].Bid * (sortedHandIndices.Count - index)
            )
            .Sum();
    }

    public readonly record struct Entry(IEnumerable<char> Hand, int Bid);

    public enum HandType
    {
        HighCard = 0,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}
