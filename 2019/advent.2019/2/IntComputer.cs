using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace advent._2019._2
{
    public readonly struct IntComputer
    {
        private readonly ChannelReader<long> _in;
        private readonly ChannelWriter<long> _out;

        public IntComputer(ChannelReader<long> input, ChannelWriter<long> output) =>
            (_in, _out) = (input, output);

        public static long[] ParseLine(string line)
        {
            var values = line.Split(',');
            return values.Select(long.Parse).ToArray();
        }

        public async ValueTask ParseAndRun(
            IAsyncEnumerable<string> lines)
        {
            var program = ParseLine(await lines.Single());
            await RunProgram(program);
        }

        public async ValueTask<long[]> RunProgram(long[] program)
        {
            var process = new Process(program);
            while (await ExecuteInstruction(process)) { }
            _out?.Complete();
            return process.Data;
        }

        public async ValueTask<bool> ExecuteInstruction(Process proc)
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
                case Operation.Input:
                    var x = await Input();
                    Param(0) = x;
                    break;
                case Operation.Output:
                    await Output(Param(0));
                    break;
                case Operation.JumpIfTrue:
                    JumpIfTrue(Param(0), Param(1), ref proc.Ip);
                    break;
                case Operation.JumpIfFalse:
                    JumpIfFalse(Param(0), Param(1), ref proc.Ip);
                    break;
                case Operation.LessThan:
                    LessThan(Param(0), Param(1), out Param(2));
                    break;
                case Operation.Equals:
                    Equals(Param(0), Param(1), out Param(2));
                    break;
                case Operation.Exit:
                    return false;
                default:
                    throw new ArgumentException(opCode.Op.ToString());
            }
            return true;

            ref long Param(int paramIndex)
            {
                ParamMode mode = opCode.Params[paramIndex];
                if (mode == ParamMode.Position) { return ref proc[proc.Read()]; }
                else if (mode == ParamMode.Immediate) { return ref proc.Read(); }
                else { throw new ArgumentException(mode.ToString()); }
            }
        }

        public static void Add(long x, long y, out long z) => z = checked(x + y);
        public static void Multiply(long x, long y, out long z) => z = checked(x * y);
        public ValueTask<long> Input() => _in.ReadAsync();
        public ValueTask Output(long x) => _out.WriteAsync(x);
        public void JumpIfTrue(long x, long y, ref long z) { if (x != 0) { z = y; } }
        public void JumpIfFalse(long x, long y, ref long z) { if (x == 0) { z = y; } }
        public void LessThan(long x, long y, out long z) => z = x < y ? 1 : 0;
        public void Equals(long x, long y, out long z) => z = x == y ? 1 : 0;

        public async ValueTask<long> ParseFixAndRun(
            IAsyncEnumerable<string> lines,
            int noun, int verb)
        {
            var program = ParseLine(await lines.Single());
            return await FixAndRun(program, noun, verb);
        }

        public async ValueTask<long> FindNounAndVerb(
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
                        var x = await FixAndRun(program, noun, verb);
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

        public async ValueTask<long> FixAndRun(long[] program, int noun, int verb)
        {
            program[1] = noun;
            program[2] = verb;
            return (await RunProgram(program))[0];
        }
    }
}
