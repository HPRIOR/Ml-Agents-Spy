using UnityEngine;

namespace Interfaces
{
    public interface ITile
    {
        Vector3 Position { get; }
        (int x, int y) Coords { get; }
        string ToString();
    }
}