﻿using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public interface IEnvTile : ITile
{
    Dictionary<Direction, IEnvTile> AdjacentTile { get; set; }
    bool HasSpy { get; set; }
    bool HasGuard { get; set;}
    bool HasEnv { get; set; }
    bool IsExit { get; set; }
    bool OnPath { get; set; }
}
