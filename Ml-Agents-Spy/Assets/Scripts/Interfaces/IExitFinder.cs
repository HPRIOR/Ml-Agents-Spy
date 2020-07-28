namespace Interfaces
{
    public interface IExitFinder
    {
        int ExitCount { get; }
    
        bool CanProceed { get; set; }

        void SetExitTiles();

        bool ExitsAreAvailable();

        void CheckMatrix(IEnvTile[,] tileMatrix);
    }
}