using System;
using System.Linq;

namespace PoorBigInt
{
    public partial class BigInt : IComparable<BigInt>
    {
        public int CompareTo(BigInt other)
        {
            return Compare(this, other);
        }

        public static int Compare(BigInt left, BigInt right)
        {
            if (!left._isNegative && right._isNegative) return 1;
            if (left._isNegative && !right._isNegative) return -1;
            var factor = right._isNegative ? -1 : 1;
            foreach (var (dl, dr) in new DigitsEnumerable(left._digits, right._digits).Reverse())
            {
                if (dl != dr)
                {
                    return dl.CompareTo(dr) * factor;
                }
            }

            return 0;
        }

        public static BigInt Min(BigInt left, BigInt right)
        {
            if (Compare(left, right) > 0)
            {
                return right;
            }
            return left;
        }

        public static BigInt Max(BigInt left, BigInt right)
        {
            if (Compare(left, right) > 0)
            {
                return left;
            }
            return right;
        }

        private static (BigInt, BigInt) SortAbs(BigInt a, BigInt b)
        {
            var aAbs = a.Abs();
            var bAbs = b.Abs();
            if (Compare(aAbs, bAbs) > 0)
            {
                return (b, a);
            }
            return (a, b);
        }
    }
}
