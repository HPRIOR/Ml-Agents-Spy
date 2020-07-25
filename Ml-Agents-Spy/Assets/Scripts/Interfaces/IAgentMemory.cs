using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.SideChannels;
using UnityEngine;

public interface IAgentMemory
{
    float[] GetAgentMemory(Vector3 agentPosition);
}
