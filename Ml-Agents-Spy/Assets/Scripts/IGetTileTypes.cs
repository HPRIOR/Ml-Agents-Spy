using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetTileTypes
{
    Dictionary<TileType, List<Tile>> GetTileTypes();
}
