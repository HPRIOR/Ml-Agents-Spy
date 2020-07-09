using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestSimplePasses()
        {
            Tile tile = new Tile(new Vector3(0 ,0,0), (0,0));
            Assert.AreEqual(false, tile.HasEnv);
        }

       
    }
}
