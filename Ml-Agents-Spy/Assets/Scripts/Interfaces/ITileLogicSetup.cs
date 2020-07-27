using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileLogicSetup
{

    IEnvTile[,] GetTileLogic();

    Dictionary<TileType, List<IEnvTile>> GetTileTypes();
    

}
