using Interfaces;

namespace Agents
{
    public class AgentMemoryFactory : IAgentMemoryFactory
    {
        public IAgentMemory GetAgentMemoryClass()
        {
            return new MovementTracker(20);
        }
    }
}