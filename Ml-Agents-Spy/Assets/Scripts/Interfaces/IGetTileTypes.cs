using System.Collections;
using System.Collections.Generic;
using EnvSetup;
using UnityEngine;

public interface IGetTileTypes
{
    Dictionary<TileType, List<EnvTile>> GetTileTypes();
}
