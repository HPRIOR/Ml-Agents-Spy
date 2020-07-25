using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMemoryFactory :IAgentMemoryFactory
{
    public IAgentMemory GetAgentMemoryClass() => new MovementTracker();
}
