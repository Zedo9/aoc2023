namespace AoC2023;

public class Day15 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input) => Parse(input).Sum(Hash);

    private static int Hash(string str)
    {
        var curr = 0;

        for (int i = 0; i < str.Length; i++)
        {
            curr += Convert.ToInt32(str[i]);
            curr *= 17;
            curr %= 256;
        }

        return curr;
    }

    private static IEnumerable<string> Parse(IEnumerable<string> input) =>
        string.Join("", input).Split(',');

    public static long SolvePart2(IEnumerable<string> input)
    {
        var boxes = new List<Dictionary<string, int>>(256);

        for (int i = 0; i < 256; i++)
        {
            boxes.Add(new Dictionary<string, int>());
        }

        foreach (var str in Parse(input))
        {
            var indexOfEq = str.IndexOf('=');
            var indexOfDash = str.IndexOf('-');
            string label;
            if (indexOfEq != -1)
            {
                label = str[..indexOfEq];
                var countIndex = indexOfEq + 1;
                var count = int.Parse(str[countIndex..]);
                var boxIndex = Hash(label);
                if (boxes[boxIndex].ContainsKey(label))
                {
                    boxes[boxIndex][label] = count;
                }
                else
                {
                    var kvp = KeyValuePair.Create(label, count);
                    boxes[boxIndex] = boxes[boxIndex].Append(kvp).ToDictionary();
                }
            }
            else
            {
                label = str[..indexOfDash];
                var boxIndex = Hash(label);
                boxes[boxIndex].Remove(label);
            }
        }

        var sum = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            var keys = boxes[i].Keys.ToArray();
            if (keys.Length == 0)
            {
                continue;
            }

            for (int j = 0; j < keys.Length; j++)
            {
                var res = (j + 1) * (i + 1) * boxes[i][keys[j]];
                // Console.WriteLine($"{keys[j]} {i + 1} {j + 1} {boxes[i][keys[j]]} -> {res}");
                sum += res;
            }
        }

        return sum;
    }
}
