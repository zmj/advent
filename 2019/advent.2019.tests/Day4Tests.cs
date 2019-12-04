using advent._2019._4;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace advent._2019.tests
{
    public class Day4Tests
    {
        [Theory]
        [InlineData(138307, new[] { 1, 3, 8, 3, 0, 7 })]
        [InlineData(654504, new[] { 6, 5, 4, 5, 0, 4 })]
        public void Base10Digits(int value, int[] digits)
        {
            var x = PasswordFilter.Base10Digits(value);
            Assert.Equal(digits.Length, x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                Assert.Equal(digits[i], x[i]);
            }
        }

        [Theory]
        [InlineData(new[] { 1, 3, 8, 3, 0, 7 }, new[] { 1, 3, 8, 3, 0, 8 })]
        [InlineData(new[] { 1, 9, 9, 9, 9, 9 }, new[] { 2, 0, 0, 0, 0, 0 })]
        public void Increment(int[] oldDigits, int[] newDigits)
        {
            var x = PasswordFilter.Increment(oldDigits.AsSpan());
            Assert.Equal(newDigits.Length, x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                Assert.Equal(newDigits[i], x[i]);
            }
        }

        [Theory]
        [InlineData(223450, false)]
        [InlineData(111111, true)]
        public void NoDecrease(int value, bool ok)
        {
            var digits = PasswordFilter.Base10Digits(value);
            Assert.Equal(ok, PasswordFilter.NoDecrease(digits));
        }

        [Theory]
        [InlineData(123789, false)]
        [InlineData(111111, true)]
        public void AtLeastOneRepeat(int value, bool ok)
        {
            var digits = PasswordFilter.Base10Digits(value);
            Assert.Equal(ok, PasswordFilter.AtLeastOneRepeat(digits));
        }

        [Theory]
        [InlineData(111111, true)]
        [InlineData(223450, false)]
        [InlineData(123789, false)]
        public void MeetsAll(int value, bool ok)
        {
            var digits = PasswordFilter.Base10Digits(value);
            Assert.Equal(ok, new PasswordFilter().MeetsAll(digits));
        }

        [Theory]
        [InlineData(138307, 654504, 1855)]
        public void Day_4_1(int min, int max, int answer)
        {
            int x = new PasswordFilter().CountValidPasswords(min, max);
            Assert.Equal(answer, x);
        }
    }
}
