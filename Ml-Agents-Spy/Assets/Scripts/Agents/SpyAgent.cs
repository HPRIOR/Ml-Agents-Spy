using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Agents
{
    public class SpyAgent : AbstractAgent
    {
        protected override float Speed { get; } = 10;

        /// <summary>
        /// Called at the start of each training episode.
        /// </summary>
        public override void OnEpisodeBegin()
        {
            Constructor();
            if (CompletedEpisodes > 0 )
            {
                InstanceController.Restart();
            }
        }
        
        /// <summary>
        /// This is called at every step - receives actions from policy and is used to give rewards
        /// </summary>
        /// <param name="action"></param>
        public override void OnActionReceived(float[] action)
        {
            // small punishment each step (magnitude of entire episode: max cumulative -1)
            AddReward(-1f/MaxStep);
            RewardAndRestartIfExitReached(DistancesToEachExitPoint());
            MoveAgent(action[0]);
        }

        /// <summary>
        /// Gets the distances to each exit point
        /// </summary>
        /// <returns>Float array of the distance to each exit point</returns>
        private float[] DistancesToEachExitPoint() => 
            InstanceController
                .TileDict[TileType.ExitTiles]
                .Select(tile => Vector3.Distance(tile.Position , transform.position))
                .ToArray();
         
        /// <summary>
        /// Sets a reward if the distance any exit point is less than 1
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
        /// defines a means to control agent for debugging purposes
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
        /// Supplies observations to ML algorithm
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            // own position (2 floats)
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());
            
            Vector3 nearestExitVector =
                transform.GetNearestTile(
                    1,
                    InstanceController.TileDict[TileType.ExitTiles],
                    x => true)[0].Position;

            // position of nearest exit, x axis (1 float)
            AddNearestExitXAxis(sensor, nearestExitVector);
            // position of nearest exit, y axis (1 float)
            AddNearestExitYAxis(sensor, nearestExitVector);

            // distance to nearest exit (1 float)
            AddDistanceToNearestExit(sensor, nearestExitVector);

            // colliding with env (1 float)
            sensor.AddObservation(IsColliding);

            // trail of visited locations (20)
            AddVisitedMemoryTrail(sensor);
            
            // 12 floats 
            AddNearestEnvTilePositions(sensor, 6);
            
            // nearest guards (6)
            AddNearestGuards(sensor, 3);
            //DebugObvs();
        }
        
        
        public List<GameObject> GetNearestGuards( int amount) =>
            transform
                .gameObject
                .GetNearest(amount, InstanceController.Guards, x => true);

        private void AddNearestGuards(VectorSensor sensor, int amount)
        {
            GetNearestGuards(3).ForEach(guard =>
            {
                var guardVector = guard.transform.position;
                sensor.AddObservation(
                    StaticFunctions.NormalisedFloat(
                        -MaxLocalDistance,
                        MaxLocalDistance, 
                        guardVector.x)
                );
                sensor.AddObservation(
                    StaticFunctions.NormalisedFloat(
                        -MaxLocalDistance,
                        MaxLocalDistance, 
                        guardVector.z)
                );
            });
            var numberOfGuards = InstanceController.Guards.Count;
            if (amount > numberOfGuards)
            {
                var difference = amount - numberOfGuards;
                for (int i = 0; i < difference; i++)
                {
                    sensor.AddObservation(0);
                    sensor.AddObservation(0);
                }
            }
        }
                

        /// <summary>
        /// Adds normalised distance to nearest exit to observations 
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        /// <param name="nearestExitVector">Vector of nearest exit</param>
        private void AddDistanceToNearestExit(VectorSensor sensor, Vector3 nearestExitVector) =>
            sensor.AddObservation(DistanceToNearestExit(nearestExitVector));

        /// <summary>
        /// Adds normalised X axis location of nearest exit to observations 
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        /// <param name="nearestExitVector">Vector of nearest exit</param>
        private void AddNearestExitYAxis(VectorSensor sensor, Vector3 nearestExitVector) =>
            sensor.AddObservation(NormalisedNearestExitYAxis(nearestExitVector));

        
        /// <summary>
        /// Adds normalised Y axis location of nearest exit to observations 
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        /// <param name="nearestExitVector">Vector of nearest exit</param>
        private void AddNearestExitXAxis(VectorSensor sensor, Vector3 nearestExitVector) =>
            sensor.AddObservation(NormalisedNearestExitXAxis(nearestExitVector));
        

        private float NormalisedNearestExitYAxis(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(-MaxLocalDistance,
            MaxLocalDistance, VectorConversions.GetLocalPosition(
                nearestExitVector, InstanceController).z);

        private float NormalisedNearestExitXAxis(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(
            -MaxLocalDistance,
            MaxLocalDistance, VectorConversions.GetLocalPosition(
                nearestExitVector, InstanceController).x);

        // made public for testing 
        public float DistanceToNearestExit(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(0f, StaticFunctions.MaxVectorDistanceToExit(InstanceController.AgentMapScale), Vector3.Distance(
                nearestExitVector,
                transform.position));
        
        /*
        private void DebugObvs()
        {
            Agent position
            Debug.Log($"Agent Position:{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x)}" +
                     $",{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z)}");
           
            Distance to nearest Exit
            Debug.Log("Distance to nearest Exit: " + DistanceToNearestExit( GetNearestTile(
               _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
               transform).Position));
            
            NearestExitPosition
            Debug.Log(NearestExitXAxis(GetNearestTile(
                _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                transform).Position) + ", " + NearestExitYAxis(GetNearestTile(
                _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                transform).Position));
            
             collisions 
            Debug.Log($"collision: {IsColliding}");
            
        }
        */
        
    }
}
