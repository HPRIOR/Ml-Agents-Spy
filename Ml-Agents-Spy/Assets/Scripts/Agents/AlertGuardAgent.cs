using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
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

       
        // test me
        public List<(float, float)> NearestExitTilePositions(int amount)
            => transform
                .GetNearestTile( amount, 
                InstanceController.TileDict[TileType.ExitTiles],
                x=>true)
                .Select(tile =>
                {
                    var localPosition = GetLocalPosition(tile.Position, InstanceController);
                    var x = NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, localPosition.x);
                    var y = NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, localPosition.z);
                    return (x, y);
                })
                .ToList();
        
        private static void AddExitPositions(VectorSensor sensor, List<(float, float)> nearestExits)
        {
            nearestExits.ForEach(t =>
            {
                sensor.AddObservation(t.Item1);
                sensor.AddObservation(t.Item2);
            });
        }

        private void AddNearestExitTilePositions(VectorSensor sensor, int amount)
        {
            var nearestExits = NearestExitTilePositions(amount);
            var requestedExitCount = nearestExits.Count;
            var exitCount = InstanceController.TileDict[TileType.ExitTiles].Count;
            
            if (exitCount < requestedExitCount)
            {
                AddExitPositions(sensor, nearestExits);
                int leftOver = requestedExitCount - exitCount;
                for (int i = 0; i < leftOver; i++)
                {
                    sensor.AddObservation(0);
                    sensor.AddObservation(0);
                }
            }
            else
            {
                AddExitPositions(sensor, nearestExits);
            }
            
        }

        

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
            AddNearestExitTilePositions(sensor, 3);
            
            //env tiles position (12)
            AddNearestEnvTilePositions(sensor, 6);
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