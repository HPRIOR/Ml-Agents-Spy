using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    public class PatrolGuardAgent : AbstractGuard
    {
        private List<IPatrolGuardTile> _currentPatrolTiles = new List<IPatrolGuardTile>(){null, null, null};
        private RayPerceptionSensorComponent3D _eyes;
        private GameObject _head;

        private IPatrolGuardTileManager _patrolGuardTileManager;
        public int envTiles;
        public int guardObvs;
        protected override float Speed { get; } = 5;

        private List<float[]> _rayBuffers;

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

        private void Awake()
        {
            _head = transform.GetChild(0).gameObject;
            _eyes = _head.GetComponent<RayPerceptionSensorComponent3D>();
            SetUpRayBuffers();
        }
        

        private void SetUpRayBuffers()
        {
            var lengthOfRayOutPuts = RayPerceptionSensor
                .Perceive(_eyes.GetRayPerceptionInput())
                .RayOutputs
                .Length;

            _rayBuffers = new List<float[]>()
            {
                new float[(2 + 2) * lengthOfRayOutPuts],
                new float[(2 + 2) * lengthOfRayOutPuts],
                new float[(2 + 2) * lengthOfRayOutPuts],
                new float[(2 + 2) * lengthOfRayOutPuts],
                new float[(2 + 2) * lengthOfRayOutPuts]
            };
        }
        
        // Some things may be able to move into awake or start if they are not needed every map update 
        public override void OnEpisodeBegin()
        {
            Constructor();
            
            var tileDict = InstanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]);
            _patrolGuardTileManager =
                new PatrolGuardTileManager(InstanceController.coroutineSurrogate, freeEnvTiles, transform);

            if (CompletedEpisodes > 0) InstanceController.Restart();
        }

        private void AddNearestPatrolTiles(VectorSensor sensor) =>
            GetNearestPatrolTilePositions().ForEach(sensor.AddObservation);
           
        


        public List<float> GetNearestPatrolTilePositions() =>
            _currentPatrolTiles
                .Select(t => 
                {
                    if (t == null) return (0, 0); 
                    return (StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance,
                        VectorConversions.GetLocalPosition(t.Position, InstanceController).x),
                    StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance,
                        VectorConversions.GetLocalPosition(t.Position, InstanceController).z)); 
                })
                .FlattenTuples()
                .ToList();
        

        public override void CollectObservations(VectorSensor sensor)
        {
            //TODO mapScale
            
            // Own position
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());

            // NearestPatrolTile
            AddNearestPatrolTiles(sensor);

            // NearestGuardAgents
            if (CanMove)
                AddNearestGuards(sensor, guardObvs);
            else
                for (var i = 0; i < guardObvs; i++)
                {
                    sensor.AddObservation(0);
                    sensor.AddObservation(0);
                }

            // TrailMemory
            AddVisitedMemoryTrail(sensor);

            // nearest env tiles
            AddNearestTilePositions(sensor, envTiles, InstanceController.TileDict[TileType.EnvTiles]);
        }

        private void RotateHead(float input)
        {
            var rotateDirection = Vector3.zero;
            var action = Mathf.FloorToInt(input);

            var headTransform = _head.transform;
            if (action == 1)
            {
                rotateDirection = headTransform.up * 1;
            }
            else if (action == 2)
            {
                rotateDirection = headTransform.up * -1;
                
            }

            headTransform.Rotate(rotateDirection, Time.fixedDeltaTime * 200f);
        }
        

        public override void OnActionReceived(float[] vectorAction)
        {

            CheckCurrentTileReward();

            CheckForSpyObservation();
            
            if (CanMove)
            {
                GetObservationDistances(_rayBuffers);
                MoveAgent(vectorAction[0]);
                RotateHead(vectorAction[1]);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    InstanceController.GuardObservations[transform.gameObject][i] = 0;
                }
            }
        }

        private void CheckCurrentTileReward()
        {
            if (_patrolGuardTileManager.CanRewardAgent(transform)) SetReward(0.01f);
            _currentPatrolTiles = _patrolGuardTileManager.GetNearestPatrolTile(transform);
        }

        private void CheckForSpyObservation()
        {
            RayPerceptionSensor
                .Perceive(_eyes.GetRayPerceptionInput())
                .RayOutputs
                .ToList()
                .ForEach(output =>
                {
                    if (output.HitTagIndex == 0)
                    {
                        var instanceControllerTrainingScenario = InstanceController.trainingScenario;
                        if (instanceControllerTrainingScenario == TrainingScenario.GuardPatrolWithSpy)
                        {
                            SetReward(1);
                            EndEpisode();
                        }

                        if (instanceControllerTrainingScenario == TrainingScenario.SpyEvade)
                            InstanceController.SwapAgents();
                    }
                });
        }


        private void GetObservationDistances(List<float[]> rayBuffers)
        {

            var rayOutputs = RayPerceptionSensor
                .Perceive(_eyes.GetRayPerceptionInput())
                .RayOutputs;
            
            for (int i = 0; i < 5; i++)
                rayOutputs[i].ToFloatArray(2, 0, rayBuffers[i]);

            DebugRays(rayOutputs);

            var headTransform = _head.transform;
            var headTransformForward = headTransform.forward;
            var position = headTransform.position;
            
            var (middleRayPosition, outerRightPosition, outerLeftPosition) 
                = GetRayPosition(rayBuffers, headTransformForward, position);
            //Debug.Log(outerLeftPosition);
            //Debug.Log(middleRayPosition);
            //Debug.Log(outerRightPosition);

            AddNormalisedObservationsToArray(outerLeftPosition, middleRayPosition, outerRightPosition);
        }

        private (Vector3 middleRayPosition, Vector3 outerRightPosition, Vector3 outerLeftPosition) GetRayPosition(
            List<float[]> rayBuffers, Vector3 headTransformForward, Vector3 position)
        {
            var middleRayPosition =
                VectorConversions.RayCastHitLocation(
                    headTransformForward,
                    position,
                    rayBuffers[0][3].ReverseNormalise(_eyes.RayLength) * 0.765f);

            var outerRightPosition =
                VectorConversions.RayCastHitLocation(
                    Quaternion.Euler(0, 15, 0) * headTransformForward,
                    position,
                    rayBuffers[3][3].ReverseNormalise(_eyes.RayLength) * 0.765f);

            var outerLeftPosition =
                VectorConversions.RayCastHitLocation(
                    Quaternion.Euler(0, -15, 0) * headTransformForward,
                    position,
                    rayBuffers[4][3].ReverseNormalise(_eyes.RayLength) * 0.765f);
            return (middleRayPosition, outerRightPosition, outerLeftPosition);
        }

        private void AddNormalisedObservationsToArray(Vector3 outerLeftPosition, Vector3 middleRayPosition,
            Vector3 outerRightPosition)
        {
            var thisGameObject = transform.gameObject;
            InstanceController.GuardObservations[thisGameObject][0] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerLeftPosition.x);
            InstanceController.GuardObservations[thisGameObject][1] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerLeftPosition.z);
            InstanceController.GuardObservations[thisGameObject][2] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, middleRayPosition.x);
            InstanceController.GuardObservations[thisGameObject][3] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, middleRayPosition.z);
            InstanceController.GuardObservations[thisGameObject][4] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerRightPosition.x);
            InstanceController.GuardObservations[thisGameObject][5] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerRightPosition.z);

            // InstanceController.GuardObservations[thisGameObject].ToList().ForEach(x => Debug.Log(x));
        }

        private void DebugRays(RayPerceptionOutput.RayOutput[] rayOutPuts)
        {
            var forward = _eyes.transform.forward;
            var rightMostAngle = Quaternion.Euler(0, 15, 0) * forward;
            var rightMidAngle = Quaternion.Euler(0, 7.5f, 0) * forward;
            var leftMidAngle = Quaternion.Euler(0, -7.5f, 0) * forward;
            var leftMostAngle = Quaternion.Euler(0, -15, 0) * forward;

            var position = _head.transform.position;
            Ray centerRightMost = new Ray(position, rightMostAngle);
            Ray centerRightMid = new Ray(position, rightMidAngle);
            Ray centerRay = new Ray(position, _head.transform.forward);
            Ray centerLeftMid = new Ray(position, leftMidAngle);
            Ray centerLeftMost = new Ray(position, leftMostAngle);


            var eyesRayLength = _eyes.RayLength;
            Debug.DrawLine(position,
                centerRightMost.GetPoint(_rayBuffers[3][3].ReverseNormalise(eyesRayLength) * 0.765f));
            Debug.DrawLine(position, 
                centerRightMid.GetPoint(_rayBuffers[1][3].ReverseNormalise(eyesRayLength) * 0.765f));
            Debug.DrawLine(position, 
                centerRay.GetPoint(_rayBuffers[0][3].ReverseNormalise(eyesRayLength) * 0.765f));
            Debug.DrawLine(position, 
                centerLeftMid.GetPoint(_rayBuffers[2][3].ReverseNormalise(eyesRayLength) * 0.765f));
            Debug.DrawLine(position, 
                centerLeftMost.GetPoint(_rayBuffers[4][3].ReverseNormalise(eyesRayLength) * 0.765f));
        }
        
        

        
    }
}