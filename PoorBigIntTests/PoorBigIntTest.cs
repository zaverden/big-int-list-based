using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorBigInt;

namespace PoorBigIntTests
{
    [TestClass]
    public class PoorBigIntTest
    {
        [TestMethod]
        public void ConvertsFromPositiveLong()
        {
            var v = long.MaxValue;
            var bi = BigInt.From(v);
            Assert.AreEqual(v.ToString(), bi.ToString());
        }

        [TestMethod]
        public void ConvertsFromNegativeLong()
        {
            var v = long.MinValue;
            var bi = BigInt.From(v);
            Assert.AreEqual(v.ToString(), bi.ToString());
        }

        [TestMethod]
        public void ConvertsFromZore()
        {
            var bi = BigInt.From(0);
            Assert.AreEqual("0", bi.ToString());
        }

        [TestMethod]
        public void NegatesPositive()
        {
            var bi = BigInt.From(1234567890);
            var nbi = bi.Negate();
            var biStr = bi.ToString();
            var nbiStr = nbi.ToString();
            Assert.AreEqual('-', nbiStr[0]);
            Assert.AreEqual(biStr, nbiStr.Substring(1));
        }

        [TestMethod]
        public void NegatesNegative()
        {
            var nbi = BigInt.From(-1234567890);
            var bi = nbi.Negate();
            var biStr = bi.ToString();
            var nbiStr = nbi.ToString();
            Assert.AreNotEqual('-', biStr[0]);
            Assert.AreEqual(biStr, nbiStr.Substring(1));
        }

        [TestMethod]
        public void EqualsSame()
        {
            var bi1 = BigInt.From(long.MaxValue);
            var bi2 = BigInt.From(long.MaxValue);
            Assert.AreNotSame(bi1, bi2);
            Assert.AreEqual(bi1, bi2);
        }

        [TestMethod]
        public void NotEqualsNegation()
        {
            var bi1 = BigInt.From(long.MaxValue);
            var bi2 = bi1.Negate();
            Assert.AreNotSame(bi1, bi2);
            Assert.AreNotEqual(bi1, bi2);
        }

        [TestMethod]
        public void EqualsDoubleNegation()
        {
            var bi1 = BigInt.From(long.MaxValue);
            var bi2 = bi1.Negate().Negate();
            Assert.AreNotSame(bi1, bi2);
            Assert.AreEqual(bi1, bi2);
        }

        [TestMethod]
        public void AddsTwoPositive()
        {
            var bi1 = BigInt.From(111111);
            var bi2 = BigInt.From(222222);
            Assert.AreEqual(BigInt.From(333333), BigInt.Add(bi1, bi2));
        }

        [TestMethod]
        public void AddsTwoNegative()
        {
            var bi1 = BigInt.From(-111111);
            var bi2 = BigInt.From(-222222);
            Assert.AreEqual(BigInt.From(-333333), BigInt.Add(bi1, bi2));
        }

        [TestMethod]
        public void AddsTwoPositiveWithExtraDigit()
        {
            var bi1 = BigInt.From(999999);
            var bi2 = BigInt.From(1);
            Assert.AreEqual(BigInt.From(1000000), BigInt.Add(bi1, bi2));
        }

        [TestMethod]
        public void SubstructsSmallerFromBigger()
        {
            var bi1 = BigInt.From(222222);
            var bi2 = BigInt.From(111111);
            Assert.AreEqual(BigInt.From(111111), BigInt.Subscruct(bi1, bi2));
        }

        [TestMethod]
        public void SubstructsBiggerFromSmaller()
        {
            var bi1 = BigInt.From(111111);
            var bi2 = BigInt.From(222222);
            Assert.AreEqual(BigInt.From(-111111), BigInt.Subscruct(bi1, bi2));
        }

        [TestMethod]
        public void AddsNumberAndNegation()
        {
            var bi1 = BigInt.From(222222);
            Assert.AreEqual(BigInt.Zero, BigInt.Add(bi1, bi1.Negate()));
        }

        [TestMethod]
        public void ComparesDifferentSigns()
        {
            var bi1 = BigInt.From(222222);
            var bi2 = BigInt.From(-222222);
            Assert.AreEqual(1, bi1.CompareTo(bi2));
            Assert.AreEqual(-1, bi2.CompareTo(bi1));
        }

        [TestMethod]
        public void ComparesPositive()
        {
            var bi1 = BigInt.From(222222);
            var bi2 = BigInt.From(111111);
            Assert.AreEqual(1, BigInt.Compare(bi1, bi2));
            Assert.AreEqual(-1, BigInt.Compare(bi2, bi1));
        }

        [TestMethod]
        public void ComparesNegative()
        {
            var bi1 = BigInt.From(-111111);
            var bi2 = BigInt.From(-222222);
            Assert.AreEqual(1, BigInt.Compare(bi1, bi2));
            Assert.AreEqual(-1, BigInt.Compare(bi2, bi1));
        }

        [TestMethod]
        public void GetsMin()
        {
            var bi1 = BigInt.From(-111111);
            var bi2 = BigInt.From(-222222);
            Assert.AreSame(bi2, BigInt.Min(bi1, bi2));
        }

        [TestMethod]
        public void GetsMax()
        {
            var bi1 = BigInt.From(-111111);
            var bi2 = BigInt.From(-222222);
            Assert.AreSame(bi1, BigInt.Max(bi1, bi2));
        }

        [TestMethod]
        public void DontHaveLeadingZeros1()
        {
            var bi1 = BigInt.From(111111);
            Assert.AreEqual("0", BigInt.Subscruct(bi1, bi1).ToString());
        }

        [TestMethod]
        public void DontHaveLeadingZeros2()
        {
            var bi1 = BigInt.From(111111);
            var bi2 = BigInt.From(90000);
            Assert.AreEqual("-21111", BigInt.Subscruct(bi2, bi1).ToString());
        }

        [TestMethod]
        public void MultipliesPositive()
        {
            var v1 = 12345;
            var v2 = 23456;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.From(v1 * v2), BigInt.Multiply(bi1, bi2));
        }

        [TestMethod]
        public void MultipliesNegative()
        {
            var v1 = -12345;
            var v2 = -23456;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.From(v1 * v2), BigInt.Multiply(bi1, bi2));
        }

        [TestMethod]
        public void MultipliesDifferentSigns()
        {
            var v1 = -12345;
            var v2 = 23456;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.From(v1 * v2), BigInt.Multiply(bi1, bi2));
        }

        [TestMethod]
        public void MultipliesCommutativity()
        {
            var v1 = 12345;
            var v2 = 23456;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.Multiply(bi2, bi1), BigInt.Multiply(bi1, bi2));
        }

        [TestMethod]
        public void DividesPositiveWithoutRemainder()
        {
            var v1 = 64746;
            var v2 = 594;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.From(v1 / v2), BigInt.Divide(bi1, bi2));
        }

        [TestMethod]
        public void DividesPositiveWithRemainder()
        {
            var v1 = 64740;
            var v2 = 594;
            var bi1 = BigInt.From(v1);
            var bi2 = BigInt.From(v2);
            Assert.AreEqual(BigInt.From(v1 / v2), BigInt.Divide(bi1, bi2));
        }
    }
}
