using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTileConverter : IAgentTileConverter
{
    private IFindAdjacentAgentTile _adjacentTileHelper;
    private List<IEnvTile> _envTiles;

    public AgentTileConverter(List<IEnvTile> envTiles, IFindAdjacentAgentTile adjacentTileHelper)
    {
        _envTiles = envTiles;
        _adjacentTileHelper = adjacentTileHelper;
    }
    public List<IAgentTile> GetAgentTiles()
    {
        throw  new NotImplementedException();
    }




}
