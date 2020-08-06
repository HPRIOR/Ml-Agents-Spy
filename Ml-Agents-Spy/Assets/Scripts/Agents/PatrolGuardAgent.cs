using System.Linq;
using Enums;
using Interfaces;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Agents
{
    public class PatrolGuardAgent : AbstractGuard
    {
        public int guardObvs;
        public int envTiles;
        
        private IPatrolGuardTileManager _patrolGuardTileManager;
        protected override float Speed { get; } = 5;
        private GameObject _head;
        private RayPerceptionSensorComponent3D _eyes;
        private IPatrolGuardTile _currentPatrolTile;

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
            actionsOut[1] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
            else if (Input.GetKey(KeyCode.Q)) actionsOut[1] = 1;
            else if (Input.GetKey(KeyCode.E)) actionsOut[1] = 2;
        }
        
        // Some things may be able to move into awake or start if they are not needed every map update 
        public override void OnEpisodeBegin()
        {
            Constructor();

            _head = transform.GetChild(0).gameObject;
            _eyes = _head.GetComponent<RayPerceptionSensorComponent3D>();

            var tileDict = InstanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]);
            _patrolGuardTileManager = new PatrolGuardTileManager(InstanceController.coroutineSurrogate, freeEnvTiles, transform);
           
            if (CompletedEpisodes > 0 )
            {
                InstanceController.Restart();
            }
        }

        private void AddNearestPatrolTiles(VectorSensor sensor)
        {
            var nearestPatrolTiles = GetNearestPatrolTiles();
            sensor.AddObservation(nearestPatrolTiles.Item1);
            sensor.AddObservation(nearestPatrolTiles.Item2);
        }
        
        public (float, float) GetNearestPatrolTiles()
        {
            if (_currentPatrolTile is null)
            {
                return (0, 0);
            }
            var patrolTilePosition = _currentPatrolTile.Position;
            var normalisedPositionX = 
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, 
                    VectorConversions.GetLocalPosition(patrolTilePosition, InstanceController).x);
            var normalisedPositionY = 
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, 
                    VectorConversions.GetLocalPosition(patrolTilePosition, InstanceController).z);
            // Debug.Log($"{normalisedPositionX},{normalisedPositionY}");
            return (normalisedPositionX, normalisedPositionY);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // Own position
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            // NearestPatrolTile
            AddNearestPatrolTiles(sensor);
            
            // NearestGuardAgents
            AddNearestGuards(sensor, guardObvs);
            
            // TrailMemory
            AddVisitedMemoryTrail(sensor);
            
            // nearest env tiles
            AddNearestEnvTilePositions(sensor, envTiles);
        }
        

        private void RotateHead(float input)
        {

            var rotateDirection = Vector3.zero;
            var action = Mathf.FloorToInt(input);
            
            if (action == 1)
            {
                rotateDirection = _head.transform.up * 1;
            }
            else if (action == 2)
            {
                rotateDirection =  _head.transform.up * -1;
            }

            _head.transform.Rotate(rotateDirection, Time.fixedDeltaTime * 200f);

        }
        
        public override void OnActionReceived(float[] vectorAction)
        {
            if (_patrolGuardTileManager.CanRewardAgent(transform))
            {
                SetReward(0.01f);
            }
            _currentPatrolTile = _patrolGuardTileManager.GetNearestPatrolTile(transform);
            
            
            RayPerceptionSensor.Perceive(_eyes.GetRayPerceptionInput()).RayOutputs.ToList().ForEach(output =>
            {
                if (output.HitTaggedObject)
                {
                    if (InstanceController.trainingScenario == TrainingScenario.GuardPatrolWithSpy)
                    {
                        SetReward(1);
                        EndEpisode();
                    }

                    if (InstanceController.trainingScenario == TrainingScenario.SpyEvade)
                    {
                        InstanceController.SwapPatrolForAlert();
                    }
                }
            });
                
                
            MoveAgent(vectorAction[0]);
            RotateHead(vectorAction[1]);
        }
    }
}
