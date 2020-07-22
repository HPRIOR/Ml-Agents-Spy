using UnityEngine;

public interface IAdjacentTileHelper
{
    void GetAdjacentTiles(ITile[,] tileMatrix, int matrixSize);
}
