using advent._2019._6;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day6Tests
    {
        [Theory]
        [InlineData("COM)B", "COM", "B")]
        [InlineData("B)C", "B", "C")]
        public void ParseOrbit(string line, string p, string c)
        {
            var (parent, child) = OrbitMap.ParseOrbit(line);
            Assert.Equal(p, parent);
            Assert.Equal(c, child);
        }

        private const string example = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";

        [Theory]
        [InlineData(example, "D", 3)]
        [InlineData(example, "L", 7)]
        [InlineData(example, "COM", 0)]
        public async Task ObjectOrbits(string orbits, string c, int n)
        {
            var map = await new OrbitMap().Load(LineReader.Split(orbits));
            int x = map.TotalOrbits(c);
            Assert.Equal(n, x);
        }

        [Theory]
        [InlineData(example, 42)]
        public async Task TotalOrbits(string orbits, int answer)
        {
            var map = await new OrbitMap().Load(LineReader.Split(orbits));
            int x = map.TotalOrbits();
            Assert.Equal(answer, x);
        }

        [Theory]
        [InlineData("input_6_1", 358244)]
        public async Task Day_6_1(string inputFile, int answer)
        {
            var map = await new OrbitMap().Load(LineReader.Open(inputFile));
            int x = map.TotalOrbits();
            Assert.Equal(answer, x);
        }

        private const string example2 = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";

        [Theory]
        [InlineData(example2, "YOU", "D", 3)]
        [InlineData(example2, "SAN", "D", 1)]
        public async Task DistanceUp(string orbits, string src, string dst, int n)
        {
            var map = await new OrbitMap().Load(LineReader.Split(orbits));
            int x = map.DistanceUp(src, dst);
            Assert.Equal(n, x);
        }

        [Theory]
        [InlineData(example2, "YOU", "SAN", 4)]
        public async Task Distance(string orbits, string src, string dst, int n)
        {
            var map = await new OrbitMap().Load(LineReader.Split(orbits));
            int x = map.Distance(src, dst);
            Assert.Equal(n, x);
        }

        [Theory]
        [InlineData("input_6_1", 517)]
        public async Task Day_6_2(string inputFile, int answer)
        {
            var map = await new OrbitMap().Load(LineReader.Open(inputFile));
            int x = map.Distance("YOU", "SAN");
            Assert.Equal(answer, x);
        }
    }
}
