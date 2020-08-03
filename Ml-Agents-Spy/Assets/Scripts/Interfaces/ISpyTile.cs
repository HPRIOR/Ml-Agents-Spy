using System.Collections.Generic;
using Enums;

namespace Interfaces
{
    public interface ISpyTile : ITile
    {
        Dictionary<Direction, ISpyTile> AdjacentTile { get;  }
        int VisitCount { get; set; }
        bool OccupiedByAgent { get; set; }
        bool VisitedByAlgo { get;  }
    }
}
