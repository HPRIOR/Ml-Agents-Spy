using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SpyAgent : Agent
{
    
    private Transform[] _guardPositions;
    private Rigidbody _agentRigidBody;
    private TrainingInstanceController _instanceController;
    private IEnumerable<Vector3> _exitPositions; 
    private float _speed = 10;

    // called at start - may be used like a constructor
    void Start()
    {
        _agentRigidBody = GetComponent<Rigidbody>();
        _instanceController = GetComponentInParent<TrainingInstanceController>();
        _exitPositions = _instanceController.ExitTiles.Select(tile => tile.Position);
    }
    
    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBeginCalled");
        if (CompletedEpisodes > 0) _instanceController.RestartEnv();
    }


    // Called at every step
    public override void OnActionReceived(float[] action)
    {
        RotateAgent(action[0]);
        MoveAgent(action[1]);



        var distanceToExitPoint = _exitPositions
            .ToList()
            .Select(position => (position - transform.localPosition).magnitude)
            .ToArray();

        //distanceToExitPoint.ToList().ForEach(distance => Debug.Log(distance));

        DistanceCheck( distanceToExitPoint);


    }

    public void DistanceCheck(float[] magnitudes)
    {
        foreach (var magnitude in magnitudes) if (magnitude < 1f) EndEpisode();
    }
    
    
    void RotateAgent(float input)
    {
        var rotateDirection = Vector3.zero;
        var action = Mathf.FloorToInt(input);
        if (action == 1) rotateDirection = transform.up * 1f;
        else if (action == 2) rotateDirection = transform.up * -1f;
        transform.Rotate(rotateDirection, Time.fixedDeltaTime * 200f);
        
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
        actionsOut[1] = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) actionsOut[0] = 2;
        else if (Input.GetKey(KeyCode.RightArrow)) actionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.W)) actionsOut[1] = 1;
        else if (Input.GetKey(KeyCode.S)) actionsOut[1] = 2;
        else if (Input.GetKey(KeyCode.D)) actionsOut[1] = 3;
        else if (Input.GetKey(KeyCode.A)) actionsOut[1] = 4;
    }

    public override void CollectObservations(VectorSensor sensor)
    {   
        //awareness of exits and own position (6 floats)
        _exitPositions
            .ToList()
            .ForEach(sensor.AddObservation);
        
        // awareness of velocity (2 floats )
        sensor.AddObservation(_agentRigidBody.velocity.x);
        sensor.AddObservation(_agentRigidBody.velocity.y);

    }



}
