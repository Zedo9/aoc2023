namespace AoC2023;

public class Day19 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        var startParsingRatings = false;
        List<Part> parts = [];
        Dictionary<string, IEnumerable<Func<Part, string>>> workflows = [];

        foreach (var line in input)
        {
            if (startParsingRatings)
            {
                parts.Add(ParseRatings(line));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    startParsingRatings = true;
                    continue;
                }

                var (name, funcs) = ParseWorkflow(line);
                workflows[name] = funcs;
            }
        }

        var sum = 0;
        foreach (var part in parts)
        {
            var output = SimulateWorflows(part, workflows);
            if (output is 'A')
            {
                sum += part.X + part.M + part.S + part.A;
            }
        }

        return sum;
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        Dictionary<string, IEnumerable<string>> workflows = [];

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var (name, rules) = ParseRules(line);
            workflows[name] = rules;
        }

        long sum = 0;

        var q = new Queue<State>();

        q.Enqueue(new State("in", (1, 4000), (1, 4000), (1, 4000), (1, 4000)));
        List<State> items = [];

        while (q.Any())
        {
            var s = q.Dequeue();
            var (curr, xRange, mRange, aRange, sRange) = s;
            if (curr == "R")
            {
                continue;
            }

            var (xLow, xHigh) = xRange;
            var (mLow, mHigh) = mRange;
            var (aLow, aHigh) = aRange;
            var (sLow, sHigh) = sRange;

            if (curr == "A")
            {
                sum +=
                    (long)(xHigh - xLow + 1)
                    * (mHigh - mLow + 1)
                    * (aHigh - aLow + 1)
                    * (sHigh - sLow + 1);
                continue;
            }

            if (xLow > xHigh || mLow > mHigh || aLow > aHigh || sLow > sHigh)
            {
                continue;
            }

            var workflow = workflows[curr];
            // Console.WriteLine(string.Join(',', workflow));
            foreach (var rule in workflow)
            {
                (xLow, xHigh) = xRange;
                (mLow, mHigh) = mRange;
                (aLow, aHigh) = aRange;
                (sLow, sHigh) = sRange;
                var ruleArr = rule.Split(':');
                if (ruleArr.Length == 1)
                {
                    q.Enqueue(new State(ruleArr[0], xRange, mRange, aRange, sRange));
                    continue;
                }

                var comparison = ruleArr[0];
                var compared = comparison[0];
                var op = comparison[1];
                var comparedTo = int.Parse(string.Join("", comparison.Skip(2)));
                var outcome = ruleArr[1];

                State state;
                if (op == '>')
                {
                    switch (compared)
                    {
                        case 'x':
                            state = new State(
                                outcome,
                                (comparedTo + 1, xHigh),
                                mRange,
                                aRange,
                                sRange
                            );
                            xRange = (xLow, comparedTo);
                            break;
                        case 'm':
                            state = new State(
                                outcome,
                                xRange,
                                (comparedTo + 1, mHigh),
                                aRange,
                                sRange
                            );
                            mRange = (mLow, comparedTo);
                            break;
                        case 'a':
                            state = new State(
                                outcome,
                                xRange,
                                mRange,
                                (comparedTo + 1, aHigh),
                                sRange
                            );
                            aRange = (aLow, comparedTo);
                            break;
                        case 's':
                            state = new State(
                                outcome,
                                xRange,
                                mRange,
                                aRange,
                                (comparedTo + 1, sHigh)
                            );
                            sRange = (sLow, comparedTo);
                            break;
                        default:
                            throw new Exception("OOPS");
                    }
                }
                else
                {
                    switch (compared)
                    {
                        case 'x':
                            state = new State(
                                outcome,
                                (xLow, comparedTo - 1),
                                mRange,
                                aRange,
                                sRange
                            );
                            xRange = (comparedTo, xHigh);
                            break;
                        case 'm':
                            state = new State(
                                outcome,
                                xRange,
                                (mLow, comparedTo - 1),
                                aRange,
                                sRange
                            );
                            mRange = (comparedTo, mHigh);
                            break;
                        case 'a':
                            state = new State(
                                outcome,
                                xRange,
                                mRange,
                                (aLow, comparedTo - 1),
                                sRange
                            );
                            aRange = (comparedTo, aHigh);
                            break;
                        case 's':
                            state = new State(
                                outcome,
                                xRange,
                                mRange,
                                aRange,
                                (sLow, comparedTo - 1)
                            );
                            sRange = (comparedTo, sHigh);
                            break;
                        default:
                            throw new Exception("OOPS");
                    }
                }
                q.Enqueue(state);
            }
        }

        return sum;
    }

    public readonly record struct State(
        string Current,
        (int, int) XRange,
        (int, int) MRange,
        (int, int) ARange,
        (int, int) SRange
    );

    public static char SimulateWorflows(
        Part part,
        Dictionary<string, IEnumerable<Func<Part, string>>> workflows
    )
    {
        var currentWorkflow = "in";
        while (true)
        {
            var flow = workflows[currentWorkflow];
            foreach (var step in flow)
            {
                var res = step(part);
                if (res == "A")
                {
                    return 'A';
                }

                if (res == "R")
                {
                    return 'R';
                }

                if (res == currentWorkflow)
                {
                    continue;
                }

                currentWorkflow = res;
                break;
            }
        }
    }

    public static (string, IEnumerable<Func<Part, string>>) ParseWorkflow(string line)
    {
        var (name, rules) = ParseRules(line);

        List<Func<Part, string>> funcs = [];
        foreach (var rule in rules)
        {
            var ruleArr = rule.Split(':');
            if (ruleArr.Length == 1)
            {
                funcs.Add((Part p) => ruleArr[0]);
                continue;
            }

            var comparison = ruleArr[0];
            var target = int.Parse(string.Join("", comparison.Skip(2)));
            var outcome = ruleArr[1];

            funcs.Add(
                (Func<Part, string>)(
                    (Part p) =>
                    {
                        var partRating = comparison[0] switch
                        {
                            'x' => p.X,
                            'm' => p.M,
                            'a' => p.A,
                            's' => p.S,
                            _ => throw new Exception($"Hi {comparison[0]}")
                        };

                        if (Compare(partRating, target, comparison[1]))
                        {
                            return outcome;
                        }

                        return name;
                    }
                )
            );
        }

        return (name, funcs);
    }

    public static (string, IEnumerable<string>) ParseRules(string line)
    {
        var lineArr = line.Split('{');
        var name = lineArr[0];
        var rules = lineArr[1].Replace("}", "").Split(',');

        return (name, rules);
    }

    public static Part ParseRatings(string line)
    {
        var arr = line.Split(',');
        var x = int.Parse(arr[0].Split('=')[1]);
        var m = int.Parse(arr[1].Split('=')[1]);
        var a = int.Parse(arr[2].Split('=')[1]);
        var s = int.Parse(arr[3].Split('=')[1].Replace("}", ""));

        return new Part(x, m, a, s);
    }

    public static bool Compare(int a, int b, char comp)
    {
        if (comp == '>')
        {
            return a > b;
        }
        if (comp == '<')
        {
            return a < b;
        }

        return a == b;
    }

    public readonly record struct Part(int X, int M, int A, int S);
}
