namespace Interfaces
{
    public interface IAgentMemoryFactory
    {
        IAgentMemory GetAgentMemoryClass(int mapScale);
    }
}