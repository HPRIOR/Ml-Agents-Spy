using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tile
{
    private Vector3 Position { get; }
    private (int, int) Coords { get; }

    private Dictionary<Direction, Tile> AdjacentTiles { get; set; }
        = new Dictionary<Direction, Tile>();

    private bool HasSpy { get; set; } = false;
    private bool HasGuard { get; set; } = false;
    private bool HasEnv { get; set; } = false;


    public Tile(Vector3 position, (int, int) coords)
    {
        Position = position;
        Coords = coords;
    }

}
