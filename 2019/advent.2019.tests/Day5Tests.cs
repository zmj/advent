using advent._2019._2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day5Tests
    {
        [Fact]
        public void OpCode_Multiply()
        {
            var op = new OpCode(1002);
            Assert.Equal(Operation.Multiply, op.Op);
            Assert.Equal(3, op.Params.Length);
            Assert.Equal(ParamMode.Position, op.Params[0]);
            Assert.Equal(ParamMode.Immediate, op.Params[1]);
            Assert.Equal(ParamMode.Position, op.Params[2]);
        }

        [Theory]
        [InlineData(101, new[] { ParamMode.Immediate, ParamMode.Position, ParamMode.Position })]
        [InlineData(1101, new[] { ParamMode.Immediate, ParamMode.Immediate, ParamMode.Position })]
        public void OpCode_Add(int opCode, ParamMode[] paramModes)
        {
            var op = new OpCode(opCode);
            Assert.Equal(Operation.Add, op.Op);
            Assert.Equal(paramModes.Length, op.Params.Length);
            for (int i = 0; i < op.Params.Length; i++)
            {
                Assert.Equal(paramModes[i], op.Params[i]);
            }
        }

        [Fact]
        public void OpCode_InvalidImmediate()
        {
            Assert.Throws<ArgumentException>(() => new OpCode(10001));
        }

        [Fact]
        public async Task Input()
        {
            var input = Channel.CreateUnbounded<int>();
            var computer = new IntComputer(input.Reader, Channel.CreateUnbounded<int>().Writer);
            await input.Writer.WriteAsync(1);
            var x = computer.RunProgram(new[] { 3, 0, 99 });
            Assert.Equal(1, x[0]);
        }

        [Fact]
        public async Task Output()
        {
            var output = Channel.CreateUnbounded<int>();
            var computer = new IntComputer(Channel.CreateUnbounded<int>().Reader, output.Writer);
            computer.RunProgram(new[] { 4, 1, 99 });
            var x = await output.Reader.ReadAsync();
            Assert.Equal(1, x);
        }

        [Theory]
        [InlineData("input_5_1", 7988899)]
        public async Task Day_5_1(string inputFile, int answer)
        {
            var (input, output) = (Channel.CreateUnbounded<int>(), Channel.CreateUnbounded<int>());
            var computer = new IntComputer(input.Reader, output.Writer);
            await input.Writer.WriteAsync(1);
            await computer.ParseAndRun(LineReader.Open(inputFile));
            int? code = null;
            await foreach (int x in output.Reader.ReadAllAsync())
            {
                code = x;
            }
            Assert.Equal(answer, code);
        }
    }
}
