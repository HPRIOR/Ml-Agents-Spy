using Interfaces;

namespace Agents
{
    public class AgentMemoryFactory : IAgentMemoryFactory
    {
        /// <summary>
        /// Gets the class used to give an agent persistent memory with specified distance between nodes.
        /// </summary>
        /// <param name="distanceBetweenNodes"></param>
        /// <returns></returns>
        public IAgentMemory GetAgentMemoryClass(int distanceBetweenNodes)
        {
            return new MovementTracker(40, distanceBetweenNodes);
        }
    }
}