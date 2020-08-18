using System;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;

namespace EnvSetup
{
    public class PathFinder : IPathFinder
    {
        /// <summary>
        ///     For each tile it will attempt to visit NESW neighbor and change its Path to true
        ///     Can move to adjacent tile if it is not an environment tile, if it hasn't already been visited and if it not null
        /// </summary>
        /// <param name="envTile">Tile which the path starts from</param>
        public void GetPath(IEnvTile envTile)
        {
            envTile.OnPath = true;
            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                if (!envTile.AdjacentTile[direction].HasEnv & !envTile.AdjacentTile[direction].OnPath &
                    !(envTile.AdjacentTile[direction] is null))
                    GetPath(envTile.AdjacentTile[direction]);
        }


        private static void DebugSphere(Vector3 tilePosition)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localPosition = tilePosition;
        }
    }
}