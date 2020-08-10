using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public static class TileHelper
{
    public static ITile GetNearestTile(List<ITile> tileArray, Transform agentTransform)
    {
        if (tileArray == null) throw new ArgumentNullException(nameof(tileArray));
        var nearestTile = tileArray[0];
        foreach (var tile in tileArray)
        {
            if (Vector3.Distance(tile.Position, agentTransform.position) <
                Vector3.Distance(nearestTile.Position, agentTransform.position))
            {
                nearestTile = tile;
            }
        }

        return nearestTile;
    }
}