using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using static VectorConversions;
using static TileHelper;

public class SpyAgent : Agent
{
    private TrainingInstanceController _instanceController;
    private float _colliding;
    private float _speed = 10;

    
    public override void OnEpisodeBegin()
    {
        _instanceController = GetComponentInParent<TrainingInstanceController>();
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
        // awareness of own position (2 floats)
        sensor.AddObservation(ConvertToVector2(transform.position));

        // awareness of nearest exit (2 floats)
        sensor.AddObservation(
            ConvertToVector2(
                GetNearestTile(
                    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile)tile),
                    transform)
                    .Position)
            );

        // Distance to nearest exit (1 float)
        sensor.AddObservation(
            Vector3.Distance(
                GetNearestTile(
                    _instanceController.TileDict[TileType.ExitTiles].ConvertAll(tile => (ITile)tile), 
                    transform)
                    .Position, 
                transform.position)
            );

        // colliding with env
        sensor.AddObservation(_colliding);

    }

    


    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.name == "Cube") _colliding = 1f;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Cube") _colliding = 0f;
    }



}
