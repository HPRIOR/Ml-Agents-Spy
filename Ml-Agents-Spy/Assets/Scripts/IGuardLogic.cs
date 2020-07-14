using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuardLogic
{
    bool GuardPlacesAreAvailable();

    void SetGuardTiles();
}
