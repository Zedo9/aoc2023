namespace AoC2023;

public static class Helpers
{
    public static long LCM(long[] numbers)
    {
        long result = numbers[0];

        for (int i = 1; i < numbers.Length; i++)
        {
            result = LCM(result, numbers[i]);
        }

        return result;
    }

    public static long LCM(long a, long b)
    {
        return (a * b) / GCD(a, b);
    }

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var tmp = b;
            b = a % b;
            a = tmp;
        }
        return a;
    }
}
