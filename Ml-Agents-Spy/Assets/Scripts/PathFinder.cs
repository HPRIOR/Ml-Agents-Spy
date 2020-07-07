using System.Linq;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public static  class PathFinder
{
    public static void GetSpyPathFrom(Tile tile)
    {
        DebugSphere(tile.Position);
        tile.OnSpyPath = true;
        
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!tile.AdjacentTile[direction].HasEnv & !tile.AdjacentTile[direction].OnSpyPath & !(tile.AdjacentTile[direction] is null))
                GetSpyPathFrom(tile.AdjacentTile[direction]);
        }
    }
    
    // this should be fixed - add exit count to arguments 
    //public int ExitCount2(Tile tile)
    //{
    //    DebugSphere(tile.Position);
    //    tile.OnSpyPath = true;
    //
    //    foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
    //    {
    //        if (!tile.AdjacentTile[direction].HasEnv & !tile.AdjacentTile[direction].OnSpyPath)
    //            return ExitCount2(tile.AdjacentTile[direction]) + 1;
    //    }
    //    return 0;
    //}

    private static void DebugSphere(Vector3 tilePosition)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localPosition = tilePosition;
    }
}
