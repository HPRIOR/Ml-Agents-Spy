using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class Tile
{
    public Vector3 Position { get; }
    public (int x, int y) Coords { get; }

    public Dictionary<Direction, Tile> AdjacentTiles { get; set; }
        = new Dictionary<Direction, Tile>();

    public bool HasSpy { get; set; } = false;
    public bool HasGuard { get; set; } = false;
    public bool HasEnv { get; set; } = false;
    public bool IsExit { get; set; } = false;


    public Tile(Vector3 position, (int, int) coords)
    {
        Position = position;
        Coords = (x: coords.Item1, y: coords.Item2);
    }

    public override string ToString()
    {
        return $"Tile at coordinate: {Coords}, \n" +
               $"Position: {Position} \n" +
               $"HasSpy = {HasSpy} \n" +
               $"HasGuard = {HasGuard} \n" +
               $"HasEnv = {HasEnv} \n";
    }
}
