using advent._2019._2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace advent._2019._7
{
    public readonly struct Circuit
    {
        private readonly int[] _program;

        public Circuit(int[] program) => _program = program;

        public async Task<int> Run(int[] phaseSettings, bool loop)
        {
            var input = Channel.CreateUnbounded<int>();
            var output = loop ? input : Channel.CreateUnbounded<int>();
            var computers = ComputerChain(
                phaseSettings.Length,
                input.Reader,
                output.Writer,
                phaseSettings[1..]);
            input.Writer.MustWrite(phaseSettings[0]);
            input.Writer.MustWrite(0);
            var prog = _program;
            await Task.WhenAll(
                computers.Select(c => 
                    Task.Run(() => c.RunProgram(prog))));
            return output.Reader.MustRead();
        }

        public static IntComputer[] ComputerChain(
            int n,
            ChannelReader<int> firstInput,
            ChannelWriter<int> lastOutput,
            ReadOnlySpan<int> initialInputs)
        {
            if (initialInputs.Length != n-1) { throw new ArgumentException(); }
            var computers = new IntComputer[n];
            ChannelReader<int> input = firstInput;
            for (int i = 0; i < n - 1; i++)
            {
                var channel = Channel.CreateUnbounded<int>();
                computers[i] = new IntComputer(input, channel.Writer);
                input = channel.Reader;
                channel.Writer.MustWrite(initialInputs[i]);
            }
            computers[^1] = new IntComputer(input, lastOutput);
            return computers;
        }
    }
}
