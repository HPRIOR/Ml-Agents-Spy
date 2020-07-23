using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTravelMemory : IAgentTravelMemory
{
    public AgentTravelMemory(List<IAgentTile> agentTiles)
    {

    }

    public float[] GetTileVisitCount()
    {
        throw new NotImplementedException();
    }

    public Vector2[] GetTileLocations()
    {
        throw new NotImplementedException();
    }
}
