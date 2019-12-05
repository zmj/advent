using System;
using System.Collections.Generic;
using System.Text;

namespace advent._2019._2
{
    public readonly struct OpCode
    {
        public Operation Op { get; }
        public ParamMode[] Params { get; }

        public OpCode(int value)
        {
            int Throw() => throw new ArgumentException($"invalid op: {value}");
            Op = (Operation)(value % 100);
            int modifiers = (value - (int)Op) / 100;
            var (paramCount, lastMustBePosition) = Op switch
            {
                Operation.Add => (3, true),
                Operation.Multiply => (3, true),
                Operation.Input => (1, true),
                Operation.Output => (1, false),
                Operation.JumpIfTrue => (2, false),
                Operation.JumpIfFalse => (2, false),
                Operation.LessThan => (3, true),
                Operation.Equals => (3, true),
                Operation.Exit => (0, false),
                _ => (Throw(), false),
            };
            Params = new ParamMode[paramCount];
            for (int i = Params.Length-1; i>=0; i--)
            {
                int base10 = 1;
                for (int b = 0; b<i; b++) { base10 *= 10; }
                int digit = modifiers / base10;
                Params[i] = (ParamMode)digit;
                if (!(Params[i] == ParamMode.Position ||
                    Params[i] == ParamMode.Immediate)) { Throw(); }
                modifiers -= digit * base10;
            }
            if (lastMustBePosition && Params[^1] != ParamMode.Position) { Throw(); }
        }
    }

    public enum Operation
    {
        Unknown = 0,
        Add = 1,
        Multiply = 2,
        Input = 3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        Exit = 99,
    }

    public enum ParamMode
    {
        Position = 0,
        Immediate = 1,
    }
}
