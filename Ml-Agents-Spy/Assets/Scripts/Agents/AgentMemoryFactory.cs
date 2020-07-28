using Interfaces;

namespace Agents
{
    public class AgentMemoryFactory :IAgentMemoryFactory
    {
        public IAgentMemory GetAgentMemoryClass() => new MovementTracker();
    }
}
