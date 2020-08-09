using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Training;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;
using static VectorConversions;
using Vector3 = UnityEngine.Vector3;

namespace Agents
{
    public class AlertGuardAgent: AbstractGuard
    {
        protected override float Speed { get; } = 20;
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
            Constructor();
            if (InstanceController.trainingScenario == TrainingScenario.SpyEvade)
            {
                CanMove = false;
            }
            
            if (CompletedEpisodes > 0 )
            {
                InstanceController.Restart();
            }
        }

        public (float, float) GetSpyLocalPositions()
        {
            var localPosition = InstanceController.Spy.transform.localPosition;
            var normalisedLocalPositionX = NormalisedFloat(-MaxLocalDistance, MaxLocalDistance,
                localPosition.x);
            var normalisedLocalPositionY = NormalisedFloat(-MaxLocalDistance, MaxLocalDistance,
                localPosition.z);
            return (normalisedLocalPositionX, normalisedLocalPositionY);
        }
        
        private void AddSpyLocalPositions(VectorSensor sensor)
        {
            var (x, y) = GetSpyLocalPositions();
            sensor.AddObservation(x);
            sensor.AddObservation(y);
        }

        private bool CloseToAgent() => Vector3.Distance(transform.position, InstanceController.Spy.transform.position) < 1.1;


        
        
        

        public override void CollectObservations(VectorSensor sensor)
        {
            // own position (2)
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            // spy position (2)
            AddSpyLocalPositions(sensor);

            // position of other guards (6)
            int guardObservationCount = 3;
            if (CanMove)
            {
                AddNearestGuards(sensor, guardObservationCount);
            }
            else
            {
                for (int i = 0; i < guardObservationCount; i++)
                {
                    sensor.AddObservation(0);
                    sensor.AddObservation(0);
                }
            }
            
            //memory trail (20)
            AddVisitedMemoryTrail(sensor);
            
            //exits (6)
            AddNearestTilePositions(sensor, 3, InstanceController.TileDict[TileType.ExitTiles]);
            
            //env tiles position (12)
            AddNearestTilePositions(sensor, 6, InstanceController.TileDict[TileType.EnvTiles]);
        }
        

        public override void OnActionReceived(float[] vectorAction)
        {
            AddReward(-1f/MaxStep);
            if (CloseToAgent())
            {
                if (InstanceController.trainingScenario == TrainingScenario.SpyEvade)
                {
                    EndEpisode();
                }
                else
                {
                    SetReward(1);
                    EndEpisode();
                }
            }
            if (CanMove) MoveAgent(vectorAction[0]);
        }
        
    }
}