using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;

namespace Agents
{
    public class AlertGuardAgent : AbstractGuard
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
            if (CompletedEpisodes > 0) InstanceController.Restart();
            Constructor();
            if (InstanceController.trainingScenario == TrainingScenario.SpyEvade) CanMove = false;
        }

        public (float, float) GetSpyLocalPositions()
        {
            var spyObject = InstanceController.Spy;
            if (spyObject is null)
            {
                return (0, 0);
            }
            var localPosition = spyObject.transform.localPosition;
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

        


        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(InstanceController.AgentMapScale);
            
            // own position (2)
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());

            // spy position (2)
            AddSpyLocalPositions(sensor);

            // position of other guards (6)
            AddNearestGuardPositions(sensor);

            //memory trail (20)
            AddVisitedMemoryTrail(sensor);

            //exits (6)
            var instanceControllerTileDict = InstanceController.TileDict;
            AddNearestTilePositions(sensor, 3, instanceControllerTileDict[TileType.ExitTiles]);

            //env tiles position (12)
            AddNearestTilePositions(sensor, 6, instanceControllerTileDict[TileType.EnvTiles]);
        }

        private void AddNearestGuardPositions(VectorSensor sensor)
        {
            var guardObservationCount = 3;
            if (CanMove)
                AddNearestGuards(sensor, guardObservationCount);
            else
                // observations added with count 
                for (var i = 0; i < guardObservationCount; i++)
                {
                    sensor.AddObservation(0);
                    sensor.AddObservation(0);
                }
        }
        
        private bool CloseToAgent() =>
            Vector3.Distance(AgentRigidbody.position,
                         InstanceController.Spy.GetComponent<Rigidbody>().position) < 1.1f;
        

        public override void OnActionReceived(float[] vectorAction)
        {
            if (CloseToAgent())
            {
                if (InstanceController.trainingScenario == TrainingScenario.SpyEvade)
                {
                    EndEpisode();
                }
                else
                {
                    SetReward(1f);
                    EndEpisode();
                }
            }
            AddReward(-1f / MaxStep);
            if (CanMove) MoveAgent(vectorAction[0]);
        }
    }
}