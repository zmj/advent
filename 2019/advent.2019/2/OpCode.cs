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
            int Throw() => throw new ArgumentException($"unknown op: {value}");
            Op = (Operation)(value % 100);
            int modifiers = (value - (int)Op) / 100;
            var (paramCount, lastMustBePosition) = Op switch
            {
                Operation.Add => (3, true),
                Operation.Multiply => (3, true),
                Operation.Store => (1, true),
                Operation.Output => (1, false),
                Operation.Exit => (0, false),
                _ => (Throw(), false),
            };
            Params = new ParamMode[paramCount];
            for (int i = 0; i < Params.Length; i++)
            {
                int base10 = 1;
                for (int b = Params.Length - 1; b > i; b--) { base10 *= 10; }
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
        Store = 3,
        Output = 4,
        Exit = 99,
    }

    public enum ParamMode
    {
        Position = 0,
        Immediate = 1,
    }
}
