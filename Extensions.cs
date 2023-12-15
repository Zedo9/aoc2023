using System.Text;

namespace AoC2023;

public static class Extensions
{
    public static void Print(this string[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Console.WriteLine(elements[i]);
        }
        Console.WriteLine();
    }

    public static string[] Rotate(this string[] rows)
    {
        var cols = new string[rows[0].Length];

        var sb = new StringBuilder();
        for (int c = 0; c < rows[0].Length; c++)
        {
            for (int r = 0; r < rows.Length; r++)
            {
                sb.Append(rows[r][c]);
            }
            cols[c] = sb.ToString();
            sb.Clear();
        }
        return cols;
    }
}
