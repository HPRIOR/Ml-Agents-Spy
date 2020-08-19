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
                if (AgentHasMovedEnough(agentPosition, _queue.ToList(), _distanceBetweenNodes))
                {
                    if (_queue.Count >= _memorySize / 2) _queue.Dequeue();
                    _queue.Enqueue(agentPosition);
                    // CreateEnv.CreateBox(new Vector3(0.5f, 0.5f, 0.5f), new RectTransform(), agentPosition);
                }
            }
            else
            {
                _queue.Enqueue(agentPosition);
                _initialVectorPlaced = true;
            }

            return Vector3ToFloatArray(_queue.ToList());
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

        private bool AgentHasMovedEnough(Vector3 agentPosition, IEnumerable<Vector3> queue, float distanceBetweenNodes)
        {
            return queue.All(q => Vector3.Distance(agentPosition, q) > distanceBetweenNodes);
            //return Vector3.Distance(agentPosition, queue) > distanceBetweenNodes;
        }
    }
}