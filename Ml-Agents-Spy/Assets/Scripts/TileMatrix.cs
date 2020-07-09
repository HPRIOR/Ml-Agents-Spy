using System;
using System.Collections.Generic;
using System.Linq;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Produces a 2D array of tiles
/// </summary>
public class TileMatrix : ICloneable
{
    public List<List<Tile>> Tiles { get; }
    private readonly Vector3 _planeCentre;
    private readonly int _matrixSize;
   

    public TileMatrix(Vector3 planeCentre, int matrixSize)
    {
        _planeCentre = planeCentre;
        _matrixSize = matrixSize;
        Tiles = CreateTilesMatrix(matrixSize, planeCentre);
        GetAdjacentTiles(Tiles, matrixSize);
    }

    /// <summary>
    /// Produces matrix of tiles 
    /// </summary>
    /// <param name="matrixSize">Size of the matrix</param>
    /// <param name="planeCentre">centre of the target plane</param>
    /// <returns>matrix of tiles</returns>
    private List<List<Tile>> CreateTilesMatrix(int matrixSize, Vector3 planeCentre)
    {
        int x = -matrixSize;
        List<List<Tile>> tilesMatrix = new List<List<Tile>>();
        for (int i = 0; i < matrixSize+1; i++)
        {
            int z = -matrixSize;
            List<Tile> column = new List<Tile>();
            for (int j = 0; j < matrixSize+1; j++)
            {
                column.Add(
                    new Tile(
                        position: planeCentre + new Vector3(x, 0.5f, z),
                        coords: (i, j) 
                    )
                );
                z += 2;
            }
            tilesMatrix.Add(column);
            x += 2;
        }
        return tilesMatrix;
    }

    /// <summary>
    /// Populates the adjacency dictionary of each tile with NESW neighboring tiles
    /// </summary>
    /// <param name="tileMatrix">2D array of tiles</param>
    /// <param name="matrixSize">The maximum width and height of tiles</param>
    private void GetAdjacentTiles(List<List<Tile>> tileMatrix, int matrixSize) =>
        tileMatrix.
            ForEach(tileRow => tileRow.
                ForEach(tile => GetAllDirectionTiles(tile, tileMatrix, matrixSize)));
    

    private void GetAllDirectionTiles(Tile tile, List<List<Tile>> tiles, int maxMapRange) =>
        System.Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .ToList()
            .ForEach(direction => tile.AdjacentTile[direction] = GetDirectionTile(direction, tile, tiles, maxMapRange));
    

    private Tile GetDirectionTile(Direction d, Tile inputTile, List<List<Tile>> tiles, int mapMaxRange)
    {
        if (d == Direction.N) return inputTile.Coords.y == mapMaxRange ? null : tiles[inputTile.Coords.x][inputTile.Coords.y + 1];
        if (d == Direction.E) return inputTile.Coords.x == mapMaxRange ? null : tiles[inputTile.Coords.x + 1][inputTile.Coords.y];
        if (d == Direction.S) return inputTile.Coords.y == 0 ? null : tiles[inputTile.Coords.x][inputTile.Coords.y - 1];
        if (d == Direction.W) return inputTile.Coords.x == 0 ? null : tiles[inputTile.Coords.x - 1][inputTile.Coords.y];
        throw new Exception("No direction given in TileMatrix.GetDirectionTile");
    }

    public object Clone()
    {
        return (TileMatrix) new TileMatrix(planeCentre: _planeCentre, _matrixSize);
    }
}
