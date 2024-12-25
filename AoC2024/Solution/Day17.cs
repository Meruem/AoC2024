using System.Diagnostics;
using System.Text;
using AoC2024.Utils;

namespace AoC2024.Solution;

// 2,4, => B = A % 8         
// 1,1, => B = B XOR 1
// 7,5, => C = A / 2 ** B
// 0,3, => A = A / 8
// 4,3, => B = B XOR C
// 1,6, => B = B XOR 6
// 5,5, => OUT (B % 8)
// 3,0 => A == 0 ? END : JUMP 0
public class Day17 : SolutionBase
{
    public override string Part1()
    {
        var regA = Lines[0].Replace("Register A: ", "").ToInt();
        var regB = Lines[1].Replace("Register B: ", "").ToInt();
        var regC = Lines[2].Replace("Register C: ", "").ToInt();
        var program = Lines[4].Replace("Program: ", "").Split(',').Select(byte.Parse).ToList();
        var computer = new Computer();
        var output = computer.RunProgram(program, regA, regB, regC);
        return string.Join(',', output);
    }

    public List<byte> SimpleCompute(long a)
    {
        long b;
        long c;
        var output = new List<byte>();
        while (true)
        {
            b = a % 8; // take last 3 bits     
            
            b = b ^ 1; // flip high bit for b
            c = a >>> (byte)b; // take bytes b -> b+2 from end
            b = b ^ c; // and xor them with b
            b = b ^ 6; // flip first and second bit for b

            a = a >>> 3; // discard last 3 bits of a and loop
            var byteB = (byte)(b % 8);
            output.Add(byteB);
            if (a == 0) return output;
        }
    }


    public record Solution(Dictionary<int, bool> Bits)
    {
        public bool TrySetBit(int index, bool newValue)
        {
            if (Bits.TryGetValue(index, out bool value) && value != newValue) return false;
            Bits[index] = newValue;
            return true;
        }

        public Solution Copy()
        {
            return new Solution(new Dictionary<int, bool>(Bits));
        }
    }

    public override string Part2()
    {
        var expected = new List<byte> { 2, 4, 1, 1, 7, 5, 0, 3, 4, 3, 1, 6, 5, 5, 3, 0 };
        // var expected = new List<byte> { 0,3,5,4,3,0 };
        var variants = Enumerable.Range(0, 8).Select(x => (byte)x).ToList();
        //var maxSize = (expected.Count + 1) * 3;
        var solutionStart = new Solution([]);

        List<Solution> solutions = [solutionStart];

        for (int i = 0; i < expected.Count; i++)
        {
            var expectedByte = expected[i];
            var e1 = (expectedByte & 1) == 1;
            var e2 = (expectedByte & 2) == 2;
            var e3 = (expectedByte & 4) == 4;
            var nextSolutions = new List<Solution>();
            foreach (var solution in solutions)
            {
                foreach (var v in variants)
                {
                    var x1 = (v & 1) == 1;
                    var x2 = (v & 2) == 2;
                    var x3 = (v & 4) == 4;
                    var index = i * 3;
                    var newSolution = solution.Copy();
                    if (!newSolution.TrySetBit(index, x1)) continue;
                    if (!newSolution.TrySetBit(index + 1, x2)) continue;
                    if (!newSolution.TrySetBit(index + 2, x3)) continue;

                    // var shift = 3;
                    var shift = v ^ 1;
                    var cIndex = index + shift;
                    // var c1 = e1;
                    // var c2 = e2;
                    // var c3 = e3;
                    var c1 = e1 == x1;
                    var c2 = e2 == x2;
                    var c3 = e3 == x3;

                    if (!newSolution.TrySetBit(cIndex, c1)) continue;
                    if (!newSolution.TrySetBit(cIndex + 1, c2)) continue;
                    if (!newSolution.TrySetBit(cIndex + 2, c3)) continue;
                    
                    nextSolutions.Add(newSolution);
                }
            }
            
            solutions = nextSolutions;
        }

        return solutions.Min(sol =>
        {
            var str = string.Join("", sol.Bits.OrderByDescending(b => b.Key).Select(b => b.Value ? "1" : "0"));
            return Convert.ToInt64(str, 2);
        }).ToString();
    }
}