using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;
using static StaticFunctions;

namespace Tests
{
    public class TestAgentTileConverter
    {
        Dictionary<ParentObject, GameObject> _dictionary =
            new Dictionary<ParentObject, GameObject>()
            {
                {ParentObject.TopParent,  new GameObject()},
                {ParentObject.EnvParent, new GameObject()},
                {ParentObject.SpyParent, new GameObject()},
                {ParentObject.GuardParent, new GameObject()},
                {ParentObject.DebugParent, new GameObject()}

            };
        
        
        private IEnvSetup Env => new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: _dictionary,
            hasMiddleTiles: true
            );


        IAgentTileConverter GetAgentTiles()
        {
            var envDict = Env.GetTileTypes();
            List<IEnvTile> envTileList = new List<IEnvTile>();
            envDict[TileType.FreeTiles].ForEach(tile => envTileList.Add(tile));
            envDict[TileType.SpyTile].ForEach(tile => envTileList.Add(tile));
            envDict[TileType.GuardTiles].ForEach(tile => envTileList.Add(tile));

            return new AgentTileConverter(envTileList, new FindAdjacentAgentTile());
        }
        
        [Test]
        public void TestAgentTileCount()
        {
            IAgentTileConverter agentTileConv = GetAgentTiles();
            Assert.AreEqual(21, agentTileConv.GetAgentTiles().Count);
        }

        [Test]
        public void TestAdjacentTiles()
        {

            IAgentTileConverter agentTileConv = GetAgentTiles();

            IAgentTile middleTile = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 3));
            IAgentTile northFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 4));
            IAgentTile southFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 2));
            IAgentTile eastFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (4, 3));
            IAgentTile westFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (4, 3));

            Assert.AreEqual(northFromMiddle, middleTile.AdjacentTile[Direction.N]);
            Assert.AreEqual(eastFromMiddle, middleTile.AdjacentTile[Direction.E]);
            Assert.AreEqual(southFromMiddle, middleTile.AdjacentTile[Direction.S]);
            Assert.AreEqual(westFromMiddle, middleTile.AdjacentTile[Direction.W]);
        }

        [Test]
        public void TestInitialSpyPlacement()
        {
            //occupied by agent = true in correct place, false everywhere else
        }

        [Test]
        public void TestInitialVisitCount()
        {
            // all equal to zero
        }

        [Test]
        public void TestInitialVisitedByAlgo()
        {
            //Visited by algo false for all
        }
    }
}
