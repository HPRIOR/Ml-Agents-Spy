using System.Collections.Generic;
using System.Linq;
using Enums;
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
        

        private IEnumerable<(float, float)> GetNormalisedNearestTilePositions(List<IEnvTile> envTiles)
            => envTiles
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
                                InstanceController).z)));
        

        private List<IEnvTile> GetNearestEnvTiles(int amount) =>
            transform.GetNearestTile(
                amount, 
                InstanceController.TileDict[TileType.EnvTiles], 
                x => true);

        // test me
        public List<float> GetNearestEnvTilePositions(int amount)
        {
            var envTiles = GetNearestEnvTiles(amount);
            // envTiles.ForEach(x => Debug.Log(x.Coords));
            var xTiles = GetNormalisedNearestTilePositions(envTiles).ToList();
            var newList = new List<float>();
            for (int i = 0; i < amount; i++)
            {
                newList.Add(xTiles[i].Item1);
                newList.Add(xTiles[i].Item2);
            }
            return newList.ToList();
        }

        protected void AddNearestEnvTilePositions(VectorSensor vectorSensor, int amount) =>
            GetNearestEnvTilePositions(amount)
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
        
        public float NormalisedPositionX() => NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.x);
        
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