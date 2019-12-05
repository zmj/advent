using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advent._2019._2
{
    public class Process
    {
        public int[] Data { get; }

        private int _instructionPointer;

        public Process(int[] program, int ip = 0)
        {
            Data = program.ToArray();
            _instructionPointer = ip;
        }

        public ref int Read()
        {
            if (_instructionPointer >= Data.Length) { throw new InvalidOperationException(); }
            return ref Data[_instructionPointer++];
        }

        public ref int Ip => ref _instructionPointer;
    }
}
