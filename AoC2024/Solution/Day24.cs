using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day24 : SolutionBase
{
    private OpLine? GetOperation(List<OpLine> operations, string a, string b, string? op = null,
        string? c = null)
    {
        return operations.FirstOrDefault(o =>
            (o.A == a && o.B == b || o.A == b && o.B == a)
            && (op is null || o.Op == op)
            && (c is null || o.C == c));
    }

    public override string Part2()
    {
        var items = new List<string> { "shj", "z07", "tpk", "wkb", "pfn", "z23", "z27", "kcd" }.Order();
        return string.Join(',', items);
    }

    //  { A = gnj, B = scw, C = shj, Op = XOR } <-> OpLine { A = gnj, B = scw, C = z07, Op = AND }
    //  wkb
    // switching: gnj XOR scw -> shj <-> gnj AND scw -> z07
    // wrong and: y16 AND x16 -> tpk <-> wrong xor: x16 XOR y16 -> wkb
    // gvb XOR pvr -> pfn <-> jwb OR hjp -> z23
    // x27 AND y27 -> z27 <-> vpt XOR hdg -> kcd
    // shj, z07, tpk, wkb, pfn, z23, z27, kcd
    // 
    public  string Part2_A()
    {
        var (wires, operations) = GetInput();
        var x = GetNumber(wires.Select(kvp => (kvp.Key, kvp.Value)), "x");
        var y = GetNumber(wires.Select(kvp => (kvp.Key, kvp.Value)), "y");
        var z = x + y;
        var zStr = Convert.ToString(z, 2);

        var inputs = Enumerable.Range(0, 45)
            .Select(i => (A: $"x{i:D2}", B: $"y{i:D2}")).ToList();

        var startAnds = inputs.Select(input => GetOperation(operations, input.A, input.B, "AND")).ToList();
        var startXors = inputs.Select(input => GetOperation(operations, input.A, input.B, "XOR")).ToList();
        var ors = operations.Where(op => op.Op == "OR").ToList();
        var ands = operations.Where(op => op.Op == "AND").ToList();
        var xors = operations.Where(op => op.Op == "XOR").ToList();
        
        var wrongOps = new List<OpLine>();

        foreach (var op in ors)
        {
            if (ands.All(o => o.C != op.A) || ands.All(o => o.C != op.B))
            {
                wrongOps.Add(op);
                Console.WriteLine($"wrong or: {op}");
            }
        }

        foreach (var op in ands)
        {
            var found = operations.Where(o => o.A == op.C || o.B == op.C).ToList();
            if (found.Count != 1)
            {
                wrongOps.Add(op);
                Console.WriteLine($"wrong and: {op}");
            }
        }

        foreach (var op in xors)
        {
            if (!op.C.StartsWith("z") && ands.Count(e => e.A == op.C || e.B == op.C) != 1)
            {
                wrongOps.Add(op);
                Console.WriteLine($"wrong xor: {op}");
            }
        }

        foreach (var op in wrongOps.Zip(wrongOps).Zip(wrongOps))
        {
            var ((first, second), third) = op;
            if (first == second || first == third || second == third) continue;
            
        }

        string c = GetOperation(operations,"x00", "y00", "AND")!.C;
        var switched = new List<(OpLine From, OpLine To)>();
        var missing = new List<OpLine>();
        var usedOperations = new List<OpLine>();
        for (int i = 1; i < 45; i++)
        {
            // 222 ops
            // missing: 

            // c0 = 0;
            // 1. and1 = x1 & y1 (45)
            // 2. xor1 = x1 ^ y1 (45)
            
            // 3. z1 = xor1 ^ c1 (44)
            // 4. tmp1 = xor1 & c1 (44)
            // 5. c1 = tmp1 | and1 (44)
            //Console.WriteLine(i);
            var andOp =  GetOperation(operations,$"x{i:D2}", $"y{i:D2}", "AND")!;
            var xorOp = GetOperation(operations,$"x{i:D2}", $"y{i:D2}", "XOR")!;
            usedOperations.AddRange(andOp, xorOp);
            // exist (x1 xor y1) xor c0 => z1
            var op1 = GetOperation(operations,a: xorOp.C, b: c, "XOR");
            // exist (x1 xor y1) and c0 => tmp
            var op2 = GetOperation(operations,xorOp.C, c, "AND");
            if (op1 is null)
            {
                var misop = new OpLine(xorOp.C, c, "XOR", $"z{i:D2}");
                missing.Add(misop);
                Console.WriteLine($"missing: {misop}");
            }

            usedOperations.Add(op1!);

            if (op2 is null)
            {
                Console.WriteLine($"i: {i} missing: {xorOp.C}, {c}, AND");

                Console.WriteLine("Ops:   ---------------");
                foreach (var operation in usedOperations)
                {
                    Console.WriteLine(operation);
                    
                }
            }

            if (op1 is not null && op2 is not null && op1.C != $"z{i:D2}" && op2.C == $"z{i:D2}")
            {
                // easy switch
                switched.Add((op1, op2));
                Console.WriteLine($"switching: {op1} <-> {op2}");
                op2 = op1;
            }

            usedOperations.Add(op2!);

            // exist tmp OR (x1 and y1) => c1
            var op3 = GetOperation(operations,op2.C, andOp.C, "OR");
            if (op3 is null)
            {
                Console.WriteLine($"i: {i} missing: {op2.C}, {andOp.C}, OR");

                Console.WriteLine("Ops:   ---------------");
                foreach (var operation in usedOperations)
                {
                    Console.WriteLine(operation);
                    
                }
            }

            usedOperations.Add(op3);

            c = op3.C;
        }

        var counter = 0;
        while (true)
        {
            counter++;
            var pulse = new List<(string, bool, OpLine)>();
            for (int i = operations.Count - 1; i >= 0; i--)
            {
                var op = operations[i];
                if (wires.TryGetValue(op.A, out bool a) && wires.TryGetValue(op.B, out bool b) &&
                    !wires.ContainsKey(op.C))
                {
                    pulse.Add((op.C, ExecuteOperation(a, b, op.Op), op));
                }
            }

            if (pulse.Count == 0) break;
            Console.WriteLine($"counter: {counter} : x {pulse.Count}");

            if (pulse.Count is < 90)
            {
                Console.WriteLine(string.Join(" | ", pulse));
            }
            else
            {
                Console.WriteLine(string.Join(" | ",
                    pulse.Where(p => p.Item3.A.StartsWith("x") || p.Item3.A.StartsWith("y"))));
            }

            foreach (var p in pulse)
            {
                wires[p.Item1] = p.Item2;
            }
        }

        var actualZ = GetNumber(wires.Select(kvp => (kvp.Key, kvp.Value)), "z");
        if (actualZ == z) Console.WriteLine("Ok");
        else Console.WriteLine("Not ok");
        return "";
    }


    public override string Part1()
    {
        var (wires, operations) = GetInput();
        return Solve(wires, operations).ToString();
    }

    private static long Solve(Dictionary<string, bool> wires, List<OpLine> operations)
    {
        while (operations.Count > 0)
        {
            for (int i = operations.Count - 1; i >= 0; i--)
            {
                var op = operations[i];
                if (wires.TryGetValue(op.A, out bool a) && wires.TryGetValue(op.B, out bool b))
                {
                    wires[op.C] = ExecuteOperation(a, b, op.Op);
                    operations.RemoveAt(i);
                }
            }
        }

        return GetNumber(wires.Select(kvp => (kvp.Key, kvp.Value)), "z");
    }

    private static long GetNumber(IEnumerable<(string, bool)> wires, string start)
    {
        var binArray = wires
            .Where(kvp => kvp.Item1.StartsWith(start))
            .OrderByDescending(v => v.Item1)
            .Select(kvp => kvp.Item2 ? "1" : "0");

        var binStr = string.Join("", binArray);
        return Convert.ToInt64(binStr, 2);
    }

    private static bool ExecuteOperation(bool a, bool b, string op) =>
        op switch
        {
            "AND" => a && b,
            "OR" => a || b,
            "XOR" => a ^ b,
            _ => throw new ArgumentException($"Invalid operation: {op}")
        };

    public record OpLine(string A, string B, string C, string Op)
    {
        override public string ToString() => $"{A} {Op} {B} -> {C}";
    }

    private  (Dictionary<string, bool> wires, List<OpLine> operations) GetInput()
    {
        var parts = Lines.SplitBy("").ToList();
        var wires = parts[0].Select(line =>
        {
            var (id, state) = line.Split(": ").ToPair();
            return (id, state == "1" ? true : false);
        }).ToDictionary();

        var operations = parts[1].Select(line =>
        {
            var (left, right) = line.Split(" -> ").ToPair();
            var (a, op, b) = left.Split(" ").ToTriple();
            return new OpLine(A: a, B: b, Op: op, C: right);
        }).ToList();
        return (wires, operations);
    }
}