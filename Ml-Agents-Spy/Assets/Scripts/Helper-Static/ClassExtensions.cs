using System.Collections.Generic;
using System.Linq;
using EnvSetup;
using UnityEngine;

public static class ClassExtensions 
{
    public static List<EnvTile> CloneTileList(this IEnumerable<EnvTile> inputList) =>
        inputList.Select(tile => (EnvTile)tile.Clone()).ToList();

    public static T MostRecentlyAdded<T>(this Queue<T> queue) => queue.ToArray().ToList().Last();

    public static List<GameObject> GetNearest(this GameObject thisGameObject, List<GameObject> targets, int amount)
        => targets
            .Select(gameObjectDistance => (gameObjectDistance, Vector3.Distance(gameObjectDistance.transform.position, thisGameObject.transform.position)))
            .OrderBy(t => t.Item2)
            .Take(amount)
            .Select( t=> t.gameObjectDistance)
            .ToList();
     

}
