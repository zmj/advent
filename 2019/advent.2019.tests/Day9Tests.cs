using advent._2019._2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day9Tests
    {
        [Fact]
        public async Task RelativeBase()
        {
            var program = new[] 
            {
                109L, 1, 204, -1, 1001, 100, 1, 100,
                1008, 100, 16, 101, 1006, 101, 0, 99 
            };
            var (input, output) = IO();
            var computer = new IntComputer(input.Reader, output.Writer);
            await computer.RunProgram(program);
            int i = 0;
            await foreach (var x in output.Reader.ReadAllAsync())
            {
                Assert.Equal(program[i++], x);
            }
        }

        private (Channel<long>, Channel<long>) IO() =>
            (Channel.CreateUnbounded<long>(), Channel.CreateUnbounded<long>());

        [Theory]
        [InlineData(34915192L * 34915192, new[] { 1102L, 34915192, 34915192, 7, 4, 7, 99, 0 })]
        [InlineData(1125899906842624, new[] { 104, 1125899906842624, 99 })]
        public async Task LargeNumber(long answer, long[] program)
        {
            var (input, output) = IO();
            var computer = new IntComputer(input.Reader, output.Writer);
            await computer.RunProgram(program);
            var x = await output.Reader.ReadAsync();
            Assert.Equal(answer, x);
        }

        [Fact]
        public async Task GrowMemory()
        {
            var program = new[] { 3L, 1000, 4, 1000, 99 };
            var (input, output) = IO();
            var computer = new IntComputer(input.Reader, output.Writer);
            input.Writer.MustWrite(9);
            await computer.RunProgram(program);
            var x = output.Reader.MustRead();
            Assert.Equal(9, x);
        }

        [Theory]
        [InlineData("input_9_1", 3409270027)]
        public async Task Day_9_1(string inputFile, long answer)
        {
            var (input, output) = IO();
            var computer = new IntComputer(input.Reader, output.Writer);
            input.Writer.MustWrite(1);
            await computer.ParseAndRun(LineReader.Open(inputFile));
            var x = output.Reader.MustRead();
            Assert.Equal(answer, x);
        }
    }
}
