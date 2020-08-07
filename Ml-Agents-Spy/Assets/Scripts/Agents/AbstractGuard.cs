using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    // this may be incorped into abstract agent because the agent will need to use this 
    public abstract class AbstractGuard : AbstractAgent
    {
        public bool CanMove { get; set; } = true;
        
        /// <summary>
        /// Gets the nearest guards to the current agent
        /// </summary>
        /// <param name="amount">Number of agents to return</param>
        /// <returns>List of agents (GameObjects)</returns>
        private List<GameObject> GetNearestGuards(int amount) =>
            transform
                .gameObject
                .GetNearest(
                    amount,
                    InstanceController.Guards,
                    guard => 
                        transform.gameObject.GetInstanceID() != guard.gameObjectDistance.GetInstanceID());


        private List<(float, float)> GetNearestGuardPositions(int amount) =>
            GetNearestGuards(amount).Select(guard =>
            {
               Vector3 positions = guard.transform.localPosition;
               return (positions.x, positions.z);
            }).ToList();


        // test me
        public List<float> NormaliseGuardPositions(int amount)
        {
            List<float> normalisedPositions = new List<float>();
            GetNearestGuardPositions(amount).ForEach(t =>
            {
                // Debug.Log($"{StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, t.Item1)}, {StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, t.Item2)}");
                normalisedPositions.Add(StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, t.Item1));
                normalisedPositions.Add(StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, t.Item2));
            });
            int numberOfGuardsInScene = InstanceController.Guards.Count - 1;
            if (numberOfGuardsInScene < amount)
            {
                int leftOver = amount - numberOfGuardsInScene;
                for (int i = 0; i < leftOver; i++)
                {
                    normalisedPositions.Add(0);
                    normalisedPositions.Add(0);
                }
            }
            return normalisedPositions;
        }
        
        
        protected void AddNearestGuards(VectorSensor sensor, int amount)
        {
            NormaliseGuardPositions(amount).ForEach(sensor.AddObservation);
        }

    }
}