using advent._2019._7;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day7Tests
    {
        [Theory]
        [InlineData(43210, "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", new[] { 4, 3, 2, 1, 0 })]
        [InlineData(54321, "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", new[] { 0, 1, 2, 3, 4 })]
        [InlineData(65210, "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", new[] { 1, 0, 4, 3, 2 })]
        public async Task BestPhaseSettings(int maxOutput, string program, int[] phaseSettings)
        {
            var amps = new Amplifiers(phaseSettings.Length);
            var prog = _2.IntComputer.ParseLine(program);
            var (output, ps) = await amps.FindBestPhaseSetting(prog);
            Assert.Equal(maxOutput, output);
            Assert.Equal(phaseSettings.Length, ps.Length);
            for (int i = 0; i < ps.Length; i++)
            {
                Assert.Equal(phaseSettings[i], ps[i]);
            }
        }

        [Theory]
        [InlineData("input_7_1", 34852)]
        public async Task Day_7_1(string inputFile, int answer)
        {
            var lines = LineReader.Open(inputFile);
            var x = await new Amplifiers(5).FindMaxOutput(lines);
            Assert.Equal(answer, x);
        }
    }
}
