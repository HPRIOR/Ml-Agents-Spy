using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentTileConverter : IAgentTileConverter
{
    IFindAdjacentAgentTile _adjacentTileHelper;
    List<IEnvTile> _envTiles;
    List<IAgentTile> GetInitialAgentTiles =>
        _envTiles
            .Select(tile => tile.Clone())
            .ToList()
            .ConvertAll(tile => (EnvTile)tile)
            .ConvertAll(tile => (AgentTile)tile)
            .ConvertAll(tile => (IAgentTile)tile);

    public AgentTileConverter(List<IEnvTile> envTiles, IFindAdjacentAgentTile adjacentTileHelper)
    {
        _envTiles = envTiles;
        _adjacentTileHelper = adjacentTileHelper;
    }

    public List<IAgentTile> GetAgentTiles()
    {
        var agentTiles = GetInitialAgentTiles;
        _adjacentTileHelper.GetAdjacentTiles(agentTiles, _envTiles);
        return agentTiles;
    }
        
    
    

}
