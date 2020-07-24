using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static TileHelper; 

public class AgentTravelMemory : IAgentTravelMemory
{
    private List<IAgentTile> _agentTiles;
    private IAgentTile _currentAgentTile;
    private IAgentTile[] _travelMemoryTiles;
    public AgentTravelMemory(List<IAgentTile> agentTiles)
    {
        _agentTiles = agentTiles;
        _currentAgentTile = _agentTiles.First(tile => tile.OccupiedByAgent);
        _travelMemoryTiles = new IAgentTile[8];
    }

    public float[] GetTileVisitCount()
    {
        throw new NotImplementedException();
    }

    public Vector2[] GetTileLocations()
    {
        throw new NotImplementedException();
    }

    public void UpdateAgentPosition(Transform agentPosition)
    {
        var newAgentTile = GetNearestTile(_agentTiles.ConvertAll(tile => (ITile) tile), agentPosition);
        if (newAgentTile.Coords != _currentAgentTile.Coords)
        {
            _currentAgentTile.VisitCount += 1;
            _currentAgentTile = (IAgentTile)newAgentTile;
        }
    }

    List<IAgentTile> GetNearestTiles()
    {
        List<IAgentTile> nearestTiles = new List<IAgentTile>();
        Dictionary<Direction, Stack<IAgentTile>> dictionaryStack = new Dictionary<Direction, Stack<IAgentTile>>();

        foreach (var direction in System.Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
             dictionaryStack[direction] = new Stack<IAgentTile>();
             dictionaryStack[direction].Push(_currentAgentTile.AdjacentTile[direction]);
        }

        while (nearestTiles.Count < 8 && !AllStacksAreNull(dictionaryStack))
        {
            foreach (var directionStack in dictionaryStack)
            {
                var poppedTile = directionStack.Value.Pop();
                if (poppedTile != null)
                {
                    nearestTiles.Add(poppedTile);
                    // get the next tile in the same direction
                    dictionaryStack[directionStack.Key].Push(poppedTile.AdjacentTile[directionStack.Key]);
                }
                else
                {
                    directionStack.Value.Push(null);
                }
            }
        }

        return nearestTiles;

    }

    static bool AllStacksAreNull(Dictionary<Direction, Stack<IAgentTile>> dictionaryStack) 
        => dictionaryStack.All(item => item.Value.Pop() == null);
    


}
