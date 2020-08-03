using System.Collections;
using System.Collections.Generic;
using Agents;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestDistanceToNearestTiles
    {

        private (Transform t, List<ITile>) GetTestStuff()
        {
            
            GameObject testGameObject = new GameObject();
            testGameObject.transform.position = new Vector3(0,0,0);
            List<ITile> tiles = new List<ITile>()
            {
                new EnvTile(new Vector3(0,0,1), (1,1) ),
                new EnvTile(new Vector3(0,0,2), (1,1) ),
                new EnvTile(new Vector3(0,0,3), (1,1) ),
                new EnvTile(new Vector3(0,0,4), (1,1) ),
                new EnvTile(new Vector3(0,0,5), (1,1) ),
                new EnvTile(new Vector3(0,0,6), (1,1) ),
                new EnvTile(new Vector3(0,0,7), (1,1) ),
            };
            return (testGameObject.transform, tiles);
        }
        
        
        [Test]
        public void TestNearestTileCount()
        {
            (Transform t, List<ITile> envTiles) = GetTestStuff();

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
            (Transform t, List<ITile> envTiles) = GetTestStuff();

            var nearestTile = t.GetNearestTile(1, envTiles, x => true);
            
            Assert.AreEqual(new Vector3(0,0, 1), nearestTile[0].Position);
        }
        
        [Test]
        public void Test_Three_Nearest_Tile()
        {
            (Transform t, List<ITile> envTiles) = GetTestStuff();

            var nearestTile = t.GetNearestTile(3, envTiles, x => true);
            
            Assert.AreEqual(new Vector3(0,0, 1), nearestTile[0].Position);
            Assert.AreEqual(new Vector3(0,0, 2), nearestTile[1].Position);
            Assert.AreEqual(new Vector3(0,0, 3), nearestTile[2].Position);
        }
        
        [Test]
        public void Test_Three_Nearest_Tile_Not_First()
        {
            (Transform t, List<ITile> envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(3, envTiles, x => x.distance > 1);

            Assert.AreEqual(new Vector3(0,0, 2), nearestTile[0].Position);
            Assert.AreEqual(new Vector3(0,0, 3), nearestTile[1].Position);
            Assert.AreEqual(new Vector3(0,0, 4), nearestTile[2].Position);
        }

        [Test]
        public void Test_Count_Exceed_Max()
        {
            (Transform t, List<ITile> envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(8, envTiles, x => true);

            Assert.AreEqual(7, nearestTile.Count);
        }
        
        [Test]
        public void Test_Zero_Tiles()
        {
            (Transform t, List<ITile> envTiles) = GetTestStuff();
            var nearestTile = t.GetNearestTile(0, envTiles, x => true);
            Assert.AreEqual(0, nearestTile.Count);
        }
        
        
    }
}
