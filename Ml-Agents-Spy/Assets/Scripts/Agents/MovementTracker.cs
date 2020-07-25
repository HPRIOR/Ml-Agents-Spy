using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static VectorConversions;

public class MovementTracker : IAgentMemory
{
    private int _memorySize;

    private bool _initialVectorPlaced;

    private float _distanceBetweenNodes;

    private Queue<Vector3> _queue;

    public MovementTracker(int memorySize = 10, float distanceBetweenNodes = 1f)
    {
        _memorySize = memorySize;
        _distanceBetweenNodes = distanceBetweenNodes;
        _queue = new Queue<Vector3>();
    }


    float[] Vector3toFloatArray(List<Vector3> vectors)
    {
        float[] vectorFloats = new float[_memorySize];
        int i = 0;
        foreach (var vector in vectors)
        {
            vectorFloats[i] = vector.x;
            i++;
            vectorFloats[i] = vector.z;
            i++;
        }

        return vectorFloats;
    }

    bool AgentHasMovedEnough(Vector3 agentPosition, Vector3 backOfQueue, float distanceBetweenNodes) =>
        Vector3.Distance(agentPosition, backOfQueue) > distanceBetweenNodes;

    public float[] GetAgentMemory(Vector3 agentPosition)
    {
        if (_initialVectorPlaced)
        {
            if (AgentHasMovedEnough(agentPosition,  _queue.MostRecentlyAdded(), _distanceBetweenNodes))
            {
                if (_queue.Count >= _memorySize/2) _queue.Dequeue();
                _queue.Enqueue(agentPosition);
            }
        }
        else
        {
            _queue.Enqueue(agentPosition);
            _initialVectorPlaced = true;
        }
        return Vector3toFloatArray(_queue.ToArray().ToList());
    }



}
