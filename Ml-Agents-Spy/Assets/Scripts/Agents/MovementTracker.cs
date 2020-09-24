using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class MovementTracker : IAgentMemory
    {
        /// <summary>
        /// Distance between each node in the memory queue
        /// </summary>
        private readonly float _distanceBetweenNodes;
        /// <summary>
        /// The number of nodes in the memory queue
        /// </summary>
        private readonly int _memorySize;
        /// <summary>
        /// Queue of positions that constitute where the agent has been
        /// </summary>
        private readonly Queue<Vector3> _queue;

        private bool _initialVectorPlaced;

        public MovementTracker(int memorySize = 10, float distanceBetweenNodes = 1f)
        {
            _memorySize = memorySize;
            _distanceBetweenNodes = distanceBetweenNodes;
            _queue = new Queue<Vector3>();
        }

        /// <summary>
        /// Returns a float array of positions where the agent has been.
        /// Will update with new positions if the agent has moved.
        /// </summary>
        /// <param name="agentPosition"></param>
        /// <returns></returns>
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


        /// <summary>
        /// returns 'flattened' array of floats from vector positions
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks to see if the agent has moved far enough away from every node to update the queue
        /// </summary>
        /// <param name="agentPosition"></param>
        /// <param name="queue"></param>
        /// <param name="distanceBetweenNodes"></param>
        /// <returns></returns>
        private bool AgentHasMovedEnough(Vector3 agentPosition, IEnumerable<Vector3> queue, float distanceBetweenNodes) 
            => queue.All(q => Vector3.Distance(agentPosition, q) > distanceBetweenNodes);
        
    }
}