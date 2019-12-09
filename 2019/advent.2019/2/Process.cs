using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advent._2019._2
{
    public class Process
    {
        private readonly List<(long offset, long[])> _memory = new List<(long, long[])>();

        private long _instructionPointer;
        private long _relativeBaseOffset;

        public Process(long[] program, int ip = 0)
        {
            _memory.Add((offset: 0, program.ToArray()));
            _instructionPointer = ip;
        }

        public ref long this[long addr]
        {
            get
            {
                if (addr < 0) { throw new ArgumentOutOfRangeException("negative address"); }
                long? offset = null;
                long[]? memory = null;
                foreach (var (o, m) in _memory)
                {
                    if (addr >= o && addr < o + m.Length)
                    {
                        (offset, memory) = (o, m);
                        break;
                    }
                }
                if (offset == null || memory == null)
                {
                    (offset, memory) = (addr, new long[1024]);
                    _memory.Add((offset.Value, memory));
                }
                return ref memory[(int)(addr - offset)];
            }
        }

        public ref long Read() => ref this[_instructionPointer++];

        public ref long Ip => ref _instructionPointer;

        public ref long Rb => ref _relativeBaseOffset;

        public long[] Data => _memory[0].Item2;
    }
}
