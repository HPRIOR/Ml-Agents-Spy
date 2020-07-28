using UnityEngine;

namespace Interfaces
{
    public interface ITile 
    {
        Vector3 Position { get; set; }
        (int x, int y) Coords { get; set; }
        string ToString();
    }
}
