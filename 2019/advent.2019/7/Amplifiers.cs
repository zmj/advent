using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._7
{
    public readonly struct Amplifiers
    {
        private readonly int[] _phaseSettingValues;

        public Amplifiers(int[] phaseSettingValues) =>
            _phaseSettingValues = phaseSettingValues;

        public async ValueTask<(long, int[])> FindBestPhaseSetting(
            long[] program, bool feedbackLoop = false)
        {
            var circuit = new Circuit(program);
            long? maxOutput = null;
            int[]? maxOutputSettings = null;
            foreach (var ps in Permutations.Enumerate(_phaseSettingValues))
            {
                var output = await circuit.Run(ps, feedbackLoop);
                if (maxOutput == null || output > maxOutput)
                {
                    maxOutput = output;
                    maxOutputSettings = ps.ToArray();
                }
            }
            return (maxOutput.Value, maxOutputSettings!);
        }

        public async ValueTask<long> FindMaxOutput(
            IAsyncEnumerable<string> lines, bool feedbackLoop = false)
        {
            var program = _2.IntComputer.ParseLine(await lines.Single());
            return (await FindBestPhaseSetting(program, feedbackLoop)).Item1;
        }
    }
}
