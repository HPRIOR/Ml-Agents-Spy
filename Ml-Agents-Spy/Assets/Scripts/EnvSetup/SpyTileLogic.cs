using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomHelper;

public class SpyTileLogic : ISpyTileLogic
{
    private readonly IEnvTile[,] _tileMatrix;
    private readonly int _matrixSize;
    public SpyTileLogic(IEnvTile[,] tileMatrix, int matrixSize)
    {
        _tileMatrix = tileMatrix;
        _matrixSize = matrixSize;
    }
    /// <summary>
    /// Set the spawn tile of the Spy agent in the first row of the tile matrix
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    public IEnvTile SetSpyTile()
    {
        int y = 1;
        int x = GetParityRandom(1, _matrixSize - 1, ParityEnum.Even);
        _tileMatrix[x, y].HasSpy = true;
        return _tileMatrix[x, y];
    }
}
