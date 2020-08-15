using Interfaces;

namespace Agents
{
    public class AgentMemoryFactory : IAgentMemoryFactory
    {
        public IAgentMemory GetAgentMemoryClass(int mapScale)
        {
            return new MovementTracker(20, mapScale);
        }
    }
}