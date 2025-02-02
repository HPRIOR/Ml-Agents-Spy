﻿using System.Linq;
using Interfaces;
using static RandomHelper;

namespace EnvSetup
{
    public class GuardTileLogic : IGuardTileLogic
    {
        private readonly int _mapScale;
        private readonly int _matrixSize;
        private readonly int _requestedGuardCount;
        private int _maxGuards;

        private IEnvTile[] _potentialGuardSpawnTiles;

        public GuardTileLogic(int mapScale, int matrixSize, int requestedGuardCount)
        {
            _matrixSize = matrixSize;
            _mapScale = mapScale;
            _requestedGuardCount = requestedGuardCount;
        }

        /// <summary>
        /// Ensures number of guards are 1 less than the number of exits
        /// </summary>
        /// <param name="maxExitCount"></param>
        public void GetMaxExitCount(int maxExitCount)
        {
            _maxGuards = _requestedGuardCount >= maxExitCount ? maxExitCount - 1 : _requestedGuardCount;
        }

        /// <summary>
        /// updates the private field with the potential spawn points of the guards
        /// </summary>
        /// <param name="tiles"></param>
        public void GetPotentialGuardPlaces(IEnvTile[,] tiles)
        {
            _potentialGuardSpawnTiles = PotentialGuardSpawnTiles(tiles, _mapScale, _matrixSize);
            // if (_potentialGuardSpawnTiles.Length < _requestedGuardCount)
            // {
            //     throw new MapCreationException("Requested guard places exceed number of available spawn tiles");
            // }
        }

        public bool GuardPlacesAreAvailable()
        {
            return GuardPlacesAreAvailableIn(_potentialGuardSpawnTiles, _maxGuards);
        }

        public void SetGuardTiles()
        {
            SetGuardTiles(_potentialGuardSpawnTiles, _maxGuards);
        }


        /// <summary>
        ///     Gets a list of tiles which are both on the Spy agents path and in the guard spawn area
        /// </summary>
        /// <param name="tileMatrix">Matrix of Tiles</param>
        /// <param name="mapScale">Size of map corresponding to plane scale</param>
        /// <param name="matrixSize">Size of Matrix</param>
        /// <returns>List of appropriate tiles</returns>
        private static IEnvTile[] PotentialGuardSpawnTiles(IEnvTile[,] tileMatrix, int mapScale, int matrixSize)
        {
            return (from IEnvTile tile in tileMatrix
                where tile.OnPath
                where tile.Coords.x % 2 == 0
                where InGuardSpawnAreaY(tile, mapScale, matrixSize)
                select tile).ToArray();
        }

        /// <summary>
        ///     Checks that there are enough spawn tiles available and at least one guard agent being spawned
        /// </summary>
        /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
        /// <param name="guardCount">Number of guards agents to spawn</param>
        /// <returns>true if there are enough spawn tiles available and at least one guard agent being spawned</returns>
        private static bool GuardPlacesAreAvailableIn(IEnvTile[] guardSpawnTiles, int guardCount)
        {
            return guardCount <= guardSpawnTiles.Length && guardCount >= 1;
        }

        /// <summary>
        ///     Randomly sets guard spawn tiles out of the candidate tiles
        /// </summary>
        /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
        /// <param name="guardCount">Number of guards agents to spawn</param>
        private static void SetGuardTiles(IEnvTile[] guardSpawnTiles, int guardCount)
        {
            var randomList = GetUniqueRandomList(guardCount, guardSpawnTiles.Length);
            for (var i = 0; i < guardCount; i++) guardSpawnTiles[randomList[i]].HasGuard = true;
        }

        /// <summary>
        ///     Checks if tile is in the guard spawn area (1 row if mapScale less than 3, else 3 rows)
        /// </summary>
        /// <param name="envTile">Tile to Check</param>
        /// <param name="mapScale">Size of the map corresponding to scale of plane</param>
        /// <param name="matrixSize">Size of tile matrix</param>
        /// <returns>True if tile is in the guard spawn area </returns>
        private static bool InGuardSpawnAreaY(IEnvTile envTile, int mapScale, int matrixSize)
        {
            return (mapScale >= 1) & (mapScale <= 3) & (envTile.Coords.y == matrixSize - 1)
                   || (mapScale > 3) & (envTile.Coords.y <= matrixSize - 1) & (envTile.Coords.y >= matrixSize - 3);
        }
    }
}