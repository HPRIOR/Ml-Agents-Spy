using System.Linq;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathFinder
{
    public int ExitCount { get; set; } = 0;
    public void GetExitCount(Tile tile)
    {
        //DebugSphere(tile.Position);
        ExitCount += 1;
        tile.HasBeenVisited = true;
        
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!tile.AdjacentTile[direction].HasEnv & !tile.AdjacentTile[direction].HasBeenVisited & !(tile.AdjacentTile[direction] is null))
                if (!tile.IsExit) GetExitCount(tile.AdjacentTile[direction]);
        }
    }

    // public int ExitCount2(Tile tile)
    // {
    //     DebugSphere(tile.Position);
    //     tile.HasBeenVisited = true;
    // 
    //     foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
    //     {
    //         if (!tile.AdjacentTile[direction].HasEnv & !tile.AdjacentTile[direction].HasBeenVisited)
    //             return ExitCount2(tile.AdjacentTile[direction]) + 1;
    //     }
    // 
    //     return 0;
    // }

    private void DebugSphere(Vector3 tilePosition)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localPosition = tilePosition;
    }
}
