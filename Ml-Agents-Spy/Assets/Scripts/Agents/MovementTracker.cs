using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class MovementTracker : IAgentMemory
    {
        private readonly float _distanceBetweenNodes;
        private readonly int _memorySize;

        private readonly Queue<Vector3> _queue;

        private bool _initialVectorPlaced;

        public MovementTracker(int memorySize = 10, float distanceBetweenNodes = 1f)
        {
            _memorySize = memorySize;
            _distanceBetweenNodes = distanceBetweenNodes;
            _queue = new Queue<Vector3>();
        }

        public float[] GetAgentMemory(Vector3 agentPosition)
        {
            if (_initialVectorPlaced)
            {
                if (AgentHasMovedEnough(agentPosition, _queue.MostRecentlyAdded(), _distanceBetweenNodes))
                {
                    if (_queue.Count >= _memorySize / 2) _queue.Dequeue();
                    _queue.Enqueue(agentPosition);
                }
            }
            else
            {
                _queue.Enqueue(agentPosition);
                _initialVectorPlaced = true;
            }

            return Vector3ToFloatArray(_queue.ToArray().ToList());
        }


        private float[] Vector3ToFloatArray(List<Vector3> vectors)
        {
            var vectorFloats = new float[_memorySize];
            var i = 0;
            foreach (var vector in vectors)
            {
                vectorFloats[i] = vector.x;
                i++;
                vectorFloats[i] = vector.z;
                i++;
            }

            return vectorFloats;
        }

        private bool AgentHasMovedEnough(Vector3 agentPosition, Vector3 backOfQueue, float distanceBetweenNodes)
        {
            // change this the distance has to be greater than x from any node in the queue 
            return Vector3.Distance(agentPosition, backOfQueue) > distanceBetweenNodes;
        }
    }
}