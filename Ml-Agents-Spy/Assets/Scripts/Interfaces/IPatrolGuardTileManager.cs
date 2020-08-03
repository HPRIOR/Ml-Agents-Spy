using Agents;
using UnityEngine;

namespace Interfaces
{
    public interface IPatrolGuardTileManager
    {
        IPatrolGuardTile LocationOfNearestTile(Transform agentPosition);
        bool CanRewardAgent(Transform agentPosition);
       
    }
}