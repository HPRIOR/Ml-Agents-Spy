using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Profiling.Experimental;

public class PathFinder
{
    public int count = 0;
    public void ExitCount(Tile tile)
    {
        count += 1;
        tile.HasBeenVisited = true;
        if (!tile.AdjacentTile[Direction.N].HasEnv & !tile.AdjacentTile[Direction.N].HasBeenVisited)
            ExitCount(tile.AdjacentTile[Direction.N]);
        if (!tile.AdjacentTile[Direction.E].HasEnv & !tile.AdjacentTile[Direction.E].HasBeenVisited)
            ExitCount(tile.AdjacentTile[Direction.E]);
        if (!tile.AdjacentTile[Direction.W].HasEnv & !tile.AdjacentTile[Direction.W].HasBeenVisited)
            ExitCount(tile.AdjacentTile[Direction.W]);
        if (!tile.AdjacentTile[Direction.S].HasEnv & !tile.AdjacentTile[Direction.S].HasBeenVisited)
            ExitCount(tile.AdjacentTile[Direction.S]);
    }

    public int ExitCount2(Tile tile)
    {
        tile.HasBeenVisited = true;
        Debug.Log(tile);
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!tile.AdjacentTile[direction].HasEnv & !tile.AdjacentTile[direction].HasBeenVisited)
                return !tile.IsExit
                    ? ExitCount2(tile.AdjacentTile[direction]) + 1
                    : ExitCount2(tile.AdjacentTile[direction]) + 0;
        }
        return 0;
    }
}
