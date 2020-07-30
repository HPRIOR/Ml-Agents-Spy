using Interfaces;
using Training;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;

namespace Agents
{
    public class GuardAlertAgent: Agent
    {
        [Tooltip("Indicates whether or not agent will be trained in current run")]
        public bool Training;

        private TrainingInstanceController _instanceController;
        private readonly IAgentMemoryFactory _agentMemoryFactory = new AgentMemoryFactory();
        private IAgentMemory _agentMemory;
        private float _maxLocalDistance;

        private int _speed = 5;
        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
        }

        public override void OnEpisodeBegin()
        {
            Debug.Log("AlertGuard Called OnEpisode Begin");
            _instanceController = GetComponentInParent<TrainingInstanceController>();
            _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
            if (CompletedEpisodes > 0 )
            {
                _instanceController.Restart();
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            MoveAgent(vectorAction[0]);
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

    }
}