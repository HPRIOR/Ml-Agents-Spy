using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class FindAdjacentAgentTile : IFindAdjacentAgentTile
{
    
    public  void GetAdjacentTiles(List<IAgentTile> agentTiles, int matrixSize)
    {
        foreach (var tile in agentTiles)
        {
            UpdateDirectionDictionary(tile, agentTiles, matrixSize);
        }
    }


    static void  UpdateDirectionDictionary(IAgentTile tile, List<IAgentTile> tileMatrix, int matrixSize)
    {
        System.Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .ToList()
            .ForEach(direction => tile.AdjacentTile[direction] = GetDirectionTile(direction, tile, tileMatrix, matrixSize));
    }


    static IAgentTile GetDirectionTile(Direction d, IAgentTile inputEnvTile, List<IAgentTile> tileMatrix, int matrixSize)
     {
        if (d == Direction.N) return inputEnvTile.Coords.y == matrixSize ? null : FindDirectionTile(tileMatrix, inputEnvTile.Coords.x, inputEnvTile.Coords.y + 1);
        if (d == Direction.E) return inputEnvTile.Coords.x == matrixSize ? null : FindDirectionTile(tileMatrix, inputEnvTile.Coords.x + 1, inputEnvTile.Coords.y);
        if (d == Direction.S) return inputEnvTile.Coords.y == 0 ? null : FindDirectionTile(tileMatrix, inputEnvTile.Coords.x, inputEnvTile.Coords.y - 1);
        if (d == Direction.W) return inputEnvTile.Coords.x == 0 ? null : FindDirectionTile(tileMatrix, inputEnvTile.Coords.x - 1, inputEnvTile.Coords.y);
        throw new Exception("No direction given in TileMatrix.GetDirectionTile");
     }

    static IAgentTile FindDirectionTile(List<IAgentTile> tile, int x, int y) => tile.First(t => t.Coords == (x, y));


}
