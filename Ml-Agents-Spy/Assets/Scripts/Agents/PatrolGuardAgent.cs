using System;
using System.Collections.Generic;
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

        public GameObject middleLine;
        public GameObject outerRightLine;
        public GameObject outerLeftLine;
        public GameObject innerLeftLine;
        public GameObject innerRightLine;
        
        private LineRenderer _middleLine;
        private LineRenderer _outerRightLine;
        private LineRenderer _outerLeftLine;
        private LineRenderer _innerLeftLine;
        private LineRenderer _innerRightLine;
        
        
        private IPatrolGuardTileManager _patrolGuardTileManager;
        protected override float Speed { get; } = 5;
        private List<float[]> _rayBuffers;
        private readonly float[] _lookBuffer = new float[15];
        private int _bufferCount;

        public delegate void PatrolEpisodeBeginHandler();
        public event PatrolEpisodeBeginHandler PatrolEpisodeBegin;
        /// <summary>
        /// Allows movement to be overriden by keyboard - for debugging
        /// </summary>
        /// <param name="actionsOut"></param>
        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
        }
        
        /// <summary>
        /// Does setup stuff when the agent is first created
        /// </summary>
        private void Awake()
        {
            ConfigureLineRenderer();
            _head = transform.GetChild(0).gameObject;
            _eyes = _head.GetComponent<RayPerceptionSensorComponent3D>();
            SetUpRayBuffers();
        }

        /// <summary>
        /// Gets the components for each line renderer, applies material to them, and set's their width
        /// </summary>
        private void ConfigureLineRenderer()
        {
            _middleLine = middleLine.GetComponent<LineRenderer>();
            _outerRightLine = outerRightLine.GetComponent<LineRenderer>();
            _outerLeftLine = outerLeftLine.GetComponent<LineRenderer>();
            _innerLeftLine = innerLeftLine.GetComponent<LineRenderer>();
            _innerRightLine = innerRightLine.GetComponent<LineRenderer>();

            _middleLine.material = new Material(Shader.Find("Sprites/Default"));
            _outerLeftLine.material = new Material(Shader.Find("Sprites/Default"));
            _outerRightLine.material = new Material(Shader.Find("Sprites/Default"));
            _innerLeftLine.material = new Material(Shader.Find("Sprites/Default"));
            _innerRightLine.material = new Material(Shader.Find("Sprites/Default"));

            _middleLine.startWidth = 0.1f;
            _outerLeftLine.startWidth = 0.1f;
            _outerRightLine.startWidth = 0.1f;
            _innerLeftLine.startWidth = 0.1f;
            _innerRightLine.startWidth = 0.1f;

        }


        /// <summary>
        /// Sets up arrays which can be used to store vector information from the ray perception sensor
        /// </summary>
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
        
        public override void OnEpisodeBegin()
        {
            MustBeCalledAnyEpisodeBegin();
            //Debug.Log("Patrol has called on episode being");
            PatrolEpisodeBegin?.Invoke();
            if (CompletedEpisodes > 0) InstanceController.Restart();
            if (!HasSubscribed) SubscribeToOtherAgents();
        }
       
        protected override void MustBeCalledAnyEpisodeBegin()
        {
            //Debug.Log("Patrol has called MustBeCalled");
            Constructor();
            var tileDict = InstanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]).Where(tile => tile.OnPath);
            _patrolGuardTileManager =
                new PatrolGuardTileManager(InstanceController.coroutineSurrogate, freeEnvTiles, transform);
        }


        /// <summary>
        /// Adds the nearest patrol tiles to the vector sensor
        /// </summary>
        /// <param name="sensor"></param>
        private void AddNearestPatrolTiles(VectorSensor sensor) =>
            GetNearestPatrolTilePositions().ForEach(sensor.AddObservation);

        /// <summary>
        /// Gets the positions of the nearest patrol tiles 
        /// </summary>
        /// <returns></returns>
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
            // Own position
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            // NearestPatrolTile
            AddNearestPatrolTiles(sensor);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            CheckCurrentTile();
            RayPerceptionOutput.RayOutput[] rayOutputs = 
                RayPerceptionSensor
                    .Perceive(_eyes.GetRayPerceptionInput())
                    .RayOutputs;
            CheckForSpyObservation(rayOutputs);
            if (CanMove)
            {
                GetObservationDistances(_rayBuffers, rayOutputs);
                MoveAgent(vectorAction[0]);
                
                // update look buffer
                _lookBuffer[_bufferCount] = vectorAction[0];
                _ = _bufferCount == 9 ? _bufferCount = 0 : _bufferCount += 1;
                
                // rotate head based on maximum number in buffer
                ChangeHeadDirection(GetMaxLookBuffer(_lookBuffer));
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    InstanceController.GuardObservations[transform.gameObject][i] = 0;
                }
            }
        }

        /// <summary>
        /// Gets the number occuring most in the input array. Used to get the look direction of the agent.
        /// </summary>
        /// <param name="buffer">Array which holds the most recent movement directions of the agent</param>
        /// <returns></returns>
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

        /// <summary>
        /// Changes the rotation of the agents eyes based on the input float
        /// </summary>
        /// <param name="direction"></param>
        private void ChangeHeadDirection(float direction)
        {
            switch (direction)
            {
                case 1:
                    // up
                    _head.transform.transform.rotation = Quaternion.Euler(0,0,0);
                    break;
                case 2:
                    // down 
                    _head.transform.transform.rotation = Quaternion.Euler(0,180,0);
                    break;
                case 3:
                    // right 
                    _head.transform.transform.rotation = Quaternion.Euler(0,90,0);
                    break;
                case 4:
                    // left
                    _head.transform.transform.rotation = Quaternion.Euler(0,270,0);
                    break;
            }
        }
        
        /// <summary>
        /// Retrieves information from the patrol guard tile manager as to whether the agent has moved to a new tile and
        /// rewards accordingly
        /// </summary>
        private void CheckCurrentTile()
        {
            if (_patrolGuardTileManager.CanRewardAgent(transform))
            {
                SetReward(0.001f);
            }
            _currentPatrolTiles = _patrolGuardTileManager.GetNearestPatrolTile(transform);
        }

        /// <summary>
        /// Checks if on of the rays omitted by the guard has hit
        /// </summary>
        /// <param name="rayOutputs"></param>
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
                            InstanceController.Spy.GetComponent<SpyAgent>().EndEpisode();
                        }

                        if (instanceControllerTrainingScenario == TrainingScenario.SpyEvade)
                            InstanceController.SwapAgents();
                    }
                });
        }
        
        /// <summary>
        /// Gets the distance between the rays origin and the hit location for each ommited ray
        /// </summary>
        /// <param name="rayBuffers"></param>
        /// <param name="rayOutputs"></param>
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

        /// <summary>
        /// Returns the hit location for the outer rays, and the middle ray.
        /// This is used by the Spy to observe the Patrol guards line of sight.
        /// </summary>
        /// <param name="rayBuffers"></param>
        /// <param name="headTransformForward"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds normalised vector information to dictionary in instance controller -
        /// this is used by the Spy to get the observations of the nearest guards
        /// </summary>
        /// <param name="outerLeftPosition"></param>
        /// <param name="middleRayPosition"></param>
        /// <param name="outerRightPosition"></param>
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

            _middleLine.SetPosition(0, position);
            _middleLine.SetPosition(1, centerRay.GetPoint(_rayBuffers[0][3].ReverseNormalise(eyesRayLength) * 0.765f));

            _outerLeftLine.SetPosition(0, position);
            _outerLeftLine.SetPosition(1, centerLeftMost.GetPoint(_rayBuffers[4][3].ReverseNormalise(eyesRayLength) * 0.765f)); 
            
            _outerRightLine.SetPosition(0, position);
            _outerRightLine.SetPosition(1, centerRightMost.GetPoint(_rayBuffers[3][3].ReverseNormalise(eyesRayLength) * 0.765f)); 
            
            _innerLeftLine.SetPosition(0, position);
            _innerLeftLine.SetPosition(1, centerLeftMid.GetPoint(_rayBuffers[2][3].ReverseNormalise(eyesRayLength) * 0.765f)); 
            
            _innerRightLine.SetPosition(0, position);
            _innerRightLine.SetPosition(1, centerRightMid.GetPoint(_rayBuffers[1][3].ReverseNormalise(eyesRayLength) * 0.765f)); 
            
            
            
            //Debug.DrawLine(position,
            //    centerRightMost.GetPoint(_rayBuffers[3][3].ReverseNormalise(eyesRayLength) * 0.765f));
            //Debug.DrawLine(position, 
            //    centerRightMid.GetPoint(_rayBuffers[1][3].ReverseNormalise(eyesRayLength) * 0.765f));
            //Debug.DrawLine(position, 
            //    centerRay.GetPoint(_rayBuffers[0][3].ReverseNormalise(eyesRayLength) * 0.765f));
            //Debug.DrawLine(position, 
            //    centerLeftMid.GetPoint(_rayBuffers[2][3].ReverseNormalise(eyesRayLength) * 0.765f));
            //Debug.DrawLine(position, 
            //    centerLeftMost.GetPoint(_rayBuffers[4][3].ReverseNormalise(eyesRayLength) * 0.765f));
        }
    }
}