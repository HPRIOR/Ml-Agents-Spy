namespace Interfaces
{
    public interface IGuardTileLogic
    {
        void GetMaxExitCount(int maxExitCount);

        void GetPotentialGuardPlaces(IEnvTile[,] tiles);

        bool GuardPlacesAreAvailable();

        void SetGuardTiles();
    }
}