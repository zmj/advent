using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._7
{
    public readonly struct Amplifiers
    {
        private readonly int _n;

        public Amplifiers(int n) => _n = n;

        public async ValueTask<(int, int[])> FindBestPhaseSetting(int[] program)
        {
            var circuit = new Circuit(program);
            int maxOutput = int.MinValue;
            int[]? maxOutputSettings = null;
            foreach (var ps in Permutations.Enumerate(Enumerable.Range(0, _n).ToArray()))
            {
                int output = await circuit.Run(ps);
                if (output > maxOutput)
                {
                    maxOutput = output;
                    maxOutputSettings = ps.ToArray();
                }
            }
            return (maxOutput, maxOutputSettings ?? throw new Exception("none?!"));
        }

        public async ValueTask<int> FindMaxOutput(IAsyncEnumerable<string> lines)
        {
            var program = _2.IntComputer.ParseLine(await lines.Single());
            return (await FindBestPhaseSetting(program)).Item1;
        }
    }
}
