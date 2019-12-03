using advent._2019._3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day3Tests
    {
        [Fact]
        public void ParseWire()
        {
            const string input = "R8,U5,L5,D3";
            var segments = Wire.ParseSegments(input);
            Assert.Equal(4, segments.Length);
            Assert.Equal((8, 0), segments[0]);
            Assert.Equal((0, 5), segments[1]);
            Assert.Equal((-5, 0), segments[2]);
            Assert.Equal((0, -3), segments[3]);
        }

        [Fact]
        public void FindIntersections()
        {
            var wire1 = new Wire("R8,U5,L5,D3");
            var wire2 = new Wire("U7,R6,D4,L4");
            var intersections = new WiredPanel()
                .FindIntersections(wire1, wire2)
                .ToArray();
            Assert.Equal(2, intersections.Length);
            Assert.Contains((3, 3), intersections);
            Assert.Contains((6, 5), intersections);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        public void FindMinIntersectionDistance(
            string w1, string w2, int answer)
        {
            int x = new WiredPanel()
                .FindMinIntersectionDistance(
                    new Wire(w1), new Wire(w2));
            Assert.Equal(answer, x);
        }

        [Theory]
        [InlineData("input_3_1", 8015)]
        public async Task Day_3_1(string inputFile, int answer)
        {
            var lines = LineReader.Open(inputFile);
            int x = await new WiredPanel().FindIntersectionDistance(lines);
            Assert.Equal(answer, x);
        }
    }
}
