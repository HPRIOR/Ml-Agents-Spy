using System.Collections.Generic;
using System.Linq;
using EnvSetup;
using NUnit.Framework;
using UnityEngine;

public class TileLogicTests
{

    Dictionary<ParentObject, GameObject> GetDictionary ()
    {
        GameObject TopParent = new GameObject();
        GameObject EnvParent = new GameObject();
        GameObject SpyParent = new GameObject();
        GameObject GuardParent = new GameObject();
        GameObject DebugParent = new GameObject();
        
        return new Dictionary<ParentObject, GameObject>()
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
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict
            );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();

        Assert.AreEqual(3, tileTypes[TileType.ExitTiles].Count);
    }

    [Test]
    public void TestSpyCount()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
            

        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();

        Assert.AreEqual(1, tileTypes[TileType.SpyTile].Count);
    }

    [Test]
    public void TestGuardCount()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();

        Assert.AreEqual(2, tileTypes[TileType.GuardTiles].Count);
    }

    [Test]
    public void TestEnvTileCount()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();
        Assert.AreEqual(245, tileTypes[TileType.EnvTiles].Count);

        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 5,
            guardAgentCount: 2,
            parentDictionary: dict
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
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
        
        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();
        Assert.AreEqual(19, tileTypes[TileType.FreeTiles].Count);

    }

    [Test]
    public void TestExitLocationsOnYAxis()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
            

        
        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.ExitTiles].ForEach(tile => Assert.AreEqual(6 ,tile.Coords.y));


        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict
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
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
        
        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.SpyTile].ForEach(tile => Assert.AreEqual(1, tile.Coords.y));

        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict
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
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 0,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
    }


    [Test]
    public void TestGuardSpawnArea()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        var tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(5, tile.Coords.y));


        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 2,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        // 2 map size
        tileLogic = tileLogicBuilder.GetTileLogicSetup();
            

        
        tileLogic.GetTileLogic();
        tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(9, tile.Coords.y));

        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict
        );

        // 3 map size
        tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(15, tile.Coords.y));

        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 4,
            mapDifficulty: 0,
            exitCount: 10,
            guardAgentCount: 9,
            parentDictionary: dict
        );

        // > 3 (4) map size
        tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(true, tile.Coords.y == 19 | tile.Coords.y == 17 | tile.Coords.y == 18));

        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 10,
            guardAgentCount: 9,
            parentDictionary: dict
        );

        // > 3 (5) map size
        tileLogic = tileLogicBuilder.GetTileLogicSetup();

        tileLogic.GetTileLogic();
        tileTypes = tileLogic.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(true, tile.Coords.y == 25 || tile.Coords.y == 24 || tile.Coords.y == 23));
    }

    [Test]
    public void TestGuardCountLessThanExitCount()
    {
        var dict = GetDictionary();

        ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 4,
            parentDictionary: dict
        );

        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
        
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
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 0,
            guardAgentCount: 4,
            parentDictionary: dict
        );

        // exit count = 0
        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();


        Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
        
        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 1,
            guardAgentCount: 4,
            parentDictionary: dict
        );

        // exit count = 1
        tileLogic = tileLogicBuilder.GetTileLogicSetup();
            
        Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
        
        tileLogicBuilder = new TileLogicBuilder(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 4,
            parentDictionary: dict
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
            mapScale: 1,
            mapDifficulty: 100,
            exitCount: 3,
            guardAgentCount: 4,
            parentDictionary: dict
        );

        // exit count = 0
        ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();

        Assert.Throws<MapCreationException>(() => tileLogic.GetTileLogic());
    }


}
