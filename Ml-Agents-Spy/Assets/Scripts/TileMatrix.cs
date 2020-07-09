using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Produces a 2D array of tiles
/// </summary>
public class TileMatrix : ICloneable
{
    public List<List<Tile>> Tiles { get; }
    private Vector3 _planeCentre;
    private int _gridMapSize;
   

    public TileMatrix(Vector3 planeCentre, int gridMapSize)
    {
        _planeCentre = planeCentre;
        _gridMapSize = gridMapSize;
        Tiles = CreateTilesMatrix(gridMapSize, planeCentre);
        GetAdjacentTiles(Tiles, gridMapSize);
    }

    /// <summary>
    /// Produces matrix of tiles 
    /// </summary>
    /// <param name="gridMapSize">max height/width from centre of outermost tile</param>
    /// <param name="planeCentre">centre of the target plane</param>
    /// <returns></returns>
    private List<List<Tile>> CreateTilesMatrix(int gridMapSize, Vector3 planeCentre)
    {
        int x = -gridMapSize;
        List<List<Tile>> tileList = new List<List<Tile>>();
        for (int i = 0; i < gridMapSize+1; i++)
        {
            int z = -gridMapSize;
            List<Tile> column = new List<Tile>();
            for (int j = 0; j < gridMapSize+1; j++)
            {
                column.Add(
                    new Tile(
                        position: planeCentre + new Vector3(x, 0.5f, z),
                        coords: (i, j) 
                    )
                );
                z += 2;
            }
            tileList.Add(column);
            x += 2;
        }
        return tileList;
    }

    /// <summary>
    /// Populates the adjacency dictionary of each tile with NESW neighboring tiles
    /// </summary>
    /// <param name="tiles">2D array of tiles</param>
    /// <param name="maxMapRange">The maximum width and height of tiles</param>
    private void GetAdjacentTiles(List<List<Tile>> tiles, int maxMapRange) =>
        tiles.
            ForEach(tileRow => tileRow.
                ForEach(tile => GetAllDirectionTiles(tile, tiles, maxMapRange)));
    

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
        return (TileMatrix) new TileMatrix(planeCentre: _planeCentre, _gridMapSize);
    }
}
