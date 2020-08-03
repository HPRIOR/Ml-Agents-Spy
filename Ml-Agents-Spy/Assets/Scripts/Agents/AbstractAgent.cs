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
        protected TrainingInstanceController _instanceController;
        protected readonly IAgentMemoryFactory _agentMemoryFactory =  new AgentMemoryFactory();
        protected IAgentMemory _agentMemory;
        protected readonly float _speed = 10;
        protected float _maxLocalDistance;
        public float IsColliding { get; private set; }
        
        
        protected void Constructor()
        {
            _instanceController = GetComponentInParent<TrainingInstanceController>();
            _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
            _maxLocalDistance = MaxLocalDistance(_instanceController.AgentMapScale);
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

            transform.Translate(movementDirection * Time.fixedDeltaTime * _speed);
        }
        
        /// <summary>
        /// Adds normalised 'trail' of visited locations to observations
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        protected void AddVisitedMemoryTrail(VectorSensor sensor) =>
            _agentMemory
                .GetAgentMemory(transform.localPosition)
                .ToList()
                .ForEach(f => sensor.AddObservation(StaticFunctions.NormalisedMemoryFloat(
                    -_maxLocalDistance,
                    _maxLocalDistance,
                    f)));
        
        public float NormalisedPositionX() => NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x);
        
        public float NormalisedPositionY() => NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z);
        
        protected void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.name == "Cube") IsColliding = 1f;
        }

        protected void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.name == "Cube") IsColliding = 0f;
        }
        
    }
}