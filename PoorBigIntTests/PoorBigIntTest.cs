using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorBigInt;
using System;

namespace PoorBigIntTests
{
    [TestClass]
    public class PoorBigIntTest
    {
        [TestMethod]
        public void CorrectlyConvertsFromPositiveLong()
        {
            var v = long.MaxValue;
            var bi = BigInt.From(v);
            Assert.AreEqual(v.ToString(), bi.ToString());
        }
        [TestMethod]
        public void CorrectlyConvertsFromNegativeLong()
        {
            var v = long.MinValue;
            var bi = BigInt.From(v);
            Assert.AreEqual(v.ToString(), bi.ToString());
        }
    }
}
