using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExitFinder
{
    int ExitCount { get; }

    void SetExitTiles();

    bool ExitsAreAvailable();
}