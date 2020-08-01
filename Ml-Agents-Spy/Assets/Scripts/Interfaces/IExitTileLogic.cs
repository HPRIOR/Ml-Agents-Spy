namespace Interfaces
{
    public interface IExitTileLogic
    {
        int ExitCount { get; }
    
        bool CanProceed { get; set; }

        void SetExitTiles();

        bool ExitsAreAvailable();

        void CheckMatrix(IEnvTile[,] tileMatrix);
    }
}