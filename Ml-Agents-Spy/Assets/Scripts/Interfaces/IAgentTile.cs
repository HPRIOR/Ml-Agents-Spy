using System.Collections.Generic;
using Enums;

namespace Interfaces
{
    public interface IAgentTile : ITile
    {
        Dictionary<Direction, IAgentTile> AdjacentTile { get; set; }
        int VisitCount { get; set; }
        bool OccupiedByAgent { get; set; }
        bool VisitedByAlgo { get; set; }
    }
}
