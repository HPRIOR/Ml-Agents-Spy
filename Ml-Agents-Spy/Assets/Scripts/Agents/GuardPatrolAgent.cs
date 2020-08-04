using System.Collections;
using System.ComponentModel;
using System.Linq;
using Enums;
using Interfaces;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    public class GuardPatrolAgent : AbstractAgent
    {
        
        private IPatrolGuardTileManager _patrolGuardTileManager;
        protected override float Speed { get; } = 5;
        private GameObject _head;
        private RayPerceptionSensorComponent3D _eyes;
        private int _currentHeadRotation = 0;

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0;
            if (Input.GetKey(KeyCode.W)) actionsOut[0] = 1;
            else if (Input.GetKey(KeyCode.S)) actionsOut[0] = 2;
            else if (Input.GetKey(KeyCode.D)) actionsOut[0] = 3;
            else if (Input.GetKey(KeyCode.A)) actionsOut[0] = 4;
            else if (Input.GetKey(KeyCode.Q)) actionsOut[1] = 1;
            else if (Input.GetKey(KeyCode.E)) actionsOut[1] = 2;
        }

        public override void OnEpisodeBegin()
        {
            Constructor();

            _head = transform.GetChild(0).gameObject;
            _eyes = _head.GetComponent<RayPerceptionSensorComponent3D>();

            var tileDict = InstanceController.TileDict;
            var freeEnvTiles =
                tileDict[TileType.FreeTiles]
                    .Concat(tileDict[TileType.GuardTiles])
                    .Concat(tileDict[TileType.SpyTile]);
            _patrolGuardTileManager = new PatrolGuardTileManager(InstanceController.coroutineSurrogate, freeEnvTiles);
           
            if (CompletedEpisodes > 0 )
            {
                InstanceController.Restart();
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            //sensor.AddObservation(_instanceController.SpyPrefabClone.transform.position);
            //Debug.Log(Vector3.Distance(transform.position, _instanceController.SpyPrefabClone.transform.position));
        }

        private void RotateHead(float input)
        {
            Debug.Log(_head.transform.rotation);
            
            var rotateDirection = Quaternion.identity;
            
            var action = Mathf.FloorToInt(input);
            if (action == 1) rotateDirection = Quaternion.AngleAxis(360, Vector3.up);
            else if (action == 2) rotateDirection = Quaternion.AngleAxis(180, Vector3.up);
            
            _head.transform.rotation = rotateDirection;
            
        }

        
        
        public override void OnActionReceived(float[] vectorAction)
        {
            MoveAgent(vectorAction[0]);
            RotateHead(vectorAction[1]);
        }
        
        
        
        
    }

    
}
