using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace advent._2019._4
{
    public readonly struct PasswordFilter
    {
        public bool MeetsAll(ReadOnlySpan<int> digits)
        {
            return SixDigits(digits) &&
                AtLeastOneRepeat(digits) &&
                NoDecrease(digits);
        }

        public int CountValidPasswords(int min, int max)
        {
            var digits = Base10Digits(min).AsSpan();
            var maxDigits = Base10Digits(max).AsSpan();
            int count = 0;
            do
            {
                if (MeetsAll(digits)) { count++; }
                digits = Increment(digits);
            } while (!digits.SequenceEqual(maxDigits));
            return count;
        }

        public static bool SixDigits(ReadOnlySpan<int> digits) =>
            digits.Length == 6;

        public static bool AtLeastOneRepeat(ReadOnlySpan<int> digits)
        {
            for (int i = 0; i<digits.Length-1; i++)
            {
                if (digits[i] == digits[i+1])
                {
                    return true;
                }
            }
            return false;
        }

        public static bool NoDecrease(ReadOnlySpan<int> digits)
        {
            for (int i= 0; i < digits.Length - 1; i++)
            {
                if (digits[i] > digits[i+1])
                {
                    return false;
                }
            }
            return true;
        }

        public static int[] Base10Digits(int value)
        {
            if (value < 100000 || value > 999999) { throw new ArgumentOutOfRangeException(); };
            int[] digits = new int[6];
            for (int i=0; i<6; i++)
            {
                int base10 = 1;
                for (int b = 5; b > i; b--) { base10 *= 10; }
                int digit = value / base10;
                digits[i] = digit;
                value -= digit * base10;
            }
            return digits;
        }

        public static Span<int> Increment(Span<int> digits)
        {
            for (int i = digits.Length-1; i>=0; i--)
            {
                var d = ++digits[i];
                if (d != 10) { break; }
                digits[i] = 0;
            }
            return digits;
        }
    }
}
