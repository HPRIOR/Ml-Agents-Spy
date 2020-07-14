using System;
using System.Collections.Generic;
using System.Linq;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Creates Tile Matrix
/// </summary>
public class TileMatrix : ICloneable
{
    public Tile[,] Tiles { get; }
    private readonly Vector3 _planeCentre;
    public int MatrixSize { get; }
   

    public TileMatrix(Vector3 planeCentre, int matrixSize)
    {
        _planeCentre = planeCentre;
        MatrixSize = matrixSize;
        Tiles = CreateTilesMatrix(matrixSize, planeCentre);
        GetAdjacentTiles(Tiles, matrixSize);
    }

    /// <summary>
    /// Produces matrix of tiles 
    /// </summary>
    /// <param name="gridMapSize">max height/width from centre of outermost tile</param>
    /// <param name="planeCentre">centre of the target plane</param>
    /// <returns></returns>
    private Tile[,] CreateTilesMatrix(int gridMapSize, Vector3 planeCentre)
    {
        Tile[,] tiles = new Tile[gridMapSize+1, gridMapSize+1];
        int x = -gridMapSize;
        for (int i = 0; i < gridMapSize+1; i++)
        {
            int z = -gridMapSize;
            for (int j = 0; j < gridMapSize+1; j++)
            {
                tiles[i, j] =
                    new Tile(
                        position: planeCentre + new Vector3(x, 0.5f, z),
                        coords: (i, j)
                    );
                z += 2;
            }
            x += 2;
        }

        return tiles;
    }

    /// <summary>
    /// Populates the adjacency dictionary of each tile with NESW neighboring tiles
    /// </summary>
    /// <param name="tileMatrix">2D array of tiles</param>
    /// <param name="matrixSize">The maximum width and height of tiles</param>
    private void GetAdjacentTiles(Tile[,] tileMatrix, int matrixSize)
    {
        foreach (var tile in tileMatrix)
        {
            GetAllDirectionTiles(tile, tileMatrix, matrixSize);
        }
    }


    private void GetAllDirectionTiles(Tile tile, Tile[,] tileMatrix, int matrixSize) =>
        System.Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .ToList()
            .ForEach(direction => tile.AdjacentTile[direction] = GetDirectionTile(direction, tile, tileMatrix, matrixSize));
    
    private Tile GetDirectionTile(Direction d, Tile inputTile, Tile[,] tileMatrix, int matrixSize)
    {
        if (d == Direction.N) return inputTile.Coords.y == matrixSize ? null : tileMatrix[inputTile.Coords.x, inputTile.Coords.y + 1];
        if (d == Direction.E) return inputTile.Coords.x == matrixSize ? null : tileMatrix[inputTile.Coords.x + 1,inputTile.Coords.y];
        if (d == Direction.S) return inputTile.Coords.y == 0 ? null : tileMatrix[inputTile.Coords.x,inputTile.Coords.y - 1];
        if (d == Direction.W) return inputTile.Coords.x == 0 ? null : tileMatrix[inputTile.Coords.x - 1,inputTile.Coords.y];
        throw new Exception("No direction given in TileMatrix.GetDirectionTile");
    }

    public object Clone()
    {
        return (TileMatrix) new TileMatrix(planeCentre: _planeCentre, MatrixSize);
    }
}
