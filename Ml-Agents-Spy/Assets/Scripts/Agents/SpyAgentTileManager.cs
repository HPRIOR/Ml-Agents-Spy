using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;
using static TileHelper;
using static VectorConversions;

namespace Agents
{
    public class SpyAgentTileManager
    {
        private readonly List<ISpyTile> _agentTiles;
        private ISpyTile _currentSpyTile;


        public SpyAgentTileManager(IAgentTileConverter tileConverter)
        {
            _agentTiles = tileConverter.GetAgentTiles();
            _currentSpyTile = _agentTiles.First(tile => tile.OccupiedByAgent);
        }


        public List<(float, Vector2)> GetTileMemory(Transform transform)
        {
            var nearestTiles = GetNearestTiles(transform);
            return GetTileVisitCount(nearestTiles)
                .Zip(GetTileLocations(nearestTiles), (visitCount, vector2) => (visitCount, vector2)).ToList();
        }

        /// <summary>
        ///     Returns an array of floats between 0-1. The smaller the float the more times the agent has visited
        /// </summary>
        /// <param name="nearestTiles">List of the nearest tiles to the agent</param>
        /// <returns>An array of floats representing the number of time an agent has visited a certain tile</returns>
        private float[] GetTileVisitCount(List<ISpyTile> nearestTiles)
        {
            var visitedTileCount = new float[8];
            for (var i = 0; i < 9; i++)
                if (i < nearestTiles.Count)
                    visitedTileCount[i] = nearestTiles[i].VisitCount;
            return visitedTileCount;
        }

        /// <summary>
        ///     Gets the Vector2 locations of the nearest tiles to the agent
        /// </summary>
        /// <param name="nearestTiles">List of the nearest tiles to the agent</param>
        /// <returns>Position of the nearest tiles</returns>
        private Vector2[] GetTileLocations(List<ISpyTile> nearestTiles)
        {
            var visitedTileLocations = new Vector2[8];
            for (var i = 0; i < 9; i++)
                if (i < nearestTiles.Count)
                    visitedTileLocations[i] = ConvertToVector2(nearestTiles[i].Position);

            return visitedTileLocations;
        }


        /// <summary>
        ///     updates the AgentTiles to find the tile nearest to the Agent at a given time
        /// </summary>
        /// <param name="agentPosition"></param>
        private void UpdateAgentPosition(Transform agentPosition)
        {
            var newAgentTile = GetNearestTile(_agentTiles.ConvertAll(tile => (ITile) tile), agentPosition);
            if (newAgentTile.Coords != _currentSpyTile.Coords)
            {
                _currentSpyTile.OccupiedByAgent = false;
                _currentSpyTile = (ISpyTile) newAgentTile;
                _currentSpyTile.OccupiedByAgent = true;
                _currentSpyTile.VisitCount += 1;
            }
            else
            {
                _currentSpyTile.VisitCount += 1;
            }
        }


        private Dictionary<Direction, ISpyTile> InitialiseDictionary()
        {
            var directionAgentDictionary = new Dictionary<Direction, ISpyTile>();

            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                directionAgentDictionary[direction] = _currentSpyTile.AdjacentTile[direction];
            return directionAgentDictionary;
        }


        private List<ISpyTile> GetNearestTiles(Transform agentPosition)
        {
            UpdateAgentPosition(agentPosition);
            var nearestTiles = new List<ISpyTile>();
            var directionAgentDictionary = InitialiseDictionary();

            while (nearestTiles.Count < 8 && !AllTilesAreNull(directionAgentDictionary))
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                    if (directionAgentDictionary[direction] != null)
                    {
                        nearestTiles.Add(directionAgentDictionary[direction]);
                        // replace the current tile in a direction with the next tile in the same direction
                        directionAgentDictionary[direction] =
                            directionAgentDictionary[direction].AdjacentTile[direction];
                    }

            return nearestTiles;
        }

        private static bool AllTilesAreNull(Dictionary<Direction, ISpyTile> directionAgentDictionary)
        {
            return directionAgentDictionary.All(item => item.Value == null);
        }
    }
}