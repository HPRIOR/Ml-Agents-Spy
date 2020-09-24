using System.Collections.Generic;
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

        /// <summary>
        /// overriden by each agent - called when other agents events are fired   
        /// </summary>
        protected abstract void MustBeCalledAnyEpisodeBegin();
       
        /// <summary>
        /// This subscribes the agents to other agents events which occur at the start of each episode.
        /// This is required to set other agents observations at the start of training - OnEpisodeBegin only called
        /// by triggered agent.
        /// </summary>
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

        /// <summary>
        /// Gets the nearest tiles to the game current gameobject
        /// </summary>
        /// <param name="amount">The number of tiles to retrieve</param>
        /// <param name="inputTiles">The tiles to choose from</param>
        /// <returns></returns>
        private List<IEnvTile> GetNearestTiles(int amount, List<IEnvTile> inputTiles) =>
            transform.GetNearestTile(
                amount,
                inputTiles,
                x => true);
        
        /// <summary>
        /// Gets the positions of the tiles selected by GetNearestTiles.
        /// Will pad the list with zeros if number of tiles to choose from is less than the requested amount
        /// </summary>
        /// <param name="amount">the number of tiles to get their position retrieved</param>
        /// <param name="inputTile">The tiles to choose from</param>
        /// <returns>Normalised list of floats</returns>
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
        

        /// <summary>
        /// Adds the retrieved positions to a vector sensor - this is used in the agent to add observations
        /// </summary>
        /// <param name="vectorSensor">Class used to add observations to</param>
        /// <param name="amount">number of tiles to add to vector sensor</param>
        /// <param name="inputTile">The list of tiles to select from </param>
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
        
        /// <summary>
        /// Adds nearest guard positions to Vector sensor
        /// </summary>
        /// <param name="sensor">Sensor to add observations to</param>
        /// <param name="amount"></param>
        protected void AddNearestGuards(VectorSensor sensor, int amount) =>
            GetGuardPositions(amount).ForEach(sensor.AddObservation);
        
        
        /// <summary>
        /// Returns a normalised float of the gameobjects position on X axis
        /// </summary>
        /// <returns></returns>
        public float NormalisedPositionX() =>
            NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.x);
        
        /// <summary>
        /// Returns a normalised float of the gameobjects position on Y axis
        /// </summary>
        /// <returns></returns>
        public float NormalisedPositionY() => 
            NormalisedFloat(-MaxLocalDistance, MaxLocalDistance, transform.localPosition.z);
        
    }
}