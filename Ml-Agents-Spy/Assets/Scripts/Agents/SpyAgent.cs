using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    public class SpyAgent : AbstractAgent
    {
        protected override float Speed { get; } = 10;

        private IEnumerable<IEnvTile> _freeTiles;

        public delegate void SpyEpisodeBeginHandler();

        public event SpyEpisodeBeginHandler SpyEpisodeBegin;

        /// <summary>
        ///     Called at the start of each training episode.
        /// </summary>
        public override void OnEpisodeBegin()
        {
            MustBeCalledAnyEpisodeBegin();
            SpyEpisodeBegin?.Invoke();
            //Debug.Log("Spy called OnEpisodeBegin");
            if (CompletedEpisodes > 0) InstanceController.Restart();
            if (!HasSubscribed) SubscribeToOtherAgents();
        }

        protected override void MustBeCalledAnyEpisodeBegin()
        {
            //Debug.Log("Spy Has Called Must be Called"); 
            Constructor();
            Dictionary<TileType,List<IEnvTile>> instanceControllerTileDict = InstanceController.TileDict;
            _freeTiles
                = instanceControllerTileDict[TileType.FreeTiles]
                    .Concat(instanceControllerTileDict[TileType.GuardTiles])
                    .Concat(instanceControllerTileDict[TileType.SpyTile]).Where(tile => tile.OnPath);
        }

        /// <summary>
        ///     This is called at every step - receives actions from policy and is used to give rewards
        /// </summary>
        /// <param name="action"></param>
        public override void OnActionReceived(float[] action)
        {
            // small punishment each step (magnitude of entire episode: max cumulative -1)
            // 5000 is an arbitrarily high number so that rewards are homogenised when testing different max steps 
            AddReward(-1f / MaxStep);
            RewardAndRestartIfExitReached(DistancesToEachExitPoint());
            MoveAgent(action[0]);
        }

        /// <summary>
        ///     Gets the distances to each exit point
        /// </summary>
        /// <returns>Float array of the distance to each exit point</returns>
        private float[] DistancesToEachExitPoint() =>
            InstanceController
                .TileDict[TileType.ExitTiles]
                .Select(tile => Vector3.Distance(tile.Position, transform.position))
                .ToArray();
        

        /// <summary>
        ///     Sets a reward if the distance any exit point is less than 1
        /// </summary>
        /// <param name="distances">Array of distances to each exit</param>
        private void RewardAndRestartIfExitReached(float[] distances)
        {
            foreach (var distance in distances)
                if (distance < 1f)
                {
                    SetReward(1f);
                    EndEpisode();
                }
        }

        /// <summary>
        ///     defines a means to control agent for debugging purposes
        /// </summary>
        /// <param name="actionsOut">Discrete array</param>
        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
        }

        /// <summary>
        ///     Supplies observations to ML algorithm
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            Dictionary<TileType,List<IEnvTile>> instanceControllerTileDict = InstanceController.TileDict;
            
            // own position (2 floats)
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            var nearestExitVector =
                transform.GetNearestTile(
                    1,
                    instanceControllerTileDict[TileType.ExitTiles],
                    x => true)[0].Position;
            // distance to nearest exit (1 float)
            AddDistanceToNearestExit(sensor, nearestExitVector);

            // position of nearest exit (6 floats)
            AddNearestTilePositions(sensor, 3, instanceControllerTileDict[TileType.ExitTiles]);

            // position of nearest input tiles (20 floats)
            AddNearestTilePositions(sensor, 10, _freeTiles.ToList());

            int requestedGuardObvs = 3;
            // nearest guards (6)
            AddNearestGuards(sensor, requestedGuardObvs);

            // 6 per guard 
            AddGuardObservations(sensor, requestedGuardObvs);
            
            // trail of visited locations (40)
            AddVisitedMemoryTrail(sensor);
        }

        private void AddGuardObservations(VectorSensor sensor, int requestedGuardObvs)
        {
            try
            {
                var nearestGuards = GetNearestGuards(requestedGuardObvs);
                nearestGuards.ForEach(guard =>
                {
                    for (int i = 0; i < 6; i++)
                    {
                        sensor.AddObservation(InstanceController.GuardObservations[guard][i]);
                    }
                });
                var nearestGuardsCount = nearestGuards.Count;
                if (requestedGuardObvs > nearestGuardsCount)
                {
                    int difference = requestedGuardObvs - nearestGuardsCount;
                    for (int i = 0; i < difference; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            sensor.AddObservation(0);
                        }
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                for (int i = 0; i < requestedGuardObvs; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        sensor.AddObservation(0);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets all nearest guards to the current spy agent
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        protected override List<GameObject> GetNearestGuards(int amount) => 
            transform
                .gameObject
                .GetNearest(amount, InstanceController.Guards, x => true);
        

        /// <summary>
        ///     Adds normalised distance to nearest exit to observations
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        /// <param name="nearestExitVector">Vector of nearest exit</param>
        private void AddDistanceToNearestExit(VectorSensor sensor, Vector3 nearestExitVector)
        {
            sensor.AddObservation(DistanceToNearestExit(nearestExitVector));
        }

        // made public for testing 
        public float DistanceToNearestExit(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(0f, 
                StaticFunctions.MaxVectorDistanceToExit(InstanceController.AgentMapScale), Vector3.Distance(
                    nearestExitVector, 
                    transform.position));
        
    }
}