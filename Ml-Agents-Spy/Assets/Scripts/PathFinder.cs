using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathFinder : IPathFinder
{
    
    /// <summary>
    /// For each tile it will attempt to visit NESW neighbor and change its Path to true
    /// Can move to adjacent tile if it is not an environment tile, if it hasn't already been visited and if it not null
    /// </summary>
    /// <param name="startTile">Tile which the path starts from</param>
    public void GetPath(Tile startTile)
    {
        // DebugSphere(tile.Position);
        startTile.OnPath = true;
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!startTile.AdjacentTile[direction].HasEnv & !startTile.AdjacentTile[direction].OnPath & !(startTile.AdjacentTile[direction] is null))
                GetPath(startTile.AdjacentTile[direction]);
        }
    }
    

    private static void DebugSphere(Vector3 tilePosition)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localPosition = tilePosition;
    }
}
