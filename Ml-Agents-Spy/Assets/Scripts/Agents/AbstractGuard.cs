using System;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    public abstract class AbstractGuard : AbstractAgent
    {
        /// <summary>
        /// Gets the nearest guards to the current agent
        /// </summary>
        /// <param name="amount">Number of agents to return</param>
        /// <returns>List of agents (GameObjects)</returns>
        private List<GameObject> GetNearestGuards(int amount) =>
            transform
                .gameObject
                .GetNearest(amount,
                    InstanceController.Guards,
                    guard => transform.gameObject.GetInstanceID() != guard.gameObjectDistance.GetInstanceID());


        private float[] GetNearestGuardPositionsY(int amount)
        {
            throw new Exception();
        }
        
        private float[] GetNearestGuardPositionsX(int amount)
        {
            throw new Exception();

        }

        private float[] NearestGuardPositions(int amount)
        {
            if (InstanceController.Guards.Count < amount)
            {
                // add extra 00 on to the list
            }
            else
            { 
                throw new Exception();

            }
            throw new Exception();

        }
        
        
        protected void AddNearestGuardsToAgent(VectorSensor sensor)
        {
            
        }
        
        
    }
}