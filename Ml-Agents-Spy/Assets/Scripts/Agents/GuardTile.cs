using EnvSetup;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class GuardTile : IGuardTile
    {
        public Vector3 Position { get; }
        public (int x, int y) Coords { get; }

        public GuardTile(Vector3 position, (int x, int y) coords)
        {
            Position = position;
            Coords = coords;
        }
    }
}