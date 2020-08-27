using Interfaces;

namespace Agents
{
    public class AgentMemoryFactory : IAgentMemoryFactory
    {
        public IAgentMemory GetAgentMemoryClass(int distanceBetweenNodes)
        {
            return new MovementTracker(40, distanceBetweenNodes);
        }
    }
}