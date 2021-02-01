using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PoorBigInt
{
    public partial class BigInt
    {
        private class DigitsEnumerable : IEnumerable<(byte, byte)>
        {
            IReadOnlyList<byte> _d1;
            IReadOnlyList<byte> _d2;

            public DigitsEnumerable(IReadOnlyList<byte> d1, IReadOnlyList<byte> d2)
            {
                _d1 = d1;
                _d2 = d2;
            }
            public IEnumerator<(byte, byte)> GetEnumerator()
            {
                return new DigitsEnumerator(_d1, _d2);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class DigitsEnumerator : IEnumerator<(byte, byte)>
        {
            IReadOnlyList<byte> _d1;
            IReadOnlyList<byte> _d2;
            int _maxLength;
            int _index = -1;

            public DigitsEnumerator(IReadOnlyList<byte> d1, IReadOnlyList<byte> d2)
            {
                _d1 = d1;
                _d2 = d2;
                _maxLength = Math.Max(d1.Count, d2.Count);
            }

            public (byte, byte) Current => _index == -1
                ? default((byte, byte))
                : (GetValue(_d1, _index), GetValue(_d2, _index));

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                /* nothing to do */
            }

            public bool MoveNext()
            {
                if (_index >= _maxLength)
                {
                    return false;
                }
                _index += 1;
                return true;
            }

            public void Reset()
            {
                _index = -1;
            }

            private static byte GetValue(IReadOnlyList<byte> d, int index)
            {
                if (index < d.Count)
                {
                    return d[index];
                }
                return 0;
            }

        }
    }
}
