namespace AoC2024.Solution;

public class Computer
{
    private long A { get; set; }
    private long B { get; set; }
    private long C { get; set; }

    private List<byte> _program = [];
    //private byte[] _output = new byte[16];

    private int _instructionPointer;

    //private Dictionary<(int, int), int> _divideCache = [];

    public List<byte> RunProgram(List<byte> program, long regA, long regB, long regC, List<byte>? expectedOutput = null)
    {
        _instructionPointer = 0;
        _program = program;
        A = regA;
        B = regB;
        C = regC;
        var output = new List<byte>();
        while (ExecuteInstruction(o =>
               {
                   if (expectedOutput is not null && (output.Count == expectedOutput.Count || expectedOutput[output.Count] != o)) return false;
                   output.Add(o);
                   return true;
               }))
        {
        }

        return output;
    }

    private long Divide(long a, byte b)
    {
        //if (_divideCache.TryGetValue((a, b), out var result)) return result;
        var res = (long)(a / Math.Pow(2, GetComboOperand(b)));
        //  _divideCache[(a, b)] = res;
        return res;
    }

    private bool ExecuteInstruction(Func<byte, bool> output)
    {
        if (_instructionPointer >= _program.Count - 1) return false;

        var opcode = _program[_instructionPointer];
        var operand = _program[_instructionPointer + 1];
        switch (opcode)
        {
            // adv 0
            case 0:
                A = Divide(A, operand);
                _instructionPointer += 2;
                break;
            // bxl 1
            case 1:
                B = B ^ operand;
                _instructionPointer += 2;
                break;
            // bst 2
            case 2:
                B = GetComboOperand(operand) % 8;
                _instructionPointer += 2;
                break;
            // jnz 3
            case 3:
                if (A != 0)
                {
                    _instructionPointer = operand;
                }
                else
                {
                    _instructionPointer += 2;
                }

                break;
            // bxc 4
            case 4:
                B = B ^ C;
                _instructionPointer += 2;
                break;
            // out 5
            case 5:
                var o = (byte)(GetComboOperand(operand) % 8);
                var cont = output(o);
                if (!cont) return false;
                _instructionPointer += 2;
                break;
            // bdv 6
            case 6:
                B = Divide(A, operand);
                _instructionPointer += 2;
                break;
            // cdv 7
            case 7:
                C = Divide(A, operand);
                _instructionPointer += 2;
                break;
        }

        return true;
    }

    private long GetComboOperand(byte operand) =>
        operand switch
        {
            <= 3 => operand,
            4 => A,
            5 => B,
            6 => C,
            _ => throw new Exception($"Unknown combo operand: {operand}")
        };
}