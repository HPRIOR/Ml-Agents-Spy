using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnvSetup
{
    /// <summary>
    /// Individual tile which forms part of the environments grid system
    /// </summary>
    /// <remarks>
    /// Each tile contains reference to their NESW neighboring tiles, their Vector position on the map, and spawning information for agents 
    /// </remarks>
    public class EnvTile : ICloneable, IEnvTile
    {
        public Vector3 Position { get; set; }
        public (int x, int y) Coords { get; set; }

        public Dictionary<Direction, IEnvTile> AdjacentTile { get; set; }
            = new Dictionary<Direction, IEnvTile>
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
        public bool OnPath { get; set; } = false;



        public EnvTile(Vector3 position, (int, int) coords)
        {
            Position = position;
            Coords = (x: coords.Item1, y: coords.Item2);
        }

        public override string ToString() => $"Tile at coordinate: {Coords}, \n" +
                                             $"Position: {Position} \n" +
                                             $"HasSpy = {HasSpy} \n" +
                                             $"HasGuard = {HasGuard} \n" +
                                             $"HasEnv = {HasEnv} \n" +
                                             $"OnSpyPath = {OnPath} \n" +
                                             $"IsExit = {IsExit} \n" +
                                             $"North-Tile = {(AdjacentTile[Direction.N] is null ? "None" : AdjacentTile[Direction.N].Coords.ToString())} \n" +
                                             $"East-Tile = {(AdjacentTile[Direction.E] is null ? "None" : AdjacentTile[Direction.E].Coords.ToString())} \n" +
                                             $"South-Tile = {(AdjacentTile[Direction.S] is null ? "None" : AdjacentTile[Direction.S].Coords.ToString())} \n" +
                                             $"West-Tile = {(AdjacentTile[Direction.W] is null ? "None" : AdjacentTile[Direction.W].Coords.ToString())} \n";

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static explicit operator AgentTile(EnvTile e)
            => new AgentTile(e.Position, e.Coords);

    }
}
