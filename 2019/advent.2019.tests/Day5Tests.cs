using advent._2019._2;
using System;
using System.Collections.Generic;
using System.Text;
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

        [Fact]
        public void OpCode_Add()
        {
            var op = new OpCode(10001);
            Assert.Equal(Operation.Add, op.Op);
            Assert.Equal(3, op.Params.Length);
            Assert.Equal(ParamMode.Immediate, op.Params[0]);
            Assert.Equal(ParamMode.Position, op.Params[1]);
            Assert.Equal(ParamMode.Position, op.Params[2]);
        }

        [Fact]
        public void OpCode_InvalidImmediate()
        {
            Assert.Throws<ArgumentException>(() => new OpCode(101));
        }
    }
}
