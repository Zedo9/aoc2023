namespace AoC2023;

public interface IDay<TInput, TOutput>
{
    static abstract TOutput SolvePart1(TInput input);
    static abstract TOutput SolvePart2(TInput input);
}
