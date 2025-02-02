﻿using System.Linq;
using EnvSetup;
using NUnit.Framework;
using UnityEngine;
using static StaticFunctions;


namespace Tests
{
    public class PathFinderTest
    {
        private readonly PathFinder p = new PathFinder();

        private readonly TileMatrixProducer tmOne =
            new TileMatrixProducer(new Vector3(0, 0, 0), MapScaleToMatrixSize(1));

        private void TestSetUp()
        {
            (from EnvTile tile in tmOne.Tiles
                where tile.Coords.x == 0
                      || tile.Coords.x == 6
                      || tile.Coords.y == 0
                      || tile.Coords.y == 6
                select tile).ToList().ForEach(tile => tile.HasEnv = true);
        }


        [Test]
        public void tmOnePathTest()
        {
            TestSetUp();
            p.GetPath(tmOne.Tiles[1, 1]);

            var pathCount =
                (from EnvTile tile in tmOne.Tiles
                    where tile.OnPath
                    select tile).Count();

            // Empty with perimeter 
            Assert.AreEqual(25, pathCount);

            // reset
            (from EnvTile tile in tmOne.Tiles
                where tile.OnPath
                select tile).ToList().ForEach(tile => tile.OnPath = false);

            tmOne.Tiles[3, 3].HasEnv = true;

            p.GetPath(tmOne.Tiles[1, 1]);

            pathCount =
                (from EnvTile tile in tmOne.Tiles
                    where tile.OnPath
                    select tile).Count();

            Assert.AreEqual(24, pathCount);
        }
    }
}