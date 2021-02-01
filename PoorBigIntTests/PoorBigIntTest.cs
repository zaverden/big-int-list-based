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
    }
}
