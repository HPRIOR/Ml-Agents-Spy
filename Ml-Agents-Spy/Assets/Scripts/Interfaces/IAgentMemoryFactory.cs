using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentMemoryFactory
{
    IAgentMemory GetAgentMemoryClass();
}
