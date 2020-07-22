﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;



namespace Tests
{
    public class TileTest
    {
         ///A Test behaves as an ordinary method
        [Test]
        public void TileTestSimplePasses()
        {
            EnvTile t = new EnvTile(new Vector3(0, 0, 0), (0, 0));
            Assert.AreEqual(false, t.HasEnv);
            Assert.AreEqual(false, t.HasGuard);
            Assert.AreEqual(false, t.HasSpy);
            Assert.AreEqual(false, t.IsExit);
            Assert.AreEqual(false, t.OnPath);
        }
    }
}
