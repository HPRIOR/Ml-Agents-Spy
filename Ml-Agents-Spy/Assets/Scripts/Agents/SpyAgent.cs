using System.Linq;
using Enums;
using Interfaces;
using Training;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using static TileHelper;
using static StaticFunctions;

namespace Agents
{
    public class SpyAgent : AbstractAgent
    {
        // private TrainingInstanceController _instanceController;
        // private readonly IAgentMemoryFactory _agentMemoryFactory =  new AgentMemoryFactory();
        // private IAgentMemory _agentMemory;
        // private float _speed = 10;
        // private float _maxLocalDistance;
        // public float IsColliding { get; private set; }

        /// <summary>
        /// Called at the start of each training episode.
        /// The instance controller needs to be gotten hear due to the call sequence between this method and the restart env delegate
        /// </summary>
        public override void OnEpisodeBegin()
        {
            Constructor();
            if (CompletedEpisodes > 0 )
            {
                _instanceController.Restart();
            }
        }

        //private void Constructor()
        //{
        //    _instanceController = GetComponentInParent<TrainingInstanceController>();
        //    _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
        //    _maxLocalDistance = MaxLocalDistance(_instanceController.AgentMapScale);
        //}

        /// <summary>
        /// This is called at every step - receives actions from policy and is used to give rewards
        /// </summary>
        /// <param name="action"></param>
        public override void OnActionReceived(float[] action)
        {
            AddReward(-1f/MaxStep);
            RewardAndRestartIfExitReached(DistancesToEachExitPoint());
            MoveAgent(action[0]);
        }

        /// <summary>
        /// Gets the distances to each exit point
        /// </summary>
        /// <returns>Float array of the distance to each exit point</returns>
        private float[] DistancesToEachExitPoint() => 
            _instanceController
                .TileDict[TileType.ExitTiles]
                .Select(tile => (tile.Position - transform.position).magnitude)
                .ToArray();
         
        /// <summary>
        /// Sets a reward if the distance any exit point is less than 1
        /// </summary>
        /// <param name="distances">Array of distances to each exit</param>
        private void RewardAndRestartIfExitReached(float[] distances)
        {
            foreach (var magnitude in distances)
                if (magnitude < 1f)
                {
                    SetReward(1f);
                    EndEpisode();
                }
        }


        /// <summary>
        /// Defines one discrete vector [0](1-4) which defines movement in up left right directions
        /// </summary>
        /// <param name="input">action[0] of the discrete action array </param>
        //public void MoveAgent(float input)
        //{
        //    var movementDirection = Vector3.zero;
        //    var action = Mathf.FloorToInt(input);
        //
        //    if (action == 1) movementDirection = transform.forward * 0.5f;
        //    else if (action == 2) movementDirection = transform.forward * -0.5f;
        //    else if (action == 3) movementDirection = transform.right * 0.5f;
        //    else if (action == 4) movementDirection = transform.right * -0.5f;
        //
        //    transform.Translate(movementDirection * Time.fixedDeltaTime * _speed);
        //}

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
            var localPosition = transform.localPosition;
            sensor.AddObservation(NormalisedPositionX());
            sensor.AddObservation(NormalisedPositionY());

            var nearestExitVector = GetNearestTile(
                _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                transform).Position;

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

            //DebugObvs();
        }
        
        /// <summary>
        /// Adds normalised 'trail' of visited locations to observations
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        //private void AddVisitedMemoryTrail(VectorSensor sensor) =>
        //    _agentMemory.GetAgentMemory(transform.localPosition)
        //        .ToList()
        //        .ForEach(f => sensor.AddObservation(StaticFunctions.NormalisedMemoryFloat(
        //            -_maxLocalDistance,
        //            _maxLocalDistance,
        //            f)));

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
            sensor.AddObservation(NearestExitYAxis(nearestExitVector));

        
        /// <summary>
        /// Adds normalised Y axis location of nearest exit to observations 
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        /// <param name="nearestExitVector">Vector of nearest exit</param>
        private void AddNearestExitXAxis(VectorSensor sensor, Vector3 nearestExitVector) =>
            sensor.AddObservation(NearestExitXAxis(nearestExitVector));


        //public float NormalisedPositionX() => 
        //    NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x);
        //
        //public float NormalisedPositionY() => 
        //    NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z);

        private float NearestExitYAxis(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(-_maxLocalDistance,
            _maxLocalDistance, VectorConversions.LocalPosition(
                nearestExitVector, _instanceController).z);

        private float NearestExitXAxis(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(
            -_maxLocalDistance,
            _maxLocalDistance, VectorConversions.LocalPosition(
                nearestExitVector, _instanceController).x);

        public float DistanceToNearestExit(Vector3 nearestExitVector) => 
            StaticFunctions.NormalisedFloat(0f, StaticFunctions.MaxVectorDistanceToExit(_instanceController.AgentMapScale), Vector3.Distance(
                nearestExitVector,
                transform.position));

        


        private void DebugObvs()
        {
            //Agent position
            //Debug.Log($"Agent Position:{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x)}" +
            //         $",{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z)}");
           
            //Distance to nearest Exit
            //Debug.Log("Distance to nearest Exit: " + DistanceToNearestExit( GetNearestTile(
            //   _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
            //   transform).Position));
            
            //NearestExitPosition
            //Debug.Log(NearestExitXAxis(GetNearestTile(
            //    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
            //    transform).Position) + ", " + NearestExitYAxis(GetNearestTile(
            //    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
            //    transform).Position));
            
            // collisions 
            //Debug.Log($"collision: {IsColliding}");
            
        }

        //void OnCollisionEnter(Collision collision) 
        //{
        //    if (collision.gameObject.name == "Cube") IsColliding = 1f;
        //}
//
        //void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.name == "Cube") IsColliding = 0f;
        //}
    }
}
