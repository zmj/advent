using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._2
{
    public readonly struct IntComputer
    {
        public static int[] ParseLine(string line)
        {
            var values = line.Split(',');
            return values.Select(int.Parse).ToArray();
        }

        public async ValueTask<int> ParseFixAndRun(
            IAsyncEnumerable<string> lines,
            int noun, int verb)
        {
            var program = ParseLine(await lines.Single());
            return FixAndRun(program, noun, verb);
        }

        public async ValueTask<int> FindNounAndVerb(
            IAsyncEnumerable<string> lines,
            int output)
        {
            var program = ParseLine(await lines.Single());
            const int max = 100;
            for (int noun = 0; noun < max; noun++)
            {
                for (int verb = 0; verb < max; verb++)
                {
                    try
                    {
                        int x = FixAndRun(program, noun, verb);
                        if (x == output)
                        {
                            return 100 * noun + verb;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            throw new Exception("not found");
        }

        public int FixAndRun(int[] program, int noun, int verb)
        {
            program[1] = noun;
            program[2] = verb;
            return RunProgram(program)[0];
        }

        public int[] RunProgram(int[] program)
        {
            var process = new Process(program);
            while (ExecuteInstruction(process)) { }
            return process.Data;
        }

        public bool ExecuteInstruction(Process proc)
        {
            var opCode = new OpCode(proc.Read());
            switch (opCode.Op)
            {
                case Operation.Add:
                    Add(Param(0), Param(1), out Param(2));
                    break;
                case Operation.Multiply:
                    Multiply(Param(0), Param(1), out Param(2));
                    break;
                case Operation.Exit:
                    return false;
                default:
                    throw new ArgumentException(opCode.Op.ToString());
            }
            return true;

            ref int Param(int paramIndex)
            {
                ParamMode mode = opCode.Params[paramIndex];
                if (mode == ParamMode.Position) { return ref proc.Data[proc.Read()]; }
                else if (mode == ParamMode.Immediate) { return ref proc.Read(); }
                else { throw new ArgumentException(mode.ToString()); }
            }
        }

        public static void Add(int x, int y, out int z) => z = x + y;
        public static void Multiply(int x, int y, out int z) => z = x * y;
    }
}
