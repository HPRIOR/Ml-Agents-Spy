﻿using System.Linq;
using static RandomHelper;

public class GuardLogic :IGuardLogic
{
    private readonly int _maxGuards;
    private readonly Tile[] _potentialGuardSpawnTiles;

    public GuardLogic(Tile[,] tiles, int mapScale, int matrixSize, int maxGuards)
    {
        _maxGuards = maxGuards;
        _potentialGuardSpawnTiles = PotentialGuardSpawnTiles(tiles, mapScale, matrixSize);
    }

    public bool GuardPlacesAreAvailable() => 
        GuardPlacesAreAvailableIn(_potentialGuardSpawnTiles, _maxGuards);
    
    public void SetGuardTiles() =>
        SetGuardTiles(_potentialGuardSpawnTiles, _maxGuards);
    

    /// <summary>
    /// Gets a list of tiles which are both on the Spy agents path and in the guard spawn area
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="mapScale">Size of map corresponding to plane scale</param>
    /// <param name="matrixSize">Size of Matrix</param>
    /// <returns>List of appropriate tiles</returns>
    private static Tile[] PotentialGuardSpawnTiles(Tile[,] tileMatrix, int mapScale, int matrixSize) =>
        (from Tile tile in tileMatrix
            where tile.OnPath
            where tile.Coords.x % 2 == 0
            where InGuardSpawnAreaY(tile, mapScale, matrixSize)
            select tile).ToArray();

    /// <summary>
    /// Checks that there are enough spawn tiles available and at least one guard agent being spawned
    /// </summary>
    /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
    /// <param name="guardCount">Number of guards agents to spawn</param>
    /// <returns>true if there are enough spawn tiles available and at least one guard agent being spawned</returns>
    private static bool GuardPlacesAreAvailableIn(Tile[] guardSpawnTiles, int guardCount) =>
        guardCount <= guardSpawnTiles.Length && guardCount >= 1;

    /// <summary>
    /// Randomly sets guard spawn tiles out of the candidate tiles 
    /// </summary>
    /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
    /// <param name="guardCount">Number of guards agents to spawn</param>
    private static void SetGuardTiles(Tile[] guardSpawnTiles, int guardCount)
    {
        var randomList = GetUniqueRandomList(guardCount, guardSpawnTiles.Length);
        for (int i = 0; i < guardCount; i++) guardSpawnTiles[randomList[i]].HasGuard = true;
    }

    /// <summary>
    /// Checks if tile is in the guard spawn area (1 row if mapScale less than 3, else 3 rows)
    /// </summary>
    /// <param name="tile">Tile to Check</param>
    /// <param name="mapScale">Size of the map corresponding to scale of plane</param>
    /// <param name="matrixSize">Size of tile matrix</param>
    /// <returns>True if tile is in the guard spawn area </returns>
    private static bool InGuardSpawnAreaY(Tile tile, int mapScale, int matrixSize) =>
        mapScale >= 1 & mapScale <= 3 & tile.Coords.y == matrixSize - 1
        || mapScale > 3 & (tile.Coords.y <= matrixSize - 1 & tile.Coords.y >= matrixSize - 3);
}
