using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ClassExtensions 
{
    public static List<Tile> CloneTileList(this List<Tile> inputList) =>
        inputList.Select(tile => (Tile)tile.Clone()).ToList();
}
