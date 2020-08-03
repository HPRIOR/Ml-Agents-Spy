using System.Linq;
using System.Runtime.CompilerServices;
using Enums;
using Interfaces;
using Training;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;

namespace Agents
{
    public class GuardPatrolAgent : Agent
    {
        private TrainingInstanceController _instanceController;
        private IPatrolGuardTileManager _patrolGuardTileManager;
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
            _instanceController = GetComponentInParent<TrainingInstanceController>();
            var tileDict = _instanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]);
            _patrolGuardTileManager = new PatrolGuardTileManager(_instanceController.coroutineSurrogate, freeEnvTiles);
            _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
            _maxLocalDistance = MaxLocalDistance(_instanceController.AgentMapScale);
            if (CompletedEpisodes > 0 )
            {
                _instanceController.Restart();
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            //sensor.AddObservation(_instanceController.SpyPrefabClone.transform.position);
            //Debug.Log(Vector3.Distance(transform.position, _instanceController.SpyPrefabClone.transform.position));
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            //RewardAndRestartIfExitReached(DistancesToEachExitPoint());
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
        public void RewardAndRestartIfExitReached(float[] distances)
        {
            foreach (var magnitude in distances)
                if (magnitude < 1f)
                {
                    SetReward(1f);
                    EndEpisode();
                }
        }
        /// <summary>
        /// Gets the distances to each exit point
        /// </summary>
        /// <returns>Float array of the distance to each exit point</returns>
        private float[] DistancesToEachExitPoint() => 
            _instanceController
                .TileDict[TileType.ExitTiles]
                .Select(tile => (tile.Position - transform.position).magnitude)
                .ToArray();
    }
}
