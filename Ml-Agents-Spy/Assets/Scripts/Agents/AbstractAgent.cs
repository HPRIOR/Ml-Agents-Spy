﻿using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Training;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static StaticFunctions;

namespace Agents
{
    public abstract class AbstractAgent : Agent
    {
        private readonly IAgentMemoryFactory _agentMemoryFactory = new AgentMemoryFactory();
        private IAgentMemory _agentMemory;
        protected Rigidbody AgentRigidbody;
        protected TrainingInstanceController InstanceController;
        protected float MaxLocalDistance;
        protected abstract float Speed { get;}
        protected bool HasSubscribed { get;  set; } = false;
        

        /// <summary>
        /// Instantiates various parts of the agent each training episode
        /// Resets or Gets the agents memory class
        /// Gets the max local distance of the current map
        /// </summary>
        protected void Constructor()
        {
            
            InstanceController = GetComponentInParent<TrainingInstanceController>();
            AgentRigidbody = GetComponent<Rigidbody>();
            _agentMemory = _agentMemoryFactory.GetAgentMemoryClass(2);
            MaxLocalDistance = GetMaxLocalDistance(5);
        }

        protected abstract void MustBeCalledAnyEpisodeBegin();
       


        protected  void SubscribeToOtherAgents()
        {
            IEnumerable<GameObject> allAgents;
             // subscribe to other Alert agents 
             if (InstanceController.trainingScenario == TrainingScenario.GuardPatrol)
             {
                allAgents =  InstanceController.GuardsSwap
                                              .Concat(InstanceController.Guards);
             }
             else
             {
                 allAgents = InstanceController.GuardsSwap
                     .Concat(InstanceController.Guards).Append(InstanceController.Spy);
             }
             
             allAgents.ToList().ForEach(agent =>
             {
                 if (agent.GetInstanceID() != GetInstanceID())
                 {
                     var spy = agent.GetComponent<SpyAgent>();
                     var alert = agent.GetComponent<AlertGuardAgent>();
                     var patrol = agent.GetComponent<PatrolGuardAgent>();
                     if (spy != null)
                     {
                         //Debug.Log("Subscribed to spy agent");
                         spy.SpyEpisodeBegin += MustBeCalledAnyEpisodeBegin;
                     }
                     
                     if (alert != null)
                     {
                         //Debug.Log("Subscribed to alert agent");
                         alert.AlertEpisodeBegin += MustBeCalledAnyEpisodeBegin;
                     }
                     
                     if (patrol != null)
                     {
                         //Debug.Log("Subscribed to patrol agent");
                         patrol.PatrolEpisodeBegin += MustBeCalledAnyEpisodeBegin;
                     }
                 }
             });
             HasSubscribed = true;
        }
        
        
        /// <summary>
        ///     Defines one discrete vector [0](1-4) which defines movement in up left right directions
        /// </summary>
        /// <param name="input">action[0] of the discrete action array </param>
        public void MoveAgent(float input)
        {
            var movementDirection = Vector3.zero;
            var action = Mathf.FloorToInt(input);

            if (action == 1) movementDirection = transform.forward * 0.5f;
            else if (action == 2) movementDirection = transform.forward * -0.5f;
            else if (action == 3) movementDirection = transform.right * 0.5f;
            else if (action == 4) movementDirection = transform.right * -0.5f;

            //transform.Translate(movementDirection * Time.fixedDeltaTime * Speed);
            AgentRigidbody.MovePosition(transform.position + movementDirection * Time.fixedDeltaTime * Speed);
        }

        private List<IEnvTile> GetNearestTiles(int amount, List<IEnvTile> inputTiles) =>
            transform.GetNearestTile(
                amount,
                inputTiles,
                x => true);
        
        public List<float> GetNearestTilePositions(int amount, List<IEnvTile> inputTile) => 
            GetNearestTiles(amount, inputTile)
                .Select(tiles =>
                    (NormalisedFloat(
                            -MaxLocalDistance,
                            MaxLocalDistance,
                            VectorConversions.GetLocalPosition(
                                tiles.Position,
                                InstanceController).x),
                        NormalisedFloat(
                            -MaxLocalDistance,
                            MaxLocalDistance,
                            VectorConversions.GetLocalPosition(
                                tiles.Position,
                                InstanceController).z)))
                .FlattenTuples()
                .PadList(amount * 2, 0)
                .ToList();
        

        protected void AddNearestTilePositions(VectorSensor vectorSensor, int amount, List<IEnvTile> inputTile) => 
            GetNearestTilePositions(amount, inputTile)
                .ForEach(vectorSensor.AddObservation);
        


        /// <summary>
        ///     Adds normalised 'trail' of visited locations to observations
        /// </summary>
        /// <param name="sensor">Sensor used to pass observations</param>
        protected void AddVisitedMemoryTrail(VectorSensor sensor) =>
            _agentMemory
                .GetAgentMemory(transform.localPosition)
                .ToList()
                .ForEach(f => sensor.AddObservation(NormalisedMemoryFloat(
                    -MaxLocalDistance,
                    MaxLocalDistance,
                    f)));
        

        /// <summary>
        ///     Gets the nearest guards to the current agent
        /// </summary>
        /// <remarks>
        ///     Method overrides determine agent specific logic in getting nearest guards
        ///     For guards this is used to exclude themselves from the observation
        /// </remarks>
        /// >
        /// <param name="amount">Number of agents to return</param>
        /// <returns>List of agents (GameObjects)</returns>
        protected abstract List<GameObject> GetNearestGuards(int amount);
        
        public List<float> GetGuardPositions(int amount) 
            => GetNearestGuards(amount)
                .Select(guard =>
                {
                    var positions = guard.transform.localPosition;
                    return (NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, positions.x),
                        NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, positions.z));
                })
                .FlattenTuples()
                .PadList(amount * 2, 0)
                .ToList();
        

        protected void AddNearestGuards(VectorSensor sensor, int amount) =>
            GetGuardPositions(amount).ForEach(sensor.AddObservation);
        
        
        public float NormalisedPositionX() =>
            NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.x);
        
        public float NormalisedPositionY() => 
            NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.z);
        
    }
}