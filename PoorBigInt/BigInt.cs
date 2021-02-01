using System;
using System.Collections.Generic;
using System.Linq;

namespace PoorBigInt
{
    public class BigInt
    {
        private const byte BASE = 10;
        private readonly bool _isNegative;
        private readonly IReadOnlyList<byte> _digits;

        private BigInt(IReadOnlyList<byte> digits, bool isNegative = false)
        {
            _digits = digits;
            _isNegative = isNegative;
        }

        public override string ToString()
        {
            var prefix = _isNegative ? "-" : "";
            if (_digits.Count == 0)
            {
                return "0";
            }
            return prefix + String.Join("", _digits.AsEnumerable().Reverse());
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
    }
}
