using System;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
    [TestFixture]
    public class BitListTest
    {
        [Test]
        public void Count_is_0_after_costruction()
        {
            BitList target = new BitList();
            Assert.AreEqual(0, target.Count);
        }

        [Test]
        public void Insert_1_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

        [Test]
        public void Insert_0_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

        private const bool O = false;
        private const bool l = true;

        [TestCase(new bool[] { }, 0, 0, new bool[] { }, TestName = "[].Append([])=[]")]
        [TestCase(new bool[] { }, 1, 1, new[] { l }, TestName = "[].Append([1])=[1]")]
        [TestCase(new bool[] { }, 0, 1, new[] { O }, TestName = "[].Append([0])=[1]")]
        [TestCase(new[] { l }, 1, 1, new[] { l, l }, TestName = "[1].Append([1])=[1,1]")]
        [TestCase(new[] { l }, 0, 1, new[] { l, O }, TestName = "[1].Append([0])=[1,0]")]
        [TestCase(new[] { O }, 1, 1, new[] { O, l }, TestName = "[0].Append([1])=[0,1]")]
        [TestCase(new[] { O }, 0, 1, new[] { O, O }, TestName = "[0].Append([0])=[0,0]")]
        [TestCase(new[] { O, l, l, O }, 25, 6, new[] { O, l, l, O, l, l, O, l, O, O }, TestName = "[0110].Append([110100;6])=[0110110100]")] //  25 = 110100
        [TestCase(new[] { O, l, l, O }, 25, 3, new[] { O, l, l, O, l, O, O }, TestName = "[0110].Append([110100;6])=[0110110100]")] //  25 = 110100
        [TestCase(new[] { O }, 0, -11, new[] { O, O }, ExpectedException = typeof(ArgumentOutOfRangeException), TestName = "[0].Append([0;-1])=[0,0] => Exception")]
        [TestCase(new[] { O }, 0, 33, new[] { O, O }, ExpectedException = typeof(ArgumentOutOfRangeException), TestName = "[0].Append([0;33])=[0,0] => Exception")]
        public void Add(bool[] initial, int value, int bitCount, bool[] expected)
        {
            var target = new BitList();
            target.Add(initial);
            target.Add(value, bitCount);
            CollectionAssert.AreEquivalent(expected, target);
        }
    }
}
