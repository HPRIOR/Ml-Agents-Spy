using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExitHelper
{
    int ExitCount { get; }

    void SetExitTiles();
}