using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentTravelMemory
{
    float[] GetTileVisitCount();

    Vector2[] GetTileLocations();

    void UpdateAgentPosition(Transform agentPosition);
}
