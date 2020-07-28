using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using static TileHelper;
using static VectorConversions;
using Direction = Enums.Direction;

namespace Agents
{
    public class AgentTileManager
    {
        List<IAgentTile> _agentTiles;
        private IAgentTile _currentAgentTile;
    

        public AgentTileManager(IAgentTileConverter tileConverter)
        {
            _agentTiles = tileConverter.GetAgentTiles();
            _currentAgentTile = _agentTiles.First(tile => tile.OccupiedByAgent);
        }


        public List<(float, Vector2)> GetTileMemory(Transform transform)
        {
            var nearestTiles = GetNearestTiles(transform);
            return GetTileVisitCount(nearestTiles)
                .Zip(GetTileLocations(nearestTiles), (visitCount, vector2) => (visitCount, vector2)).ToList();
        }

        /// <summary>
        /// Returns an array of floats between 0-1. The smaller the float the more times the agent has visited
        /// </summary>
        /// <param name="nearestTiles">List of the nearest tiles to the agent</param>
        /// <returns>An array of floats representing the number of time an agent has visited a certain tile</returns>
        float[] GetTileVisitCount(List<IAgentTile> nearestTiles)
        {
            float[] visitedTileCount = new float[8];
            for (int i = 0; i < 9; i++)
            {
                if (i < nearestTiles.Count) visitedTileCount[i] = nearestTiles[i].VisitCount;
            }
            return visitedTileCount;
        }

        /// <summary>
        /// Gets the Vector2 locations of the nearest tiles to the agent
        /// </summary>
        /// <param name="nearestTiles">List of the nearest tiles to the agent</param>
        /// <returns>Position of the nearest tiles</returns>
        Vector2[] GetTileLocations(List<IAgentTile> nearestTiles)
        {
            Vector2[] visitedTileLocations = new Vector2[8];
            for (int i = 0; i < 9; i++)
            {
                if (i < nearestTiles.Count) visitedTileLocations[i] = ConvertToVector2(nearestTiles[i].Position);
            }

            return visitedTileLocations;
        }


        /// <summary>
        /// updates the AgentTiles to find the tile nearest to the Agent at a given time
        /// </summary>
        /// <param name="agentPosition"></param>
        void UpdateAgentPosition(Transform agentPosition)
        {
            var newAgentTile = GetNearestTile(_agentTiles.ConvertAll(tile => (ITile)tile), agentPosition);
            if (newAgentTile.Coords != _currentAgentTile.Coords)
            {
            
                _currentAgentTile.OccupiedByAgent = false;
                _currentAgentTile = (IAgentTile)newAgentTile;
                _currentAgentTile.OccupiedByAgent = true;
                _currentAgentTile.VisitCount += 1;
            }
            else
            {
                _currentAgentTile.VisitCount += 1;
            }
        }


        Dictionary<Direction, IAgentTile> InitialiseDictionary()
        {
            Dictionary<Direction, IAgentTile> directionAgentDictionary = new Dictionary<Direction, IAgentTile>();

            foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                directionAgentDictionary[direction] = _currentAgentTile.AdjacentTile[direction];
            }
            return directionAgentDictionary;
        }


        List<IAgentTile> GetNearestTiles(Transform agentPosition)
        {
            UpdateAgentPosition(agentPosition);
            List<IAgentTile> nearestTiles = new List<IAgentTile>();
            Dictionary<Direction, IAgentTile> directionAgentDictionary = InitialiseDictionary();

            while (nearestTiles.Count < 8 && !AllTilesAreNull(directionAgentDictionary))
            {
                foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (directionAgentDictionary[direction] != null)
                    {
                        nearestTiles.Add(directionAgentDictionary[direction]);
                        // replace the current tile in a direction with the next tile in the same direction
                        directionAgentDictionary[direction] = directionAgentDictionary[direction].AdjacentTile[direction];
                    }
                }
            }
            return nearestTiles;
        }

        static bool AllTilesAreNull(Dictionary<Direction, IAgentTile> directionAgentDictionary)
            => directionAgentDictionary.All(item => item.Value == null);


    }
}
