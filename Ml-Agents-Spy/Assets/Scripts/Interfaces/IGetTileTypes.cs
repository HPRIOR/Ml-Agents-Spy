using System.Collections.Generic;
using Enums;
using EnvSetup;

namespace Interfaces
{
    public interface IGetTileTypes
    {
        Dictionary<TileType, List<EnvTile>> GetTileTypes();
    }
}
