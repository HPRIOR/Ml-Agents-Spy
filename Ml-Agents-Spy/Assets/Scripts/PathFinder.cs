using System.Linq;

public class PathFinder : IPathFinder
{
    /// <summary>
    /// Walks through every tile that can be reached from a specific tile - does not work on large maps (stack overflow)
    /// </summary>
    /// <param name="startTile">Start tile</param>
    public void GetPath(Tile startTile)
    {
        // DebugSphere(tile.Position);
        startTile.OnSpyPath = true;
        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            if (!startTile.AdjacentTile[direction].HasEnv & !startTile.AdjacentTile[direction].OnSpyPath & !(startTile.AdjacentTile[direction] is null))
                GetPath(startTile.AdjacentTile[direction]);
        }
    }
}
