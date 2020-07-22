using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathFinder : IPathFinder
{
    
    /// <summary>
    /// For each tile it will attempt to visit NESW neighbor and change its Path to true
    /// Can move to adjacent tile if it is not an environment tile, if it hasn't already been visited and if it not null
    /// </summary>
    /// <param name="startEnvTile">Tile which the path starts from</param>
    public void GetPath(IEnvTile startEnvTile)
    {
        // DebugSphere(tile.Position);
        startEnvTile.OnPath = true;
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!startEnvTile.AdjacentTile[direction].HasEnv & !startEnvTile.AdjacentTile[direction].OnPath & !(startEnvTile.AdjacentTile[direction] is null))
                GetPath(startEnvTile.AdjacentTile[direction]);
        }
    }
    

    private static void DebugSphere(Vector3 tilePosition)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localPosition = tilePosition;
    }
}
