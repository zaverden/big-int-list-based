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
            _digits = TrimLeadingZeros(digits);
            _isNegative = _digits.Count != 0 && isNegative;
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

            if (left._isNegative == right._isNegative)
            {
                return new BigInt(AddDigits(new DigitsEnumerable(left._digits, right._digits)), left._isNegative);
            }

            var (min, max) = SortAbs(left, right);

            return new BigInt(SubstructDigits(new DigitsEnumerable(max._digits, min._digits)), max._isNegative);
        }

        public static BigInt Subscruct(BigInt left, BigInt right)
        {
            return Add(left, right.Negate());
        }

        public static BigInt Multiply(BigInt left, BigInt right)
        {
            var isNegative = left._isNegative != right._isNegative;
            var acc = Zero;
            var dlIndex = -1;
            foreach (var dl in left._digits)
            {
                dlIndex += 1;
                var digits = Enumerable.Repeat<byte>(0, dlIndex).ToList();
                byte extra = 0;
                foreach (var dr in right._digits)
                {
                    byte d = (byte)(dl * dr + extra);
                    extra = (byte)(d / BASE);
                    d = (byte)(d % BASE);
                    digits.Add(d);
                }
                if (extra != 0)
                {
                    digits.Add(extra);
                }
                acc = Add(acc, new BigInt(digits));
            }
            return isNegative ? acc.Negate() : acc;
        }

        public static BigInt Divide(BigInt left, BigInt right)
        {
            var isNegative = left._isNegative != right._isNegative;
            var resultDigits = new List<byte>();
            var position = left._digits.Count - right._digits.Count;
            var tail = left.Abs();
            for (int i = position; i >= 0; i--)
            {
                var part = new BigInt(Enumerable.Repeat<byte>(0, i).Concat(right._digits).ToArray());
                byte n = 0;
                while (Compare(tail, part) >= 0)
                {
                    n += 1;
                    tail = Subscruct(tail, part);
                }
                resultDigits.Add(n);
            }
            resultDigits.Reverse();
            return new BigInt(resultDigits, isNegative);
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
                int d = d1 - d2 - extra;
                if (d < 0)
                {
                    extra = 1;
                    d += BASE;
                }
                else
                {
                    extra = 0;
                }
                digits.Add((byte)d);
            }
            return digits;
        }

        private static IReadOnlyList<byte> TrimLeadingZeros(IReadOnlyList<byte> digits)
        {
            var list = new List<byte>(digits);
            var lastNonZeroIndex = list.FindLastIndex(d => d != 0);
            return list.Take(lastNonZeroIndex + 1).ToArray();
        }
    }
}
