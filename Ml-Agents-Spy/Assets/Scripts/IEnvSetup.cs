using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvSetup
{
    void SetUpEnv();

    Dictionary<TileType, List<Tile>> GetTileTypes();
    

}
