using advent._2019._8;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day8Tests
    {
        [Fact]
        public void Parse()
        {
            var img = new SpaceImage("123456789012", 3, 2);
            Assert.Equal(2, img.Layers.Length);
            Assert.Equal(1, img.Layers[0][0, 0]);
            Assert.Equal(6, img.Layers[0][1, 2]);
            Assert.Equal(9, img.Layers[1][0, 2]);
            Assert.Equal(0, img.Layers[1][1, 0]);
        }

        [Theory]
        [InlineData("123456789012", 1)]
        public void IntegrityCheck(string pixels, int answer)
        {
            var img = new SpaceImage(pixels, 3, 2);
            int x = img.IntegrityCheck();
            Assert.Equal(answer, x);
        }

        [Theory]
        [InlineData("input_8_1", 25, 6, 1820)]
        public async Task Day_8_1(string inputFile, int w, int h,int answer)
        {
            var lines = LineReader.Open(inputFile);
            var img = await SpaceImage.Parse(lines, w, h);
            var x = img.IntegrityCheck();
            Assert.Equal(answer, x);
        }
    }
}
