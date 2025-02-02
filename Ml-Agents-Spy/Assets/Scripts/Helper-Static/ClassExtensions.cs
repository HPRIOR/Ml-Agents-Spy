﻿using System;
using System.Collections.Generic;
using System.Linq;
using EnvSetup;
using Interfaces;
using UnityEngine;

public static class ClassExtensions
{
    public static List<EnvTile> CloneTileList(this IEnumerable<EnvTile> inputList)
    {
        return inputList.Select(tile => (EnvTile) tile.Clone()).ToList();
    }

    /// <summary>
    ///     Returns item at the end of the queue
    /// </summary>
    /// <param name="queue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T MostRecentlyAdded<T>(this Queue<T> queue)
    {
        return queue.ToArray().ToList().Last();
    }

    /// <summary>
    ///     Gets a specified amount of the nearest gameobject(s) to the current gameobject
    /// </summary>
    /// <param name="thisGameObject">Current game object</param>
    /// <param name="amount">Number of gameobjects to return</param>
    /// <param name="targets">List of gameobjects which will be evaluated for closeness</param>
    /// <param name="predicate">Predicate used to filter which of the targets should be evaluated</param>
    /// <returns>List of game objects in ascending order of distance</returns>
    public static List<GameObject> GetNearest(this GameObject thisGameObject, int amount, List<GameObject> targets,
        Func<(GameObject gameObjectDistance, float), bool> predicate) => 
        targets
            .Select(gameObjectDistance => (gameObjectDistance,
                Vector3.Distance(gameObjectDistance.transform.position, thisGameObject.transform.position)))
            .Where(predicate)
            .OrderBy(t => t.Item2)
            .Take(amount)
            .Select(t => t.gameObjectDistance)
            .ToList();
    

    /// <summary>
    ///     Gets the nearest tile(s) to the current transform
    /// </summary>
    /// <param name="t">Current transform</param>
    /// <param name="amount">number of tiles to return</param>
    /// <param name="targets">List of tiles to evaluate for closeness</param>
    /// <param name="predicate">Predicate used to filter which of the target tiles should be evaluated</param>
    /// <typeparam name="T">Any tile type which extends ITile</typeparam>
    /// <returns>List of tiles in ascending order of distance</returns>
    public static List<T> GetNearestTile<T>(this Transform t, int amount, List<T> targets,
        Func<(T tDistances, float distance), bool> predicate) where T : ITile
    {
        return targets
            .Select(tDistances => (tDistances, Vector3.Distance(tDistances.Position, t.position)))
            .Where(predicate)
            .OrderBy(tDistances => tDistances.Item2)
            .Take(amount)
            .Select(valueTuple => valueTuple.tDistances)
            .ToList();
    }


    /// <summary>
    ///     Pads list with additional specified item
    /// </summary>
    /// <param name="currentList"></param>
    /// <param name="desiredSize">Desired size of new padded list</param>
    /// <param name="padWith">Item to pad list with</param>
    /// <typeparam name="T">Any type</typeparam>
    /// <returns>The current list padded to the specified length, with the specified number</returns>
    public static IEnumerable<T> PadList<T>(this IEnumerable<T> currentList, int desiredSize, T padWith)
    {
        var enumerable = currentList.ToList();
        var count = enumerable.Count();
        return count >= desiredSize ? 
            enumerable : 
            enumerable.Concat(Enumerable.Range(0, desiredSize-count).Select(i => padWith));
    }

    /// <summary>
    ///     Converts list of tuples, to a flat list of plain values, in the same order as the tuple list
    /// </summary>
    /// <param name="currentList"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> FlattenTuples<T>(this IEnumerable<(T, T)> currentList)
    {
        var tList = new List<T>();
        currentList.ToList().ForEach(t =>
        {
            tList.Add(t.Item1);
            tList.Add(t.Item2);
        });
        return tList;
    }
    
    public static float ReverseNormalise(this float init, float max) => init * max;
}