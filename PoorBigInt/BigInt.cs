using System;
using System.Collections.Generic;
using System.Linq;

namespace PoorBigInt
{
    public partial class BigInt
    {
        private const byte BASE = 10;
        private readonly bool _isNegative;
        private readonly IReadOnlyList<byte> _digits;

        private BigInt(IReadOnlyList<byte> digits, bool isNegative = false)
        {
            _digits = digits;
            _isNegative = isNegative;
        }

        public static BigInt From(long value)
        {
            var isNegative = value < 0;
            var digits = new List<byte>();
            while (value != 0)
            {
                digits.Add((byte)Math.Abs(value % BASE)); // it is safe to cast because BASE is byte, so mod cannot overflow byte
                value /= BASE;
            }
            return new BigInt(digits, isNegative);
        }

        public static BigInt Zero = new BigInt(new byte[0], false);

        public override string ToString()
        {
            var prefix = _isNegative ? "-" : "";
            if (_digits.Count == 0)
            {
                return "0";
            }
            return prefix + String.Join("", _digits.AsEnumerable().Reverse());
        }

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != typeof(BigInt))
            {
                return false;
            }
            return Equals((BigInt)obj);
        }

        public bool Equals(BigInt bi)
        {
            if (bi == null) return false;
            if (_isNegative != bi._isNegative) return false;

            var allSame = new DigitsEnumerable(_digits, bi._digits)
                .All((p) => p.Item1 == p.Item2);

            return allSame;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public BigInt Negate()
        {
            return new BigInt(_digits, !_isNegative);
        }

        public BigInt Abs()
        {
            return new BigInt(_digits, false);
        }

        public static BigInt Add(BigInt left, BigInt right)
        {
            left = left ?? Zero;
            right = right ?? Zero;
            DigitsEnumerable digitsEnumerable = new DigitsEnumerable(left._digits, right._digits);

            if (left._isNegative == right._isNegative)
            {
                return new BigInt(AddDigits(digitsEnumerable), left._isNegative);
            }

            return new BigInt(SubstructDigits(digitsEnumerable), left._isNegative);
        }

        public static BigInt Subscruct(BigInt left, BigInt right)
        {
            return Add(left, right.Negate());
        }

        private static IReadOnlyList<byte> AddDigits(DigitsEnumerable enumerable)
        {
            var digits = new List<byte>();
            byte extra = 0;
            foreach (var (d1, d2) in enumerable)
            {
                byte d = (byte)(d1 + d2 + extra);
                extra = (byte)(d / BASE);
                d = (byte)(d % BASE);
                digits.Add(d);
            }
            if (extra != 0)
            {
                digits.Add(extra);
            }
            return digits;
        }

        private static IReadOnlyList<byte> SubstructDigits(DigitsEnumerable enumerable)
        {
            var digits = new List<byte>();
            byte extra = 0;
            foreach (var (d1, d2) in enumerable)
            {
                byte d = (byte)(d1 - d2 - extra);
                if (d < 0)
                {
                    extra = 1;
                    d += BASE;
                } else
                {
                    extra = 0;
                }
                digits.Add(d);
            }
            return digits;
        }
    }
}
