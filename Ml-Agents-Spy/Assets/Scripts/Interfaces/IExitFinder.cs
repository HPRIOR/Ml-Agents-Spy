using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExitFinder
{
    int ExitCount { get; }
    
    bool CanProceed { get; set; }

    void SetExitTiles();

    bool ExitsAreAvailable();

    void CheckMatrix(IEnvTile[,] tileMatrix);
}