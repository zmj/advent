﻿using System;
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
            while (ExecuteIntruction(process)) { }
            return process.Data;
        }

        public bool ExecuteIntruction(Process proc)
        {
            Func<int, int, int> binaryOp;
            ReadOnlySpan<int> binaryOpArgs;
            int opCode = proc.ExecutionPointer[0];
            switch (opCode)
            {
                case 1:
                    binaryOp = (x, y) => x + y;
                    binaryOpArgs = proc.ExecutionPointer.Slice(1, 3);
                    break;
                case 2:
                    binaryOp = (x, y) => x * y;
                    binaryOpArgs = proc.ExecutionPointer.Slice(1, 3);
                    break;
                case 99: //done
                    return false;
                default:
                    throw new ArgumentException(opCode.ToString());
            }
            // do binary operation
            var (op1, op2) = (proc.Data[binaryOpArgs[0]], proc.Data[binaryOpArgs[1]]);
            int result = binaryOp(op1, op2);
            proc.Data[binaryOpArgs[2]] = result;
            proc.Advance(1 + binaryOpArgs.Length);
            return true;
        }
    }
}
