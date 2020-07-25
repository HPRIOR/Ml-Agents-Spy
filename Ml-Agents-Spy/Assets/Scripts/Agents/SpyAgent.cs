using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using static VectorConversions;
using static TileHelper;

public class SpyAgent : Agent
{
    private TrainingInstanceController _instanceController;
    private readonly IAgentMemoryFactory _agentMemoryFactory =  new AgentMemoryFactory();
    private IAgentMemory _agentMemory;
    private float _colliding;
    private float _speed = 10;
    private float _maxLocalDistance;


    public override void OnEpisodeBegin()
    {
        _instanceController = GetComponentInParent<TrainingInstanceController>();
        _agentMemory = _agentMemoryFactory.GetAgentMemoryClass();
        _maxLocalDistance = MaxLocalDistance(_instanceController.MapScale);
        if (CompletedEpisodes > 0) _instanceController.RestartEnv();
    }

    // Called at every step
    public override void OnActionReceived(float[] action)
    {
        AddReward(-1f/MaxStep);

        var distanceToExitPoint = 
            _instanceController
                .TileDict[TileType.ExitTiles]
                .Select(tile => (tile.Position - transform.position).magnitude)
                .ToArray();
        
        SetRewardAndRestartIfExitReached(distanceToExitPoint);
        
        if (transform.position.y < 0f) EndEpisode();
        
        MoveAgent(action[0]);
    }

    public void SetRewardAndRestartIfExitReached(float[] magnitudes)
    {
        foreach (var magnitude in magnitudes)
            if (magnitude < 1f)
            {
                SetReward(1f);
                EndEpisode();
            }
    }
    

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

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        
        if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
        else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
        else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // own position (2 floats)
        sensor.AddObservation(NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.x));
        sensor.AddObservation(NormalisedFloat(-_maxLocalDistance, _maxLocalDistance, transform.localPosition.z));
        

        // position of nearest exit, x axis (1 float)
        sensor.AddObservation(
            NormalisedFloat(
            -_maxLocalDistance, 
            _maxLocalDistance, 
            LocalPosition(
                GetNearestTile(
                    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile)tile), 
                    transform).Position).x));
        
        // position of nearest exit, y axis (1 float)
        sensor.AddObservation(
            NormalisedFloat(
            -_maxLocalDistance,
            _maxLocalDistance,
            LocalPosition(
                GetNearestTile(
                    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile)tile),
                    transform).Position).z));

        // distance to nearest exit (1 float)
        sensor.AddObservation(NormalisedFloat(0f, MaxVectorDistanceToExit(_instanceController.MapScale), Vector3.Distance(
            GetNearestTile(
                    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                    transform)
                .Position,
            transform.position)));


        // colliding with env (1 float)
        sensor.AddObservation(_colliding);

        // trail of visited locations (default = 10)
        _agentMemory.GetAgentMemory(transform.localPosition)
            .ToList()
            .ForEach(f => sensor.AddObservation(
                NormalisedMemoryFloat(
                    -_maxLocalDistance, 
                    _maxLocalDistance, 
                    f)));
    }

    Vector3 LocalPosition(Vector3 position) => position - _instanceController.transform.localPosition;

    float MaxLocalDistance(int mapScale) => mapScale % 2 == 0 ? 
        (mapScale * 5) - 1.3f : 
        (mapScale * 5) - 0.3f;

    float MaxVectorDistanceToExit(int mapScale) => mapScale % 2 == 0 ?
        (mapScale * 10) - 1.4f : 
        (mapScale * 10) + 0.6f;

    float NormalisedFloat(float min, float max, float current) =>
        (current - min) / (max - min);

    float NormalisedMemoryFloat(float min, float max, float current) => current == 0 ? 0f :
        (current - min) / (max - min);

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.name == "Cube") _colliding = 1f;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Cube") _colliding = 0f;
    }



}
