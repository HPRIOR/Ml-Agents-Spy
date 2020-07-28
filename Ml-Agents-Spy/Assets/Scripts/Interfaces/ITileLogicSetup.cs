using System.Collections.Generic;
using Enums;

namespace Interfaces
{
    public interface ITileLogicSetup
    {

        IEnvTile[,] GetTileLogic();

        Dictionary<TileType, List<IEnvTile>> GetTileTypes();
    

    }
}
