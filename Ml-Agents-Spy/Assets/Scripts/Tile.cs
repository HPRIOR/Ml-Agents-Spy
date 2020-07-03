using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector3 Position { get; }
    public (int x, int y) Coords { get; }

    public Dictionary<Direction, Tile> AdjacentTile { get; set; }
        = new Dictionary<Direction, Tile>()
        {
            {Direction.N, null},
            {Direction.E, null},
            {Direction.S, null},
            {Direction.W, null}
        };

    public bool HasSpy { get; set; } = false;
    public bool HasGuard { get; set; } = false;
    public bool HasEnv { get; set; } = false;
    public bool IsExit { get; set; } = false;
    public bool HasBeenVisited { get; set; } = false;



    public Tile(Vector3 position, (int, int) coords)
    {
        Position = position;
        Coords = (x: coords.Item1, y: coords.Item2);
    }

    public override string ToString() => $"Tile at coordinate: {Coords}, \n" +
                                         $"Position: {Position} \n" +
                                         $"HasSpy = {HasSpy} \n" +
                                         $"HasGuard = {HasGuard} \n" +
                                         $"HasEnv = {HasEnv} \n" +
                                         $"HasBeenVisited = {HasBeenVisited} \n" +
                                         $"IsExit = {IsExit} \n" +
                                         $"North-Tile = {(AdjacentTile[Direction.N] is null ? "None" : AdjacentTile[Direction.N].Coords.ToString())} \n" +
                                         $"East-Tile = {(AdjacentTile[Direction.E] is null ? "None" : AdjacentTile[Direction.E].Coords.ToString())} \n" +
                                         $"South-Tile = {(AdjacentTile[Direction.S] is null ? "None" : AdjacentTile[Direction.S].Coords.ToString())} \n" +
                                         $"West-Tile = {(AdjacentTile[Direction.W] is null ? "None" : AdjacentTile[Direction.W].Coords.ToString())} \n";
        
    
}
