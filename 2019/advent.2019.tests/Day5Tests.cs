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
            var (input, output) = Channels();
            var computer = new IntComputer(input.Reader, output.Writer);
            await input.Writer.WriteAsync(1);
            var x = await computer.RunProgram(new[] { 3, 0, 99 });
            Assert.Equal(1, x[0]);
        }

        [Fact]
        public async Task Output()
        {
            var (input, output) = Channels();
            var computer = new IntComputer(input.Reader, output.Writer);
            await computer.RunProgram(new[] { 4, 1, 99 });
            var x = await output.Reader.ReadAsync();
            Assert.Equal(1, x);
        }

        [Theory]
        [InlineData("input_5_1", 7988899)]
        public async Task Day_5_1(string inputFile, int answer)
        {
            var (input, output) = Channels();
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

        [Fact]
        public async Task JumpIfTrueAsync()
        {
            var program = new[] { 1105, 1, 7, 1, 0, 0, 0, 99 };
            var x = await new IntComputer().RunProgram(program);
            Assert.Equal(1105, x[0]);
        }

        [Fact]
        public async Task JumpIfFalseAsync()
        {
            var program = new[] { 1106, 0, 7, 1, 0, 0, 0, 99 };
            var x = await new IntComputer().RunProgram(program);
            Assert.Equal(1106, x[0]);
        }

        [Theory]
        [InlineData(3, 4, true)]
        [InlineData(4, 4, false)]
        [InlineData(5, 4, false)]
        public async Task LessThanAsync(int op1, int op2, bool lt)
        {
            var program = new[] { 1107, op1, op2, 0, 99 };
            var x = await new IntComputer().RunProgram(program);
            Assert.Equal(lt ? 1 : 0, x[0]);
        }

        [Theory]
        [InlineData(3, 4, false)]
        [InlineData(4, 4, true)]
        [InlineData(5, 4, false)]
        public async Task EqualAsync(int op1, int op2, bool eq)
        {
            var program = new[] { 1108, op1, op2, 0, 99 };
            var x = await new IntComputer().RunProgram(program);
            Assert.Equal(eq ? 1 : 0, x[0]);
        }

        [Theory]
        [InlineData(new[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, 1)]
        [InlineData(new[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, 0)]
        [InlineData(new[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, 1)]
        [InlineData(new[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, 0)]
        [InlineData(new[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, 1)]
        [InlineData(new[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, 1)]
        [InlineData(new[] { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 }, 1000)]
        public async Task InputOutput(int[] program, int answer)
        {
            var (input, output) = Channels();
            var computer = new IntComputer(input.Reader, output.Writer);
            await input.Writer.WriteAsync(8);
            await computer.RunProgram(program);
            int x = await output.Reader.ReadAsync();
            Assert.Equal(answer, x);
        }

        private (Channel<int>, Channel<int>) Channels() =>
            (Channel.CreateUnbounded<int>(), Channel.CreateUnbounded<int>());

        [Theory]
        [InlineData("input_5_1", 13758663)]
        public async Task Day_5_2(string inputFile, int answer)
        {
            var (input, output) = Channels();
            var computer = new IntComputer(input.Reader, output.Writer);
            await input.Writer.WriteAsync(5);
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
