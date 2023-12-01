namespace AoC2023;

public interface IDay<TInput, TOutput>
{
    abstract static TOutput SolvePart1(TInput input);
    abstract static TOutput SolvePart2(TInput input);
}
