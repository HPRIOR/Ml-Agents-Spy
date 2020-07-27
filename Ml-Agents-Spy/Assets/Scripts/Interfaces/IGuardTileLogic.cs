using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuardTileLogic
{

    void GetMaxExitCount(int maxExitCount);

    void GetPotentialGuardPlaces(IEnvTile[,] tiles);

    bool GuardPlacesAreAvailable();

    void SetGuardTiles();
}
