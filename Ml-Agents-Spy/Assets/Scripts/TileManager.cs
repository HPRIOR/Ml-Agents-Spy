using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Numerics;
using Microsoft.Win32.SafeHandles;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Produces a 2D array of tiles
/// </summary>
public class TileManager
{
    public List<List<Tile>> Tiles { get; }
   

    public TileManager(int mapSize, Vector3 planeCentre, int gridMapSize)
    {
        Tiles = CreateTilesList(gridMapSize, planeCentre);
    }

   

    /// <summary>
    /// produces 2D list of tiles 
    /// </summary>
    /// <param name="gridMapSize">max height/width from centre of outermost tile</param>
    /// <param name="planeCentre">centre of the target plane</param>
    /// <returns></returns>
    private List<List<Tile>> CreateTilesList(int gridMapSize, Vector3 planeCentre)
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
    private void GetAdjacentTiles(List<List<Tile>> tiles)
    {

    }

}
