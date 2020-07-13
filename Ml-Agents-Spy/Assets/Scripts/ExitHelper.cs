using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.PlayerLoop;
using Random = System.Random;


public class ExitHelper : IExitHelper
{
    public int ExitCount { get; }
    private readonly List<List<Tile>> _groupedAdjacentTiles;
    public ExitHelper(List<Tile> potentialExitTiles)
    {
        List<List<Tile>> tileGroups = GroupByAdjacent(potentialExitTiles);
        ExitCount = TotalMaxExit(tileGroups);
        _groupedAdjacentTiles = GroupByAdjacent(potentialExitTiles);
    }

    static List<List<Tile>> GroupByAdjacent(List<Tile> tiles)
    {
        var adjacentTiles = new List<List<Tile>>();
        var currentList = new List<Tile> {tiles[0]};
        for (int i = 1; i < tiles.Count; i++)
        {
            
            if (TilesAreAdjacent(tiles[i], currentList.Last()))
            {
                currentList.Add(tiles[i]);
                if (i == tiles.Count - 1)
                {
                    adjacentTiles.Add(currentList);
                }
            }
            else AddCurrentListAndReset(adjacentTiles, currentList, tiles[i]);
            
        }
        return adjacentTiles;
    }

    static void AddCurrentListAndReset(List<List<Tile>> adjacentTiles, List<Tile> currentList, Tile tile)
    {
        adjacentTiles.Add(currentList);
        currentList.Clear();
        currentList.Add(tile);
    }

    static bool TilesAreAdjacent(Tile tileOne, Tile tileTwo) =>
        tileOne == tileTwo.AdjacentTile[Direction.E] | tileOne == tileTwo.AdjacentTile[Direction.W];

    static int MaxExit(List<Tile> tiles)
    {
        if (tiles.Count == 2 | tiles.Count == 3) return 1;
        if (tiles.Count == 4 | tiles.Count == 5) return 2;
        int nearestFloorDivThree = tiles.Count;
        while (nearestFloorDivThree % 3 != 0)
        {
            nearestFloorDivThree -= 1;
        }

        return (nearestFloorDivThree / 3) + 1;

    }

    static int TotalMaxExit(List<List<Tile>> tileGroups) =>
        tileGroups.Select(tiles => MaxExit(tiles)).Aggregate((a,b) => a + b);
    
    public void SetExitTiles()
    {
        Random r = new Random();
        foreach (var tileGroup in _groupedAdjacentTiles)
        {
            int maxExits = MaxExit(tileGroup);
            for (int i = 0; i < maxExits; i++)
            {
                var selectedExit = tileGroup[r.Next(0, tileGroup.Count)];
                selectedExit.IsExit = true;
                selectedExit.HasEnv = false;
                tileGroup.Remove(selectedExit);
                tileGroup.Remove(selectedExit.AdjacentTile[Direction.E]);
                tileGroup.Remove(selectedExit.AdjacentTile[Direction.W]);
            }
        }
    }

    
}