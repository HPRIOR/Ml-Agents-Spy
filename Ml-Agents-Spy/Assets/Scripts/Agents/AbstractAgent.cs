using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Training;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;

namespace Agents
{
    public abstract  class AbstractAgent : Agent
    {
        protected TrainingInstanceController InstanceController;
        private readonly IAgentMemoryFactory _agentMemoryFactory =  new AgentMemoryFactory();
        private IAgentMemory _agentMemory;
        protected abstract float Speed { get; }
        protected float MaxLocalDistance;
        public float IsColliding { get; private set; }
        
        
        protected void Constructor()
        {
            InstanceController = GetComponentInParent<TrainingInstanceController>();
            _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
            MaxLocalDistance = GetMaxLocalDistance(InstanceController.AgentMapScale);
            
        }
        
        /// <summary>
        /// Defines one discrete vector [0](1-4) which defines movement in up left right directions
        /// </summary>
        /// <param name="input">action[0] of the discrete action array </param>
        public void MoveAgent(float input)
        {
            var movementDirection = Vector3.zero;
            var action = Mathf.FloorToInt(input);

            if (action == 1) movementDirection = transform.forward * 0.5f;
            else if (action == 2) movementDirection = transform.forward * -0.5f;
            else if (action == 3) movementDirection = transform.right * 0.5f;
            else if (action == 4) movementDirection = transform.right * -0.5f;

            
            transform.Translate(movementDirection * Time.fixedDeltaTime * Speed);
        }
        
        
        private List<IEnvTile> GetNearestTiles(int amount, List<IEnvTile> inputTiles) => 
            transform.GetNearestTile(
            amount, 
            inputTiles, 
            x => true);


        // TODO test me
        public List<float> GetNearestTilePositions(int amount, List<IEnvTile> inputTile) =>
            GetNearestTiles(amount, inputTile)
                .Select(tiles =>
                    (NormalisedFloat(
                            -MaxLocalDistance,
                            MaxLocalDistance,
                            VectorConversions.GetLocalPosition(
                                tiles.Position,
                                InstanceController).x),
                        NormalisedFloat(
                            -MaxLocalDistance,
                            MaxLocalDistance,
                            VectorConversions.GetLocalPosition(
                                tiles.Position,
                                InstanceController).z)))
                .FlattenTuples()
                .ToList();
           
        

        protected void AddNearestTilePositions(VectorSensor vectorSensor, int amount, List<IEnvTile> inputTile) =>
            GetNearestTilePositions(amount, inputTile)
                .ForEach(vectorSensor.AddObservation);
        
        

        /// <summary>
        /// Adds normalised 'trail' of visited locations to observations
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        protected void AddVisitedMemoryTrail(VectorSensor sensor) =>
            _agentMemory
                .GetAgentMemory(transform.localPosition)
                .ToList()
                .ForEach(f => sensor.AddObservation(NormalisedMemoryFloat(
                    -MaxLocalDistance,
                    MaxLocalDistance,
                    f)));


        /// <summary>
        /// Gets the nearest guards to the current agent
        /// </summary>
        /// <remarks>
        /// Method overrides determine agent specific logic in getting nearest guards
        /// For guards this is used to exclude themselves from the observation 
        /// </remarks>>
        /// <param name="amount">Number of agents to return</param>
        /// <returns>List of agents (GameObjects)</returns>
        protected abstract List<GameObject> GetNearestGuards(int amount);

        
        // TODO test me
        public List<float> GetGuardPositions(int amount) =>
            GetNearestGuards(amount)
                .Select(guard => {
                Vector3 positions = guard.transform.localPosition;
                return (NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, positions.x),
                    NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, positions.z)); 
                })
                .FlattenTuples()
                .ToList()
                .PadList(amount*2, 0)
                .ToList();
            
        
        
        protected void AddNearestGuards(VectorSensor sensor, int amount)
        {
            GetGuardPositions(amount).ForEach(sensor.AddObservation);
        }
        
        
        // TODO test me
        public float NormalisedPositionX() => NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.x);
        // TODO test me
        public float NormalisedPositionY() => NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.z);
        
        private void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.name == "Cube") IsColliding = 1f;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.name == "Cube") IsColliding = 0f;
        }
        
        
        
        
        
    }
}