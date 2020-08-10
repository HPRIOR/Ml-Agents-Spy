using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class TestFlattenTuples
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestFlattenTuplesSimplePasses()
        {
            var tupleList = new List<(int, int)> {(1, 1), (2, 2), (3, 3), (4, 4)};

            Assert.AreEqual(tupleList.FlattenTuples(), new List<int> {1, 1, 2, 2, 3, 3, 4, 4});
        }
    }
}