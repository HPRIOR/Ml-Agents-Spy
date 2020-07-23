using System.Collections.Generic;
using UnityEngine;

public interface IFindAdjacentAgentTile
{
    void GetAdjacentTiles(List<IAgentTile> agentTiles, List<IEnvTile> envTiles);
}
