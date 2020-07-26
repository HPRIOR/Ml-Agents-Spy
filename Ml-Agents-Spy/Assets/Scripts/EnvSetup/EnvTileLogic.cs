using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvTileLogic : IEnvTileLogic
{
    private readonly int _matrixSize;
    private readonly int _mapDifficulty;
    private bool _hasMiddleTiles;

    public EnvTileLogic(int matrixSize, int mapDifficulty, bool hasMiddleTiles = true)
    {
        _matrixSize = matrixSize;
        _mapDifficulty = mapDifficulty;
        _hasMiddleTiles = hasMiddleTiles;
    }
    public void SetEnvTiles(IEnvTile[,] tileMatrix)
    {
        CreateInitialEnv(tileMatrix, _matrixSize, _hasMiddleTiles);
        SetEnvDifficulty(tileMatrix, _matrixSize, _mapDifficulty);
    }

    /// <summary>
    /// Sets environment tiles on the perimeter and on their default positions in the middle
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <param name="hasMiddleTiles">allows to change whether middle tiles are created or not</param>
    private static void CreateInitialEnv(IEnvTile[,] tileMatrix, int matrixSize, bool hasMiddleTiles)
    {
        foreach (var tile in tileMatrix)
        {
            if (CanPlacePerimeter(tile, matrixSize)) tile.HasEnv = true;
            else
            {
                if (CanPlaceMiddle(tile, matrixSize) & hasMiddleTiles) tile.HasEnv = true;
            }

        }
    }

    /// <summary>
    /// Sets a random group of environment tiles 
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <param name="mapDifficulty">Number of tiles to randomly set</param>
    private static void SetEnvDifficulty(IEnvTile[,] tileMatrix, int matrixSize, int mapDifficulty)
    {
        var freeTiles =
            (from IEnvTile tile in tileMatrix
                where CanPlaceEnvDifficulty(tile, matrixSize)
                select tile)
            .ToArray();

        // Defaults to max free guardSpawnTiles if difficulty is higher. Ensures max 1 env-block per tile
        var checkDifficultyCount = mapDifficulty > freeTiles.Length ? freeTiles.Length : mapDifficulty;

        List<int> randSequence = RandomHelper.GetUniqueRandomList(mapDifficulty, freeTiles.Length);

        for (int i = 0; i < checkDifficultyCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            tile.HasEnv = true;
        }
    }

    /// <summary>
    /// Checks if a tile can be used as an environment tile to increase difficulty 
    /// </summary>
    /// <param name="envTile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if appropriate tile</returns>
    private static bool CanPlaceEnvDifficulty(IEnvTile envTile, int matrixSize) =>
        !envTile.IsExit & !envTile.HasEnv & !envTile.HasGuard & !envTile.HasSpy & !CanPlacePerimeter(envTile, matrixSize) & !CanPlaceMiddle(envTile, matrixSize);

    /// <summary>
    /// Checks if tile can be used as a default environment tile
    /// </summary>
    /// <param name="envTile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    private static bool CanPlaceMiddle(IEnvTile envTile, int matrixSize) =>
        (envTile.Coords.y % 2 == 0 & envTile.Coords.x % 2 == 0)
        & !(envTile.Coords.x == 0
            || envTile.Coords.x == matrixSize
            || envTile.Coords.y == 0
            || envTile.Coords.y == matrixSize);

    /// <summary>
    /// Checks if tile is on the outside perimeter of matrix
    /// </summary>
    /// <param name="envTile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if tile is on the perimeter</returns>
    private static bool CanPlacePerimeter(IEnvTile envTile, int matrixSize) =>
        (envTile.Coords.x == 0
         || envTile.Coords.x == matrixSize
         || envTile.Coords.y == 0
         || envTile.Coords.y == matrixSize)
        & !envTile.IsExit;
}
