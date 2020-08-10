using UnityEngine;

namespace Interfaces
{
    public interface IAgentMemory
    {
        float[] GetAgentMemory(Vector3 agentPosition);
    }
}