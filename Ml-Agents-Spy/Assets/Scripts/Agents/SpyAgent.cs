using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using static TileHelper;
using static StaticFunctions;

public class SpyAgent : Agent
{
    private TrainingInstanceController _instanceController;
    private readonly IAgentMemoryFactory _agentMemoryFactory =  new AgentMemoryFactory();
    private IAgentMemory _agentMemory;
    private float _colliding;
    private float _speed = 10;
    private float _maxLocalDistance;

    /// <summary>
    /// Called at the start of each training episode.
    /// The instance controller needs to be gotten hear due to the call sequence between this method and the restart env delegate
    /// </summary>
    public override void OnEpisodeBegin()
    {
        _instanceController = GetComponentInParent<TrainingInstanceController>();
        _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
        _maxLocalDistance = MaxLocalDistance(_instanceController.AgentMapScale);
        if (CompletedEpisodes > 0) _instanceController.RestartEnv();
    }

    /// <summary>
    /// This is called at every step - receives actions from policy and is used to give rewards
    /// </summary>
    /// <param name="action"></param>
    public override void OnActionReceived(float[] action)
    {
        AddReward(-1f/MaxStep);
        SetRewardAndRestartIfExitReached(DistancesToEachExitPoint());
        if (transform.position.y < 0f) EndEpisode();
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
    public void SetRewardAndRestartIfExitReached(float[] distances)
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
    void MoveAgent(float input)
    {
        var movementDirection = Vector3.zero;
        var action = Mathf.FloorToInt(input);

        if (action == 1) movementDirection = transform.forward * 0.5f;
        else if (action == 2) movementDirection = transform.forward * -0.5f;
        else if (action == 3) movementDirection = transform.right * 0.5f;
        else if (action == 4) movementDirection = transform.right * -0.5f;

       transform.Translate(movementDirection * Time.fixedDeltaTime * _speed);
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
        sensor.AddObservation(NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x));
        sensor.AddObservation(NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z));

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
        sensor.AddObservation(_colliding);

        // trail of visited locations (default = 10)
        AddVisitedMemoryTrail(sensor);

        DebugObvs();
    }

    void DebugObvs()
    {
        // Debug.Log($"Agent Position:{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x)}" +
        //           $",{NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z)}");



    }

    /// <summary>
    /// Adds normalised 'trail' of visited locations to observations
    /// </summary>
    /// <param name="sensor">Sensor used to pass observations</param>
    private void AddVisitedMemoryTrail(VectorSensor sensor) =>
        _agentMemory.GetAgentMemory(transform.localPosition)
            .ToList()
            .ForEach(f => sensor.AddObservation(StaticFunctions.NormalisedMemoryFloat(
                    -_maxLocalDistance,
                    _maxLocalDistance,
                    f)));

    /// <summary>
    /// Adds normalised distance to nearest exit to observations 
    /// </summary>
    /// <param name="sensor">Sensor used to pass observations</param>
    /// <param name="nearestExitVector">Vector of nearest exit</param>
    private void AddDistanceToNearestExit(VectorSensor sensor, Vector3 nearestExitVector) =>
        sensor.AddObservation(StaticFunctions.NormalisedFloat(0f, StaticFunctions.MaxVectorDistanceToExit(_instanceController.AgentMapScale), Vector3.Distance(
            nearestExitVector,
            transform.position)));

    /// <summary>
    /// Adds normalised X axis location of nearest exit to observations 
    /// </summary>
    /// <param name="sensor">Sensor used to pass observations</param>
    /// <param name="nearestExitVector">Vector of nearest exit</param>
    private void AddNearestExitYAxis(VectorSensor sensor, Vector3 nearestExitVector) =>
        sensor.AddObservation(StaticFunctions.NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, VectorConversions.LocalPosition(
            nearestExitVector, _instanceController).z));

    /// <summary>
    /// Adds normalised Y axis location of nearest exit to observations 
    /// </summary>
    /// <param name="sensor">Sensor used to pass observations</param>
    /// <param name="nearestExitVector">Vector of nearest exit</param>
    private void AddNearestExitXAxis(VectorSensor sensor, Vector3 nearestExitVector) =>
        sensor.AddObservation(StaticFunctions.NormalisedFloat(
                -_maxLocalDistance,
                _maxLocalDistance, VectorConversions.LocalPosition(
                    nearestExitVector, _instanceController).x));
    

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.name == "Cube") _colliding = 1f;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Cube") _colliding = 0f;
    }
}
