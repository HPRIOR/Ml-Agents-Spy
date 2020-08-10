using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IPatrolGuardTileManager
    {
        List<IPatrolGuardTile> GuardTiles { get; }
        List<IPatrolGuardTile> GetNearestPatrolTile(Transform agentPosition);
        bool CanRewardAgent(Transform agentPosition);
    }
}