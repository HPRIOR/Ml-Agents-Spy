using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public interface ITile 
{
    Vector3 Position { get; set; }
    (int x, int y) Coords { get; set; }
    string ToString();
}
