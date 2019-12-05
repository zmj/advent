using advent._2019._2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day2Tests
    {
        [Fact]
        public void Parse()
        {
            const string input = "1,9,10,3,2,3,11,0,99,30,40,50";
            int[] output = new[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 };
            var x = IntComputer.ParseLine(input);
            Assert.Equal(output.Length, x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                Assert.Equal(output[i], x[i]);
            }
        }

        [Fact]
        public void Add()
        {
            var proc = new Process(new[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 });
            bool ok = new IntComputer().ExecuteInstruction(proc);
            Assert.True(ok);
            Assert.Equal(70, proc.Data[3]);
        }

        [Fact]
        public void Multiply()
        {
            var proc = new Process(new[] { 1, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, ip: 4);
            bool ok = new IntComputer().ExecuteInstruction(proc);
            Assert.True(ok);
            Assert.Equal(3500, proc.Data[0]);
        }

        [Fact]
        public void Done()
        {
            var proc = new Process(new[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, ip: 8);
            bool ok = new IntComputer().ExecuteInstruction(proc);
            Assert.False(ok);
        }

        [Theory]
        [InlineData(new[] { 1, 0, 0, 0, 99 }, new[] { 2, 0, 0, 0, 99 })]
        [InlineData(new[] { 2, 3, 0, 3, 99 }, new[] { 2, 3, 0, 6, 99 })]
        [InlineData(new[] { 2, 4, 4, 5, 99, 0 }, new[] { 2, 4, 4, 5, 99, 9801 })]
        [InlineData(new[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, new[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void RunToCompletion(int[] start, int[] done)
        {
            var x = new IntComputer().RunProgram(start);
            Assert.Equal(done.Length, x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                Assert.Equal(done[i], x[i]);
            }
        }

        [Theory]
        [InlineData("input_2_1", 2890696)]
        public async Task Day_2_1(string inputFile, int answer)
        {
            var lines = LineReader.Open(inputFile);
            var x = await new IntComputer().ParseFixAndRun(lines, 12, 02);
            Assert.Equal(answer, x);
        }

        [Theory]
        [InlineData("input_2_1", 2890696, 1202)]
        [InlineData("input_2_1", 19690720, 8226)]
        public async Task Day_2_2(string inputFile, int output, int nounAndVerb)
        {
            var lines = LineReader.Open(inputFile);
            var x = await new IntComputer().FindNounAndVerb(lines, output);
            Assert.Equal(nounAndVerb, x);
        }
    }
}
