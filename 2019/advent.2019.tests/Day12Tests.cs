using advent._2019._12;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day12Tests
    {
        [Theory]
        [InlineData("<x=-1, y=0, z=2>", -1, 0, 2)]
        [InlineData("<x=2, y=-10, z=-7>", 2, -10, -7)]
        public void ParseLine(string line, int x, int y, int z)
        {
            var pos = new Position(line);
            Assert.Equal(x, pos.X);
            Assert.Equal(y, pos.Y);
            Assert.Equal(z, pos.Z);
        }

        [Theory]
        [InlineData(new[] { 2, 1, -3 }, new[] { -3, -2, 1}, 36)]
        [InlineData(new[] { 2, 0, 4 }, new[] { 1, -1, -1 }, 18)]
        public void MoonEnergy(int[] pos, int[] vel, int energy)
        {
            var moon = new Moon(
                new Position(pos[0], pos[1], pos[2]),
                new Velocity(vel[0], vel[1], vel[2]));
            Assert.Equal(energy, moon.Energy());
        }

        private const string example1 = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";

        private const string example2 = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";

        [Theory]
        [InlineData(example1, 10, 179)]
        [InlineData(example2, 100, 1940)]
        public async Task SystemEnergy(string moons, int steps, int energy)
        {
            var map = await new MoonMap().Load(LineReader.Split(moons));
            map.Step(steps);
            Assert.Equal(energy, map.Energy());
        }

        [Fact]
        public void MoveMoon()
        {
            var moon = new Moon(
                new Position(1, 2, 3),
                new Velocity(-2, 0, 3));
            var moved = moon.Move();
            Assert.Equal(-1, moved.Position.X);
            Assert.Equal(2, moved.Position.Y);
            Assert.Equal(6, moved.Position.Z);
            Assert.Equal(moon.Velocity.X, moved.Velocity.X);
            Assert.Equal(moon.Velocity.Y, moved.Velocity.Y);
            Assert.Equal(moon.Velocity.Z, moved.Velocity.Z);
        }

        [Theory]
        [InlineData("input_12_1", 1000, 6490)]
        public async Task Day_12_1(string inputFile, int steps, int answer)
        {
            var lines = LineReader.Open(inputFile);
            var map = await new MoonMap().Load(lines);
            map.Step(steps);
            int x = map.Energy();
            Assert.Equal(answer, x);
        }
    }
}
