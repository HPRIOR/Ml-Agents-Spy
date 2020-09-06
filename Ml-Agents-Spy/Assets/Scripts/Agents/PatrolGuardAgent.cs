using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Enums;
using Interfaces;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Agents
{
    public class PatrolGuardAgent : AbstractGuard
    {
        private List<IPatrolGuardTile> _currentPatrolTiles = new List<IPatrolGuardTile>{null, null, null};
        private RayPerceptionSensorComponent3D _eyes;
        private GameObject _head;

        private IPatrolGuardTileManager _patrolGuardTileManager;
        public int envTiles;
        public int guardObvs;
        protected override float Speed { get; } = 5;

        private List<float[]> _rayBuffers;

        private readonly float[] _lookBuffer = new float[10];
        private int _bufferCount;
        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
//            actionsOut[1] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
            //else if (Input.GetKey(KeyCode.Q)) actionsOut[1] = 1;
            //else if (Input.GetKey(KeyCode.E)) actionsOut[1] = 2;
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
            if (CompletedEpisodes > 0) InstanceController.Restart();
            Constructor();
            var tileDict = InstanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]).Where(tile => tile.OnPath);
            _patrolGuardTileManager =
                new PatrolGuardTileManager(InstanceController.coroutineSurrogate, freeEnvTiles, transform);
        }

        private void AddNearestPatrolTiles(VectorSensor sensor) =>
            GetNearestPatrolTilePositions().ForEach(sensor.AddObservation);


        private List<float> GetNearestPatrolTilePositions() =>
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
            //map scale
            //sensor.AddObservation(InstanceController.AgentMapScale);
            
            // Own position
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            // rotation of head
//            sensor.AddObservation(StaticFunctions.NormalisedFloat(0, 360,_head.transform.rotation.eulerAngles.y));
            
            // NearestPatrolTile
            AddNearestPatrolTiles(sensor);
            
            

            // NearestGuardAgents
            // if (CanMove)
            //     AddNearestGuards(sensor, guardObvs);
            // else
            //     for (var i = 0; i < guardObvs; i++)
            //     {
            //         sensor.AddObservation(0);
            //         sensor.AddObservation(0);
            //     }

            // TrailMemory
            //AddVisitedMemoryTrail(sensor);

            // nearest env tiles
            //AddNearestTilePositions(sensor, envTiles, InstanceController.TileDict[TileType.EnvTiles]);
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
            
            CheckCurrentTile();
            RayPerceptionOutput.RayOutput[] rayOutputs = 
                RayPerceptionSensor
                    .Perceive(_eyes.GetRayPerceptionInput())
                    .RayOutputs;
            CheckForSpyObservation(rayOutputs);
//            CheckHeadRotation(_head.transform.rotation.eulerAngles.y, vectorAction[0]);
            if (CanMove)
            {
                GetObservationDistances(_rayBuffers, rayOutputs);
                MoveAgent(vectorAction[0]);
                
                // update look buffer
                _lookBuffer[_bufferCount] = vectorAction[0];
                _ = _bufferCount == 9 ? _bufferCount = 0 : _bufferCount += 1;
                
                // rotate head based on maximum number in buffer
                //RotateHead(vectorAction[1]);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    InstanceController.GuardObservations[transform.gameObject][i] = 0;
                }
            }
        }

        private float GetMaxLookBuffer(float[] buffer)
        {
            Dictionary<float, int> bufferCount = new Dictionary<float, int>();
            foreach (var f in buffer)
            {
                if (bufferCount.ContainsKey(f))
                {
                    bufferCount[f] += 1;
                }
                else
                {
                    bufferCount.Add(f, 1);
                }
            }
            return bufferCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        private void ChangeHeadDirection(float direction, GameObject head)
        {
            switch (direction)
            {
                case 1:
                    // up
                    break;
                case 2:
                    // down 
                    break;
                case 3:
                    // right 
                    break;
                case 4:
                    // left
                    break;
                default:
                    break;
            }
        }
            private void CheckHeadRotation(float headRotation, float movementDirection)
        {
            const float reward = -0.0001f;
            switch (movementDirection)
            {
                case 1:
                    if (!(headRotation > 340 || headRotation < 20))
                        SetReward(reward);
                    break;
                case 2:
                    if (!(headRotation > 160 & headRotation < 200)) 
                        SetReward(reward);
                    break;
                case 3:
                    if (!(headRotation > 70 & headRotation < 110))
                        SetReward(reward);
                    break;
                case 4: 
                    if (!(headRotation > 250 & headRotation < 290))
                        SetReward(reward);
                    break;
            }
        }

        private void CheckCurrentTile()
        {
            if (_patrolGuardTileManager.CanRewardAgent(transform))
            {
                SetReward(0.001f);
            }
            _currentPatrolTiles = _patrolGuardTileManager.GetNearestPatrolTile(transform);
        }

        private void CheckForSpyObservation(RayPerceptionOutput.RayOutput[] rayOutputs)
        {
            rayOutputs
                .ToList()
                .ForEach(output =>
                {
                    if (output.HitTagIndex == 0)
                    {
                        var instanceControllerTrainingScenario = InstanceController.trainingScenario;
                        if (instanceControllerTrainingScenario == TrainingScenario.GuardPatrolWithSpy)
                        {
                            EndEpisode();
                        }

                        if (instanceControllerTrainingScenario == TrainingScenario.SpyEvade)
                            InstanceController.SwapAgents();
                    }
                });
        }
        
        private void GetObservationDistances(List<float[]> rayBuffers, RayPerceptionOutput.RayOutput[] rayOutputs)
        {
            for (int i = 0; i < 5; i++)
                rayOutputs[i].ToFloatArray(2, 0, rayBuffers[i]);

            DebugRays();

            var headTransform = _head.transform;
            var headTransformForward = headTransform.forward;
            var position = headTransform.localPosition;
            
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
            var guardObservations = InstanceController.GuardObservations;
            guardObservations[thisGameObject][0] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerLeftPosition.x);
            guardObservations[thisGameObject][1] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerLeftPosition.z);
            guardObservations[thisGameObject][2] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, middleRayPosition.x);
            guardObservations[thisGameObject][3] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, middleRayPosition.z);
            guardObservations[thisGameObject][4] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerRightPosition.x);
            guardObservations[thisGameObject][5] =
                StaticFunctions.NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, outerRightPosition.z);

            //InstanceController.GuardObservations[thisGameObject].ToList().ForEach(x => Debug.Log(x));
        }

        private void DebugRays()
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