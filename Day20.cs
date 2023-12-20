namespace AoC2023;

public class Day20 : IDay<IEnumerable<string>, long>
{
    public static long SolvePart1(IEnumerable<string> input)
    {
        ParseInput(
            input,
            out var map,
            out _,
            out var forwardConnections,
            out var flipFlopStatus,
            out var conjunctionLastRecievedPulses
        );

        var pushes = 0;
        var l = 0;
        var h = 0;
        while (pushes < 1000)
        {
            pushes++;
            var q = new Queue<(string, bool, string)>();
            var sendsTo = forwardConnections["broadcaster"];
            l++; // coming from the button press

            foreach (var st in sendsTo)
            {
                q.Enqueue(("broadcaster", false, st));
            }

            while (q.Count > 0)
            {
                var (src, pulse, dest) = q.Dequeue();
                if (pulse)
                {
                    h++;
                }
                else
                {
                    l++;
                }
                // Console.WriteLine($"{src} {pulse} -> {dest}");

                bool p = pulse;

                if (forwardConnections.TryGetValue(dest, out sendsTo))
                {
                    if (map[dest] == ModuleType.FlipFlop)
                    {
                        if (!pulse)
                        {
                            flipFlopStatus[dest] = !flipFlopStatus[dest];
                            p = flipFlopStatus[dest];
                            foreach (var st in sendsTo)
                            {
                                q.Enqueue((dest, p, st));
                            }
                        }
                    }
                    else
                    {
                        conjunctionLastRecievedPulses[dest][src] = p;
                        p = !conjunctionLastRecievedPulses[dest].Values.All(v => v == true);
                        foreach (var st in sendsTo)
                        {
                            q.Enqueue((dest, p, st));
                        }
                    }
                }
            }
        }

        return l * h;
    }

    public static long SolvePart2(IEnumerable<string> input)
    {
        ParseInput(
            input,
            out var map,
            out var backConnections,
            out var forwardConnections,
            out var flipFlopStatus,
            out var conjunctionLastRecievedPulses
        );

        const string destination = "rx";
        // There is only one back connection
        var destinationBackConnection = backConnections[destination][0];
        var countOfDestinationBackConnectionBackConnections = backConnections[
            destinationBackConnection
        ].Count;
        var found = 0;
        var cycles = new long[countOfDestinationBackConnectionBackConnections];
        long pushes = 0;

        while (found < countOfDestinationBackConnectionBackConnections)
        {
            pushes++;
            var q = new Queue<(string, bool, string)>();
            var sendsTo = forwardConnections["broadcaster"];

            foreach (var st in sendsTo)
            {
                q.Enqueue(("broadcaster", false, st));
            }

            while (q.Count > 0)
            {
                var (src, pulse, dest) = q.Dequeue();
                if (dest == destinationBackConnection && pulse)
                {
                    cycles[found] = pushes;
                    found++;
                    // Console.WriteLine(pushes);
                    // Console.WriteLine($"{src} {pulse} -> {dest}");
                }

                if (dest == destination && !pulse)
                {
                    return pushes;
                }

                bool p = pulse;

                if (forwardConnections.TryGetValue(dest, out sendsTo))
                {
                    if (map[dest] == ModuleType.FlipFlop)
                    {
                        if (!pulse)
                        {
                            flipFlopStatus[dest] = !flipFlopStatus[dest];
                            p = flipFlopStatus[dest];
                            foreach (var st in sendsTo)
                            {
                                q.Enqueue((dest, p, st));
                            }
                        }
                    }
                    else
                    {
                        conjunctionLastRecievedPulses[dest][src] = p;
                        p = !conjunctionLastRecievedPulses[dest].Values.All(v => v == true);
                        foreach (var st in sendsTo)
                        {
                            q.Enqueue((dest, p, st));
                        }
                    }
                }
            }
        }

        // Somehow 🤔 for this input
        // [bc1, bc2, bc3, ... bcn] -> conjunction -> rx
        //
        // bc1 ... bcn are cyclic, so we just need to find the earliest cycle of pushes
        // where they will all emit a high pulse
        return Helpers.LCM(cycles);
    }

    public enum ModuleType
    {
        Broadcaster,
        FlipFlop,
        Conjunction
    }

    public static void ParseInput(
        IEnumerable<string> input,
        out Dictionary<string, ModuleType> map,
        out Dictionary<string, List<string>> backConnections,
        out Dictionary<string, List<string>> forwardConnections,
        out Dictionary<string, bool> flipFlopStatus,
        out Dictionary<string, Dictionary<string, bool>> conjunctionLastRecievedPulses
    )
    {
        map = [];
        backConnections = [];
        forwardConnections = [];
        flipFlopStatus = [];
        conjunctionLastRecievedPulses = [];

        foreach (var line in input)
        {
            var arr = line.Split(" -> ");
            var moduleType = arr[0][0] switch
            {
                '%' => ModuleType.FlipFlop,
                '&' => ModuleType.Conjunction,
                'b' => ModuleType.Broadcaster,
                _ => throw new Exception($"OOPS: {arr[0][0]} - {line}")
            };
            var moduleName = moduleType == ModuleType.Broadcaster ? arr[0] : arr[0][1..];

            if (moduleType == ModuleType.FlipFlop)
            {
                flipFlopStatus[moduleName] = false;
            }

            var sendsTo = arr[1].Split(", ").ToList();
            forwardConnections[moduleName] = sendsTo;
            map[moduleName] = moduleType;

            foreach (var module in sendsTo)
            {
                if (backConnections.TryGetValue(module, out var c))
                {
                    c.Add(moduleName);
                }
                else
                {
                    backConnections.Add(module, [moduleName]);
                }
            }
        }

        foreach (var node in map.Where(kvp => kvp.Value == ModuleType.Conjunction))
        {
            conjunctionLastRecievedPulses[node.Key] = new();
            foreach (var bc in backConnections[node.Key])
            {
                conjunctionLastRecievedPulses[node.Key][bc] = false;
            }
        }
    }
}
