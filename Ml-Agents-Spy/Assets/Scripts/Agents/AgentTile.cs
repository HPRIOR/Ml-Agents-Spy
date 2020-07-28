using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class AgentTile : IAgentTile
    {
        public Vector3 Position { get; set; }
        public (int x, int y) Coords { get; set; }
        public Dictionary<Direction, IAgentTile> AdjacentTile { get; set; }
            = new Dictionary<Direction, IAgentTile>
            {
                {Direction.N, null},
                {Direction.E, null},
                {Direction.S, null},
                {Direction.W, null}
            };

        public int VisitCount { get; set; } = 1;
        public bool OccupiedByAgent { get; set; } = false;

        public bool VisitedByAlgo { get; set; } = false;

        public AgentTile(Vector3 position, (int, int) coords)
        {
            Position = position;
            Coords = (x: coords.Item1, y: coords.Item2);
        }
    }
}
