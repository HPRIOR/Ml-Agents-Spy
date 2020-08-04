using System;
using System.Collections.Generic;
using System.Linq;
using Agents;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

public static class ClassExtensions 
{
    public static List<EnvTile> CloneTileList(this IEnumerable<EnvTile> inputList) =>
        inputList.Select(tile => (EnvTile)tile.Clone()).ToList();

    public static T MostRecentlyAdded<T>(this Queue<T> queue) => queue.ToArray().ToList().Last();

    public static List<GameObject> GetNearest(this GameObject thisGameObject, int amount, List<GameObject> targets, Func<(GameObject gameObjectDistance, float),  bool> predicate)
        => targets
            .Select(gameObjectDistance => (gameObjectDistance, Vector3.Distance(gameObjectDistance.transform.position, thisGameObject.transform.position)))
            .Where(predicate)
            .OrderBy(t => t.Item2)
            .Take(amount)
            .Select( t=> t.gameObjectDistance)
            .ToList();
    
    public static List<T> GetNearestTile<T>(this Transform t, int amount, List<T> targets, Func<(T tDistances, float distance), bool> predicate) where T : ITile
        =>  targets
            .Select((tDistances) => (tDistances, Vector3.Distance(tDistances.Position, t.position)))
            .Where(predicate)
            .OrderBy(tDistances => tDistances.Item2)
            .Take(amount)
            .Select( valueTuple => valueTuple.tDistances)
            .ToList();

   


}
