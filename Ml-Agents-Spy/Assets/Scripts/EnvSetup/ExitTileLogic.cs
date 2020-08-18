using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;
using Random = System.Random;

namespace EnvSetup
{
    public class ExitTileLogic : IExitTileLogic
    {
        private readonly int _matrixSize;
        private readonly int _requestedExitCount;

        private List<List<IEnvTile>> _groupedAdjacentTiles;


        public ExitTileLogic(int matrixSize, int requestedExitCount)
        {
            _matrixSize = matrixSize;
            _requestedExitCount = requestedExitCount;
            
        }

        public int ExitCount { get; private set; }
        public bool CanProceed { get; set; } = true;

        public void CheckMatrix(IEnvTile[,] tileMatrix)
        {
            var potentialExitTiles = PotentialExitTiles(tileMatrix, _matrixSize);
            if (potentialExitTiles.Count < 2)
            {
                CanProceed = false;
            }
            else
            {
                _groupedAdjacentTiles = GroupAdjacentTiles(potentialExitTiles);
                var maxPossibleExitCount = TotalMaxExit(_groupedAdjacentTiles);
                ExitCount = _requestedExitCount > maxPossibleExitCount
                    ? maxPossibleExitCount
                    : _requestedExitCount;
            }
        }

        public void SetExitTiles()
        {
            List<(int MaxExit, List<IEnvTile> tileGroup)> associatedList = AssociateExitCountWithTileGroup(_groupedAdjacentTiles);

            var r = new Random();
            for (var i = 0; i < ExitCount; i++)
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

                var maxExit = MaxExit(replacementList);
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

        public bool ExitsAreAvailable()
        {
            return ExitCount > 1 && CanProceed;
        }


        /// <summary>
        ///     Gets perimeter tiles from farthest side of perimeter which are on the Spy agents potential path
        /// </summary>
        /// <param name="tileMatrix">Matrix of Tiles </param>
        /// <param name="matrixSize">Size of matrix</param>
        /// <returns>List of tiles which are candidates for exit tiles</returns>
        private static List<IEnvTile> PotentialExitTiles(IEnvTile[,] tileMatrix, int matrixSize)
        {
            return (from IEnvTile tile in tileMatrix
                where tile.Coords.y == matrixSize
                where tile.AdjacentTile[Direction.S].OnPath
                select tile).ToList();
        }

        private static List<List<IEnvTile>> GroupAdjacentTiles(List<IEnvTile> tiles)
        {
            var orderedTiles = tiles.OrderBy(tile => tile.Coords.y).ToList();
            var adjacentTiles = new List<List<IEnvTile>>();
            var currentList = new List<IEnvTile>();
            for (var i = 0; i < tiles.Count; i++)
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
                        currentList = new List<IEnvTile>();
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
                        currentList = new List<IEnvTile> {orderedTiles[i]};
                        adjacentTiles.Add(currentList);
                    }
                }

            return adjacentTiles.Where(tile => tile.Count > 0).ToList();
        }


        private static bool TilesAreAdjacent(IEnvTile envTileOne, IEnvTile envTileTwo)
        {
            return envTileOne == envTileTwo.AdjacentTile[Direction.W];
        }

        private static List<(int MaxExit, List<IEnvTile> tileGroup)> AssociateExitCountWithTileGroup(
            List<List<IEnvTile>> tileGroups)
        {
            var associatedList = new List<(int MaxExit, List<IEnvTile> tileGroup)>();
            tileGroups.ForEach(tileGroup => associatedList.Add((MaxExit(tileGroup), tileGroup)));
            return associatedList;
        }

        private static int MaxExit(List<IEnvTile> tiles)
        {
            if (tiles.Count == 0) return 0;
            if ((tiles.Count == 1) | (tiles.Count == 2) | (tiles.Count == 3)) return 1;
            var nearestCeilingDivThree = tiles.Count;
            while (nearestCeilingDivThree % 3 != 0) nearestCeilingDivThree += 1;
            return nearestCeilingDivThree / 3;
        }

        private int TotalMaxExit(List<List<IEnvTile>> tileGroups)
        {
            return tileGroups.Select(MaxExit).Aggregate((a, b) => a + b);
        }

        private void DebugTuples(int iteration, List<(int MaxExit, List<IEnvTile> tileGroup)> associatedList)
        {
            Debug.Log($"-------------------------ITERATION {iteration}-----------------------");
            foreach (var tuple in associatedList)
            {
                Debug.Log(tuple.MaxExit);
                foreach (var tile in tuple.tileGroup) Debug.Log(tile);
            }
        }
    }
}