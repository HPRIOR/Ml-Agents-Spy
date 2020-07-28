namespace Interfaces
{
    public interface ITileMatrixProducer
    {
        IEnvTile[,] Tiles { get; }
        object Clone();
    }
}
