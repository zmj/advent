using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advent._2019._2
{
    public class Process
    {
        public int[] Data { get; }

        private int _executionPointer;

        public Process(int[] program) => Data = program.ToArray();

        public ReadOnlySpan<int> ExecutionPointer =>
            Data.AsSpan().Slice(_executionPointer);

        public void Advance(int count)
        {
            var newPosition = _executionPointer + count;
            if (newPosition >= Data.Length) { throw new ArgumentOutOfRangeException(); }
            _executionPointer = newPosition;
        }
    }
}
