using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static TileHelper;

public class AgentTileManager
{
    List<IAgentTile> _agentTiles;
    private IAgentTravelMemory _travelMemory;

    public AgentTileManager(IAgentTileConverter tileConverter, IAgentTravelMemory travelMemory)
    {
        _agentTiles = tileConverter.GetAgentTiles();
        _travelMemory = travelMemory;
    }

    public void UpdateAgentPosition(Vector3 agentPosition)
    {
        // count etc
    }
    
    
    // zip values together
    public (float, Vector2)[] GetTileMemory() => throw new NotImplementedException();

    


}
