﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class EnvSetupTests
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

        EnvSetup env = new EnvSetup(
          mapScale: 5,
          mapDifficulty: 0,
          exitCount:3,
          guardAgentCount: 2,
          parentDictionary: dict
        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();

        Assert.AreEqual(3, tileTypes[TileType.ExitTiles].Count);
    }

    [Test]
    public void TestSpyCount()
    {
        var dict = GetDictionary();

        EnvSetup env = new EnvSetup(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();

        Assert.AreEqual(1, tileTypes[TileType.SpyTile].Count);
    }

    [Test]
    public void TestGuardCount()
    {
        var dict = GetDictionary();

        EnvSetup env = new EnvSetup(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();

        Assert.AreEqual(2, tileTypes[TileType.GuardTiles].Count);
    }

    [Test]
    public void TestEnvTileCount()
    {
        var dict = GetDictionary();

        // initial set up 
        EnvSetup env = new EnvSetup(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 3,
            guardAgentCount: 2,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        Assert.AreEqual(245, tileTypes[TileType.EnvTiles].Count);

        // assert change in tiles on increase exit count
        env = new EnvSetup(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 5,
            guardAgentCount: 2,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        Assert.AreEqual(243, tileTypes[TileType.EnvTiles].Count);
    }


    [Test]
    public void TestFreeTilesCount()
    {
        var dict = GetDictionary();

        // initial set up 
        EnvSetup env = new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        Assert.AreEqual(19, tileTypes[TileType.FreeTiles].Count);

    }

    [Test]
    public void TestExitLocationsOnYAxis()
    {
        var dict = GetDictionary();

        // initial set up 
        EnvSetup env = new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        tileTypes[TileType.ExitTiles].ForEach(tile => Assert.AreEqual(6 ,tile.Coords.y));

        env = new EnvSetup(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.ExitTiles].ForEach(tile => Assert.AreEqual(16, tile.Coords.y));
    }

    [Test]
    public void TestSpyOnYAxis()
    {
        var dict = GetDictionary();

        // initial set up 
        EnvSetup env = new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        tileTypes[TileType.SpyTile].ForEach(tile => Assert.AreEqual(1, tile.Coords.y));

        env = new EnvSetup(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.SpyTile].ForEach(tile => Assert.AreEqual(1, tile.Coords.y));
    }

    [Test]
    public void TestGuardSpawnArea()
    {
        var dict = GetDictionary();

        // 1 map size
        EnvSetup env = new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(5, tile.Coords.y));
        // 2 map size
        env = new EnvSetup(
            mapScale: 2,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(9, tile.Coords.y));
        // 3 map size
        env = new EnvSetup(
            mapScale: 3,
            mapDifficulty: 0,
            exitCount: 4,
            guardAgentCount: 1,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(15, tile.Coords.y));
        // > 3 (4) map size
        env = new EnvSetup(
            mapScale: 4,
            mapDifficulty: 0,
            exitCount: 10,
            guardAgentCount: 9,
            parentDictionary: dict

        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(true, tile.Coords.y == 19 | tile.Coords.y == 17 | tile.Coords.y == 18));
        // > 3 (5) map size
        env = new EnvSetup(
            mapScale: 5,
            mapDifficulty: 0,
            exitCount: 10,
            guardAgentCount: 9,
            parentDictionary: dict
        );
        env.SetUpEnv();
        tileTypes = env.GetTileTypes();
        tileTypes[TileType.GuardTiles].ForEach(tile => Assert.AreEqual(true, tile.Coords.y == 25 || tile.Coords.y == 24 || tile.Coords.y == 23));
    }

    [Test]
    public void TestGuardCountLessThanExitCount()
    {
        var dict = GetDictionary();
        EnvSetup env = new EnvSetup(
            mapScale: 1,
            mapDifficulty: 0,
            exitCount: 2,
            guardAgentCount: 4,
            parentDictionary: dict

        );
        env.SetUpEnv();
        var tileTypes = env.GetTileTypes();
        Assert.Greater(tileTypes[TileType.ExitTiles].Count, tileTypes[TileType.GuardTiles].Count);
        Assert.AreEqual(1, tileTypes[TileType.GuardTiles].Count);
    }
    





    // check exit count > 1
    // test edge case of setExits == max number of exits
    // test env complexity numbers 
    // test exit counts < size of map
    // test throw error

}
