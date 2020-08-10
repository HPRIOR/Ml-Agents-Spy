using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TileLogicTests
    {
        private Dictionary<ParentObject, GameObject> GetDictionary()
        {
            var TopParent = new GameObject();
            var EnvParent = new GameObject();
            var SpyParent = new GameObject();
            var GuardParent = new GameObject();
            var DebugParent = new GameObject();

            return new Dictionary<ParentObject, GameObject>
            {
                {ParentObject.TopParent, TopParent},
                {ParentObject.EnvParent, EnvParent},
                {ParentObject.SpyParent, SpyParent},
                {ParentObject.GuardParent, GuardParent},
                {ParentObject.DebugParent, DebugParent}
            };
        }


        [Test]
        public void TestExitCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                3,
                2,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();

            Assert.AreEqual(3, tileTypes[TileType.ExitTiles].Count);
        }

        [Test]
        public void TestSpyCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                3,
                2,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();


            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();

            Assert.AreEqual(1, tileTypes[TileType.SpyTile].Count);
        }

        [Test]
        public void TestGuardCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                3,
                2,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();

            Assert.AreEqual(2, tileTypes[TileType.GuardTiles].Count);
        }

        [Test]
        public void TestEnvTileCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                3,
                2,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            Assert.AreEqual(245, tileTypes[TileType.EnvTiles].Count);

            tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                5,
                2,
                dict
            );
            // assert change in tiles on increase exit count
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            Assert.AreEqual(243, tileTypes[TileType.EnvTiles].Count);
        }


        [Test]
        public void TestFreeTilesCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                1,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            Assert.AreEqual(19, tileTypes[TileType.FreeTiles].Count);
        }

        [Test]
        public void TestExitLocationsOnYAxis()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                1,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();


            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.ExitTiles].ForEach(tile => Assert.AreEqual(6, tile.Coords.y));


            tileLogicBuilder = new TileLogicBuilder(
                3,
                0,
                4,
                1,
                dict
            );

            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.ExitTiles].ForEach(tile => Assert.AreEqual(16, tile.Coords.y));
        }

        [Test]
        public void TestSpyOnYAxis()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                1,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.SpyTile].ForEach(tile => Assert.AreEqual(1, tile.Coords.y));

            tileLogicBuilder = new TileLogicBuilder(
                3,
                0,
                4,
                1,
                dict
            );

            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.SpyTile].ForEach(tile => Assert.AreEqual(1, tile.Coords.y));
        }

        [Test]
        public void TestGuardGreaterThanOne()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                0,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
        }


        [Test]
        public void TestGuardSpawnArea()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                1,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(5, tile.Coords.y));


            tileLogicBuilder = new TileLogicBuilder(
                2,
                0,
                4,
                1,
                dict
            );

            // 2 map size
            tileLogic = tileLogicBuilder.GetTileLogicSetup();


            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(9, tile.Coords.y));

            tileLogicBuilder = new TileLogicBuilder(
                3,
                0,
                4,
                1,
                dict
            );

            // 3 map size
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(15, tile.Coords.y));

            tileLogicBuilder = new TileLogicBuilder(
                4,
                0,
                10,
                9,
                dict
            );

            // > 3 (4) map size
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.GuardTiles].ForEach(tile =>
                Assert.AreEqual(true, (tile.Coords.y == 19) | (tile.Coords.y == 17) | (tile.Coords.y == 18)));

            tileLogicBuilder = new TileLogicBuilder(
                5,
                0,
                10,
                9,
                dict
            );

            // > 3 (5) map size
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            tileTypes = tileLogic.GetTileTypes();
            tileTypes[TileType.GuardTiles].ForEach(tile =>
                Assert.AreEqual(true, tile.Coords.y == 25 || tile.Coords.y == 24 || tile.Coords.y == 23));
        }

        [Test]
        public void TestGuardCountLessThanExitCount()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                4,
                dict
            );

            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            tileLogic.GetTileLogic();
            var tileTypes = tileLogic.GetTileTypes();
            Assert.Greater(tileTypes[TileType.ExitTiles].Count, tileTypes[TileType.GuardTiles].Count);
            Assert.AreEqual(1, tileTypes[TileType.GuardTiles].Count);
        }

        [Test]
        public void TestThrowErrorIfExitCountBelow2()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                0,
                4,
                dict
            );

            // exit count = 0
            var tileLogic = tileLogicBuilder.GetTileLogicSetup();


            Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());

            tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                1,
                4,
                dict
            );

            // exit count = 1
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());

            tileLogicBuilder = new TileLogicBuilder(
                1,
                0,
                2,
                4,
                dict
            );

            // exit count = 2
            tileLogic = tileLogicBuilder.GetTileLogicSetup();

            Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
        }


        [Test]
        public void TestThrowExceptionIfMapTooComplex()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                100,
                3,
                4,
                dict
            );

            // exit count = 0
            var tileLogic = tileLogicBuilder.GetTileLogicSetup();

            Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
        }
    }
}