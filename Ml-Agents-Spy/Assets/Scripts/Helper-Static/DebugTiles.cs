using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using EnvSetup;
using UnityEngine;
using static CreateEnv;

public static class DebugTiles 
{
    /// <summary>
    /// Creates 3D object on first tile matching predicate - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    static void DebugFirstInstance(EnvTile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<EnvTile, bool> tilePredicate) =>
        CreateBox(new Vector3(1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, (
            from EnvTile tile in tileMatrix
            select tile).Where(tilePredicate).ToList()[0].Position);

    /// <summary>
    /// Creates 3D objects on all tiles matching predicates - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    static void DebugAll(EnvTile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<EnvTile, bool> tilePredicate) =>
        (from EnvTile tile in tileMatrix
            select tile).Where(tilePredicate).ToList().ForEach(tile =>
            CreateBox(new Vector3(1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, tile.Position));
}
