using System.Collections.Generic;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestDistanceToNearestTiles
    {
        private (Transform t, List<ITile>) GetTestStuff()
        {
            var testGameObject = new GameObject();
            testGameObject.transform.position = new Vector3(0, 0, 0);
            var tiles = new List<ITile>
            {
                new EnvTile(new Vector3(0, 0, 1), (1, 1)),
                new EnvTile(new Vector3(0, 0, 2), (1, 1)),
                new EnvTile(new Vector3(0, 0, 3), (1, 1)),
                new EnvTile(new Vector3(0, 0, 4), (1, 1)),
                new EnvTile(new Vector3(0, 0, 5), (1, 1)),
                new EnvTile(new Vector3(0, 0, 6), (1, 1)),
                new EnvTile(new Vector3(0, 0, 7), (1, 1))
            };
            return (testGameObject.transform, tiles);
        }


        [Test]
        public void TestNearestTileCount()
        {
            var (t, envTiles) = GetTestStuff();

            var nearestTile = t.GetNearestTile(1, envTiles, x => true);
            var nearestTwoTile = t.GetNearestTile(2, envTiles, x => true);
            var nearestFourTile = t.GetNearestTile(4, envTiles, x => true);
            var nearestSevenTile = t.GetNearestTile(7, envTiles, x => true);

            Assert.AreEqual(1, nearestTile.Count);
            Assert.AreEqual(2, nearestTwoTile.Count);
            Assert.AreEqual(4, nearestFourTile.Count);
            Assert.AreEqual(7, nearestSevenTile.Count);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void Test_Nearest_Tile()
        {
            (var t, var envTiles) = GetTestStuff();

            var nearestTile = t.GetNearestTile(1, envTiles, x => true);

            Assert.AreEqual(new Vector3(0, 0, 1), nearestTile[0].Position);
        }

        [Test]
        public void Test_Three_Nearest_Tile()
        {
            (var t, var envTiles) = GetTestStuff();

            var nearestTile = t.GetNearestTile(3, envTiles, x => true);

            Assert.AreEqual(new Vector3(0, 0, 1), nearestTile[0].Position);
            Assert.AreEqual(new Vector3(0, 0, 2), nearestTile[1].Position);
            Assert.AreEqual(new Vector3(0, 0, 3), nearestTile[2].Position);
        }

        [Test]
        public void Test_Three_Nearest_Tile_Not_First()
        {
            (var t, var envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(3, envTiles, x => x.distance > 1);

            Assert.AreEqual(new Vector3(0, 0, 2), nearestTile[0].Position);
            Assert.AreEqual(new Vector3(0, 0, 3), nearestTile[1].Position);
            Assert.AreEqual(new Vector3(0, 0, 4), nearestTile[2].Position);
        }

        [Test]
        public void Test_Count_Exceed_Max()
        {
            (var t, var envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(8, envTiles, x => true);

            Assert.AreEqual(7, nearestTile.Count);
        }

        [Test]
        public void Test_Zero_Tiles()
        {
            (var t, var envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(0, envTiles, x => true);
            Assert.AreEqual(0, nearestTile.Count);
        }

        [Test]
        public void Test_Same_Distance_Multiple_tiles()
        {
            var agentGo = new GameObject();
            agentGo.transform.position = Vector3.zero;
            var envTiles = new List<EnvTile>
            {
                new EnvTile(new Vector3(0, 0, 1), (1, 1)),
                new EnvTile(new Vector3(0, 0, 1), (1, 1)),
                new EnvTile(new Vector3(0, 0, 2), (1, 1)),
                new EnvTile(new Vector3(0, 0, 2), (1, 1)),
                new EnvTile(new Vector3(0, 0, 3), (1, 1)),
                new EnvTile(new Vector3(0, 0, 3), (1, 1)),
                new EnvTile(new Vector3(0, 0, 3), (1, 1))
            };

            var nearestTile = agentGo.transform.GetNearestTile(3, envTiles, x => true);

            Assert.AreEqual(3, nearestTile.Count);

            nearestTile = agentGo.transform.GetNearestTile(7, envTiles, x => true);
            Assert.AreEqual(new Vector3(0, 0, 1), nearestTile[0].Position);
            Assert.AreEqual(new Vector3(0, 0, 1), nearestTile[1].Position);
            Assert.AreEqual(new Vector3(0, 0, 2), nearestTile[2].Position);
            Assert.AreEqual(new Vector3(0, 0, 2), nearestTile[3].Position);
            Assert.AreEqual(new Vector3(0, 0, 3), nearestTile[4].Position);
            Assert.AreEqual(new Vector3(0, 0, 3), nearestTile[5].Position);
            Assert.AreEqual(new Vector3(0, 0, 3), nearestTile[6].Position);
        }
    }
}