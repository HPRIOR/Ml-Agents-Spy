using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class FindAdjacentAgentTile : IFindAdjacentAgentTile
{
    public void GetAdjacentTiles(List<IAgentTile> agentTiles, List<IEnvTile> envTiles)
    {
        List<(IAgentTile, IEnvTile)> correspondingTiles = agentTiles
            .OrderBy(tile => tile.Coords.x)
            .ThenBy(tile => tile.Coords.y)
            .Zip(
                envTiles
                    .OrderBy(tile => tile.Coords.x)
                    .ThenBy(tile => tile.Coords.y),
                (agentTile, envTile) => (agentTile, envTile)
            ).ToList();


        /*
         * Loops through the corresponding list of agent and env tiles.
         * For each of the directions in env tiles, the matching env tile is found in the corresponding list
         * when the match is found, the appropriate direction in the env tile adjacency dictionary is given the
         * corresponding agent tile
         */
        foreach (var (correspondingAgentTile, correspondingEnvTile) in correspondingTiles)
        foreach (var keyValue in correspondingEnvTile.AdjacentTile)
        foreach (var (candidateAgentTile, candidateEnvTile) in correspondingTiles)
            if (keyValue.Value == candidateEnvTile) 
                correspondingAgentTile.AdjacentTile[keyValue.Key] = candidateAgentTile;
            

        
           


    }

}
