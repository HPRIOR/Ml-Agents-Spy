using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ClassExtensions 
{
    public static List<EnvTile> CloneTileList(this List<EnvTile> inputList) =>
        inputList.Select(tile => (EnvTile)tile.Clone()).ToList();

}
