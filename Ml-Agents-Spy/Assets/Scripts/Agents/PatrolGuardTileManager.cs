﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class PatrolGuardTileManager : IPatrolGuardTileManager
    {
        private IPatrolGuardTile _currentTile;

        public PatrolGuardTileManager(GameObject surrogateGameObject, IEnumerable<IEnvTile> envTiles,
            Transform agentTransform)
        {
            foreach (var envTile in envTiles)
                GuardTiles.Add(new PatrolGuardTile(surrogateGameObject, envTile.Position, envTile.Coords));

            _currentTile = agentTransform.GetNearestTile(1, GuardTiles, x => true).First();
            _currentTile.RecentlyVisitedByGuard = true;
        }

        public List<IPatrolGuardTile> GuardTiles { get; } = new List<IPatrolGuardTile>();


        /// <summary>
        ///     Used to get the location of the nearest tile. Must be called after CanRewardAgent
        /// </summary>
        /// <param name="agentPosition"></param>
        /// <returns></returns>
        public List<IPatrolGuardTile> GetNearestPatrolTile(Transform agentPosition) =>
            agentPosition
                .GetNearestTile(
                    3,
                    GuardTiles,
                    tile
                        => tile.tDistances.RecentlyVisitedByGuard == false &&
                           tile.tDistances.Coords != _currentTile.Coords
                )
                .PadList(3, null)
                .ToList();


        /// <summary>
        ///     Checks which tile is currently occupied by the agent. True is returned if the tile has changed then and it hasn't
        ///     been
        ///     visited within the tile timeout period.
        /// </summary>
        /// <remarks>
        ///     Logic of changing the current tile is tied into this function, so GetNearestPatrolTile must be called after
        ///     and the result stored in a field to get accurate nearest tile. (should be refactored)
        /// </remarks>
        /// <param name="agentPosition"></param>
        /// <returns></returns>
        public bool CanRewardAgent(Transform agentPosition)
        {
            var nearestTile =
                agentPosition.GetNearestTile(1, GuardTiles, tile => true).First();

            // check change in tile (no change in tile, no reward given)
            if (_currentTile is null || nearestTile.Coords != _currentTile.Coords)
            {
                // change current tile if moved
                _currentTile = nearestTile;
                // check that current tile has not been recently visited 
                if (!_currentTile.RecentlyVisitedByGuard)
                {
                    // if it hasn't, visit it and change its status to visited
                    _currentTile.RecentlyVisitedByGuard = true;
                    // if all tiles have been visited then reset them all apart from the current tile
                    if (GuardTiles.All(tile => tile.RecentlyVisitedByGuard))
                    {
                        GuardTiles
                            .Where(tile => !Equals(tile, _currentTile))
                            .ToList()
                            .ForEach(tile => tile.RecentlyVisitedByGuard = false);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}