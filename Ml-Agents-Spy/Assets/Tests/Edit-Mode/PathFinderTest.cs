using System.Linq;
using NUnit.Framework;
using UnityEngine;
using static StaticFunctions;


public class PathFinderTest
{

    TileMatrix tmOne = new TileMatrix(new Vector3(0, 0, 0), MapScaleToMatrixSize(1));
    PathFinder p = new PathFinder();

    private void TestSetUp()
    {
        (from Tile tile in tmOne.Tiles
            where (tile.Coords.x == 0
                   || tile.Coords.x == 6
                   || tile.Coords.y == 0
                   || tile.Coords.y == 6)
        select tile).ToList().ForEach(tile => tile.HasEnv = true);

    }

    
    [Test]
    public void tmOnePathTest()
    {
        TestSetUp();
        p.GetPath(tmOne.Tiles[1,1]);

        int pathCount =
            (from Tile tile in tmOne.Tiles
                where tile.OnPath
                select tile).Count();

        // Empty with perimeter 
        Assert.AreEqual(25, pathCount);

        // reset
        (from Tile tile in tmOne.Tiles
            where tile.OnPath
            select tile).ToList().ForEach(tile => tile.OnPath = false);

        tmOne.Tiles[3,3].HasEnv = true;

        p.GetPath(tmOne.Tiles[1,1]);

        pathCount =
            (from Tile tile in tmOne.Tiles
                where tile.OnPath
                select tile).Count();

        Assert.AreEqual(24, pathCount);
    }

    
}
