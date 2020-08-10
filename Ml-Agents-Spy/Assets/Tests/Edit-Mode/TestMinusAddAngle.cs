using Enums;
using NUnit.Framework;
using static StaticFunctions;

namespace Tests
{
    public class TestMinusAddAngle
    {
        // subtraction

        [Test]
        public void Test_Minus_To_Zero()
        {
            Assert.AreEqual(0, MinusAddAngle(10, AddSub.Sub, 10));
        }

        [Test]
        public void Test_Minus_To_Minus_One()
        {
            Assert.AreEqual(359, MinusAddAngle(10, AddSub.Sub, 11));
        }

        [Test]
        public void Test_Minus_360()
        {
            Assert.AreEqual(0, MinusAddAngle(0, AddSub.Sub, 360));
            Assert.AreEqual(180, MinusAddAngle(180, AddSub.Sub, 360));
        }

        [Test]
        public void Test_Minus_180()
        {
            Assert.AreEqual(0, MinusAddAngle(180, AddSub.Sub, 180));
        }


        [Test]
        public void Test_Normal_Sub()
        {
            Assert.AreEqual(20, MinusAddAngle(30, AddSub.Sub, 10));
        }

        // addition

        [Test]
        public void Test_Add_To_360()
        {
            Assert.AreEqual(0, MinusAddAngle(350, AddSub.Add, 10));
        }

        [Test]
        public void Test_Add_To_Plus_One()
        {
            Assert.AreEqual(1, MinusAddAngle(11, AddSub.Add, 350));
        }

        [Test]
        public void Test_Add_360()
        {
            Assert.AreEqual(0, MinusAddAngle(0, AddSub.Add, 360));
            Assert.AreEqual(180, MinusAddAngle(180, AddSub.Add, 360));
        }

        [Test]
        public void Test_Add_180()
        {
            Assert.AreEqual(0, MinusAddAngle(180, AddSub.Add, 180));
        }

        [Test]
        public void Test_Normal_Add()
        {
            Assert.AreEqual(20, MinusAddAngle(10, AddSub.Add, 10));
        }
    }
}