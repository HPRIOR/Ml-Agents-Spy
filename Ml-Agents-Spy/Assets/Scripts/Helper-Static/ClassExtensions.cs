using System.Collections.Generic;
using System.Linq;
using EnvSetup;

public static class ClassExtensions 
{
    public static List<EnvTile> CloneTileList(this IEnumerable<EnvTile> inputList) =>
        inputList.Select(tile => (EnvTile)tile.Clone()).ToList();

    public static T MostRecentlyAdded<T>(this Queue<T> queue) => queue.ToArray().ToList().Last();

}
