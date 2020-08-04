using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class MovementTracker : IAgentMemory
    {
        private readonly int _memorySize;

        private bool _initialVectorPlaced;

        private readonly float _distanceBetweenNodes;

        private readonly Queue<Vector3> _queue;

        public MovementTracker(int memorySize = 10, float distanceBetweenNodes = 1f)
        {
            _memorySize = memorySize;
            _distanceBetweenNodes = distanceBetweenNodes;
            _queue = new Queue<Vector3>();
        }


        float[] Vector3ToFloatArray(List<Vector3> vectors)
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
            return Vector3ToFloatArray(_queue.ToArray().ToList());
        }



    }
}
