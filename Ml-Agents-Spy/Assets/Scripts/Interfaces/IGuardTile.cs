using UnityEngine;

namespace Interfaces
{
    public interface IGuardTile
    {
        Vector3 Position { get; }
        (int x, int y) Coords { get; }
    }
}