using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    void GetPath(IEnvTile startEnvTile);
}
