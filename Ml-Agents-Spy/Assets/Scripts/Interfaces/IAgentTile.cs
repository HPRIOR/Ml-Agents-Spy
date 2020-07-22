﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentTile : ITile
{
    Dictionary<Direction, IAgentTile> AdjacentTile { get; set; }
    int VisitCount { get; set; }
    bool OccupiedByAgent { get; set; }
}
