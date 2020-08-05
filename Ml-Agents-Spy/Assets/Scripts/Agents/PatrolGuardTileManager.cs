using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Agents
{
    class PatrolGuardTileManager : IPatrolGuardTileManager
    {
        private readonly List<IPatrolGuardTile> _guardTiles = new List<IPatrolGuardTile>();

        private IPatrolGuardTile _currentTile;
        
        public PatrolGuardTileManager(GameObject surrogateGameObject, IEnumerable<IEnvTile> envTiles)
        {
            foreach (var envTile in envTiles)
            {
                _guardTiles.Add(new PatrolGuardTile(surrogateGameObject, envTile.Position, envTile.Coords));
            }
        }
        
        //need to work out the distance between two tiles ( && tile != _currentTile)
        /// <summary>
        /// Used to get the location of the nearest tile 
        /// </summary>
        /// <param name="agentPosition"></param>
        /// <returns></returns>
        public IPatrolGuardTile LocationOfNearestTile(Transform agentPosition) =>
            agentPosition
                .GetNearestTile(
                    1,
                    _guardTiles,
                    tile => tile.distance > 1 & tile.tDistances != _currentTile
                    )[0];
        
        public bool CanRewardAgent(Transform agentPosition)
        { 
            var nearestTile = 
                agentPosition.GetNearestTile(1, _guardTiles, tile => true)[0];
            
            if (_currentTile is null || nearestTile != _currentTile)
            {
               _currentTile = nearestTile;
               return true;
            }
            return false;
        }
        
    }
}