using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Profiling.Experimental;

public class PathFinder
{
   
    private int count = 0;

    
    public void SearchTiles(Tile tile)
    {
        tile.HasBeenVisited = true;
        Debug.Log(tile);
        Debug.Log(count);
        count += 1;
        if (!tile.AdjacentTile[Direction.N].HasEnv & !tile.AdjacentTile[Direction.N].HasBeenVisited)
        {
            SearchTiles(tile.AdjacentTile[Direction.N]);
            
        }
        if (!tile.AdjacentTile[Direction.E].HasEnv & !tile.AdjacentTile[Direction.E].HasBeenVisited)
        {
             SearchTiles(tile.AdjacentTile[Direction.E]);
             
        }
        if (!tile.AdjacentTile[Direction.W].HasEnv & !tile.AdjacentTile[Direction.W].HasBeenVisited)
        {
             SearchTiles(tile.AdjacentTile[Direction.W]);
             
        }
        if (!tile.AdjacentTile[Direction.S].HasEnv & !tile.AdjacentTile[Direction.S].HasBeenVisited)
        {
             SearchTiles(tile.AdjacentTile[Direction.S]);
             
        }
    }
}
