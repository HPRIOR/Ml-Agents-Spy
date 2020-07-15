using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnvSetup
{
    void SetUpEnv();

    List<Tile> GetSpyTile();

    List<Tile> GetGuardTiles();

    List<Tile> GetExitTiles();

}
