using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class ExitFinder : IExitFinder
{
    public int ExitCount { get; }
    private bool _canProceed = true;
    private readonly List<List<Tile>> _groupedAdjacentTiles;


    public ExitFinder(Tile[,] tileMatrix, int matrixSize, int requestedExitCount)
    {
        List<Tile> potentialExitTiles = PotentialExitTiles(tileMatrix, matrixSize);
        if (potentialExitTiles.Count < 2)
        {
            _canProceed = false;
        }
        else
        {
            _groupedAdjacentTiles = GroupAdjacentTiles(potentialExitTiles);
            var maxPossibleExitCount = TotalMaxExit(_groupedAdjacentTiles);
            ExitCount = requestedExitCount > maxPossibleExitCount
                ? maxPossibleExitCount
                : requestedExitCount;
        }
    }


    /// <summary>
    /// Gets perimeter tiles from farthest side of perimeter which are on the Spy agents potential path
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles </param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>List of tiles which are candidates for exit tiles</returns>
    static List<Tile> PotentialExitTiles(Tile[,] tileMatrix, int matrixSize) =>
        (from Tile tile in tileMatrix
            where tile.Coords.y == matrixSize
            where tile.AdjacentTile[Direction.S].OnPath
            select tile).ToList();

    static List<List<Tile>> GroupAdjacentTiles(List<Tile> tiles)
    {
        var orderedTiles = tiles.OrderBy(tile => tile.Coords.y).ToList();
        var adjacentTiles = new List<List<Tile>>();
        var currentList = new List<Tile> ();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (i != tiles.Count - 1)
            {
                if (TilesAreAdjacent(orderedTiles[i], orderedTiles[i + 1]))
                {
                    currentList.Add(tiles[i]);
                }
                else
                {
                    currentList.Add(tiles[i]);
                    adjacentTiles.Add(currentList);
                    currentList = new List<Tile>();
                }
            }
            else
            {
                if (orderedTiles[i].AdjacentTile[Direction.W] == orderedTiles[i - 1])
                {
                    currentList.Add(tiles[i]);
                    adjacentTiles.Add(currentList);
                }
                else
                {
                    adjacentTiles.Add(currentList);
                    currentList = new List<Tile> {orderedTiles[i]};
                    adjacentTiles.Add(currentList);
                }
            }
        }
        return adjacentTiles.Where(tile => tile.Count > 0).ToList();
    }


    static bool TilesAreAdjacent(Tile tileOne, Tile tileTwo) =>
        tileOne == tileTwo.AdjacentTile[Direction.W];

    static List<(int MaxExit, List<Tile> tileGroup)> AssociateExitCountWithTileGroup(List<List<Tile>> tileGroups)
    {
        List<(int MaxExit, List<Tile> tileGroup)> associatedList = new List<(int MaxExit, List<Tile> tileGroup)>();
        tileGroups.ForEach(tileGroup => associatedList.Add((MaxExit(tileGroup), tileGroup)));
        return associatedList;
    }

    static int MaxExit(List<Tile> tiles)
    {
        if (tiles.Count == 0) return 0;
        if (tiles.Count == 1 | tiles.Count == 2 | tiles.Count == 3) return 1;
        int nearestCeilingDivThree = tiles.Count;
        while (nearestCeilingDivThree % 3 != 0)
        {
            nearestCeilingDivThree += 1;
        }

        return (nearestCeilingDivThree / 3);
    }

    int TotalMaxExit(List<List<Tile>> tileGroups) => tileGroups.Select(MaxExit).Aggregate((a, b) => a + b);
    
    public void SetExitTiles()
    {
        
        List<(int MaxExit, List<Tile> tileGroup)> associatedList = AssociateExitCountWithTileGroup(_groupedAdjacentTiles);
        
        Random r = new Random();
        for (int i = 0; i < ExitCount; i++)
        {
            var tuple = associatedList[r.Next(0, associatedList.Count)];
            var selectedTile = tuple.tileGroup[r.Next(0, tuple.tileGroup.Count)];
            
            selectedTile.IsExit = true;
            selectedTile.HasEnv = false;
            
            // replace tuple with new one
            var replacementList = tuple.tileGroup;
            replacementList.Remove(selectedTile);
            replacementList.Remove(selectedTile.AdjacentTile[Direction.E]);
            replacementList.Remove(selectedTile.AdjacentTile[Direction.W]);
            
            int maxExit = MaxExit(replacementList);
            var replacementTuple = (maxExit, replacementList);
            if (maxExit > 0)
            {
                associatedList.Remove(tuple);
                associatedList.Add(replacementTuple);
            }
            else
            {
                associatedList.Remove(tuple);
            }
        }
    }

    public bool ExitsAreAvailable() => ExitCount > 1 && _canProceed;

    void DebugTuples(int iteration, List<(int MaxExit, List<Tile> tileGroup)> associatedList)
    {
        Debug.Log($"-------------------------ITERATION {iteration}-----------------------");
        foreach (var tuple in associatedList)
        {
            Debug.Log(tuple.MaxExit);
            foreach (var tile in tuple.tileGroup)
            {
                Debug.Log(tile);
            }
        }
    }
}