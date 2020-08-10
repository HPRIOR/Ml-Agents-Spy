﻿using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Training;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace Agents
{
    public class PatrolGuardAgent : AbstractGuard
    {
        private List<IPatrolGuardTile> _currentPatrolTiles = new List<IPatrolGuardTile>();
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
        }
        

        private void SetUpRayBuffers()
        {
            var lengthOfRayOutPuts = RayPerceptionSensor
                .Perceive(_eyes.GetRayPerceptionInput())
                .RayOutputs.Length;
            
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
                    if (t is null) return (0, 0); 
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
            //TODO not giving right number of obvs null not working
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

            if (action == 1)
                rotateDirection = _head.transform.up * 1;
            else if (action == 2) rotateDirection = _head.transform.up * -1;

            _head.transform.Rotate(rotateDirection, Time.fixedDeltaTime * 200f);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (_patrolGuardTileManager.CanRewardAgent(transform)) SetReward(0.01f);
            _currentPatrolTiles = _patrolGuardTileManager.GetNearestPatrolTile(transform);
            
            RayPerceptionSensor
                .Perceive(_eyes.GetRayPerceptionInput())
                .RayOutputs
                .ToList()
                .ForEach(output =>
                {
                    FillGuardObservationBuffer(output);
                    if (output.HitTagIndex == 0)
                    {
                        if (InstanceController.trainingScenario == TrainingScenario.GuardPatrolWithSpy)
                        {
                            SetReward(1);
                            EndEpisode();
                        }

                        if (InstanceController.trainingScenario == TrainingScenario.SpyEvade)
                            InstanceController.SwapAgents();
                    }
                });
            if (CanMove)
            {
                MoveAgent(vectorAction[0]);
                RotateHead(vectorAction[1]);
            }
        }

        private void FillGuardObservationBuffer(RayPerceptionOutput.RayOutput output)
        {
            // TODO refactor
            output.ToFloatArray(2, 0, _rayBuffers[0]);
            output.ToFloatArray(2, 1, _rayBuffers[1]);
            output.ToFloatArray(2, 2, _rayBuffers[2]);
            output.ToFloatArray(2, 3, _rayBuffers[3]);
            output.ToFloatArray(2, 4, _rayBuffers[4]);
            var thisGameObject = transform.gameObject;
            InstanceController.GuardObservations[thisGameObject][0] = _rayBuffers[0][3];
            InstanceController.GuardObservations[thisGameObject][1] = _rayBuffers[1][3];
            InstanceController.GuardObservations[thisGameObject][2] = _rayBuffers[2][3];
            InstanceController.GuardObservations[thisGameObject][3] = _rayBuffers[3][3];
            InstanceController.GuardObservations[thisGameObject][4] = _rayBuffers[4][3];
        }
    }
}