using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvSetup
{

    int MapScale { get; }
    void SetUpEnv();

    Dictionary<TileType, List<IEnvTile>> GetTileTypes();
    

}
