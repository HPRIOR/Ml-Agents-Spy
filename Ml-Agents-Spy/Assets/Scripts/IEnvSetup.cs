using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnvSetup
{
    void SetUpEnv();

    Dictionary<TileType, List<Tile>> GetTileTypes();
    

}
