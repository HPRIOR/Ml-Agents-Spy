using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvTileLogic : IEnvTileLogic
{
    private readonly Tile[,] _tiles;
    private readonly int _matrixSize;
    private readonly int _mapDifficulty;

    public EnvTileLogic(Tile[,] tiles, int matrixSize, int mapDifficulty)
    {
        _tiles = tiles;
        _matrixSize = matrixSize;
        _mapDifficulty = mapDifficulty;
    }
    public void SetEnvTiles()
    {
        CreateInitialEnv(_tiles, _matrixSize);
        SetEnvDifficulty(_tiles, _matrixSize, _mapDifficulty);
    }

    /// <summary>
    /// Sets environment tiles on the perimeter and on their default positions in the middle
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    private static void CreateInitialEnv(Tile[,] tileMatrix, int matrixSize)
    {
        foreach (var tile in tileMatrix)
        {
            if (CanPlacePerimeter(tile, matrixSize)) tile.HasEnv = true;
            else if (CanPlaceMiddle(tile, matrixSize)) tile.HasEnv = true;

        }
    }

    /// <summary>
    /// Sets a random group of environment tiles 
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <param name="mapDifficulty">Number of tiles to randomly set</param>
    private static void SetEnvDifficulty(Tile[,] tileMatrix, int matrixSize, int mapDifficulty)
    {
        var freeTiles =
            (from Tile tile in tileMatrix
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
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if appropriate tile</returns>
    private static bool CanPlaceEnvDifficulty(Tile tile, int matrixSize) =>
        !tile.IsExit & !tile.HasEnv & !tile.HasGuard & !tile.HasSpy & !CanPlacePerimeter(tile, matrixSize) & !CanPlaceMiddle(tile, matrixSize);

    /// <summary>
    /// Checks if tile can be used as a default environment tile
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    private static bool CanPlaceMiddle(Tile tile, int matrixSize) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !(tile.Coords.x == 0
            || tile.Coords.x == matrixSize
            || tile.Coords.y == 0
            || tile.Coords.y == matrixSize);

    /// <summary>
    /// Checks if tile is on the outside perimeter of matrix
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if tile is on the perimeter</returns>
    private static bool CanPlacePerimeter(Tile tile, int matrixSize) =>
        (tile.Coords.x == 0
         || tile.Coords.x == matrixSize
         || tile.Coords.y == 0
         || tile.Coords.y == matrixSize)
        & !tile.IsExit;
}
