using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileMatrixProducer
{
    IEnvTile[,] Tiles { get; }
    object Clone();
}
