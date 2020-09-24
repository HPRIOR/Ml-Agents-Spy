using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agents;
using Enums;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;
using Random = System.Random;

namespace Training
{
    /// <summary>
    ///     This is attached to the TrainingInstance and controls the generation of the env for each instance
    /// </summary>
    public class TrainingInstanceController : MonoBehaviour
    {

        /// <summary>
        ///     Contains GameObjects from scene hierarchy
        /// </summary>
        private Dictionary<ParentObject, GameObject> _parentObjects;

        public Dictionary<GameObject, float[]> GuardObservations { get; set; } = new Dictionary<GameObject, float[]>();
        
        public GameObject topParent;
        public GameObject debugParent;
        public GameObject envParent;
        
        public GameObject coroutineSurrogate;
        
        public GameObject guardAlertPrefab;
        public GameObject guardPatrolPrefab;
        public GameObject spyPrefab;
        
        public CurriculumEnum curriculum;
        public TrainingScenario trainingScenario;
        
        public int mapScale;
        public int exitCount;
        public int guardAgentCount;
        public int mapDifficulty;
        public bool hasMiddleTiles = true;
       
        public Material[] materials;
        
        public bool TestSetUpComplete { get; private set; }
        
        public bool debugSetup;
        public bool waitForTestSetup = true;
        public bool randomiseParameters;
        private bool firstSetup = true;
         
        /// <summary>
        ///     Instantiated spy in the scene
        /// </summary>
        public GameObject Spy { get; private set; }

        /// <summary>
        ///     Instantiated guards in the scene
        /// </summary>
        public List<GameObject> Guards { get; } = new List<GameObject>();

        /// <summary>
        ///     Instantiated guards to spawn during spy evade scenario (between alert and patrol)
        /// </summary>
        public List<GameObject> GuardsSwap { get; } = new List<GameObject>();

        /// <summary>
        ///     Contains the various tiles created during environment setup
        /// </summary>
        public Dictionary<TileType, List<IEnvTile>> TileDict { get; private set; }

        public int AgentMapScale { get; private set; }

        /// <summary>
        ///     Called before the first frame update - used to set up the academy
        /// </summary>
        public void Awake()
        {
            _parentObjects = new Dictionary<ParentObject, GameObject>
            {
                {ParentObject.TopParent, topParent},
                {ParentObject.EnvParent, envParent},
                {ParentObject.DebugParent, debugParent}
            };
            StartCoroutine(WaitUntilAcademyCanProceed());
        }

        private IEnumerator WaitUntilAcademyCanProceed()
        {
            yield return new WaitUntil(() => !waitForTestSetup);
            Academy.Instance.OnEnvironmentReset += InitSetup;
        }


        /// <summary>
        ///     Called once by Academy.Instance at the start of training. Sets up environment, and Spawns agent
        /// </summary>
        public void InitSetup()
        {
            if (firstSetup)
            {
                try
                {
                    ClearChildrenOf(envParent);
                    if (debugSetup) InitDebugSetup();
                    else InitCurrSetup();
                    TestSetUpComplete = true;
                }
                catch (MapCreationException) 
                {
                    //Debug.Log(e);
                    ClearChildrenOf(envParent);
                    InitSetup();
                }
                firstSetup = false;
            }
            
        }

        /// <summary>
        ///     This sets up environment and agents based on parameters given in inspector, or otherwise
        ///       # these can be replaced by restart Env so long Initialise agents is accounted for 
        /// </summary>
        public void InitDebugSetup()
        {
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            var tileLogic = GetTileLogicDebug(tileLogicBuilder);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
            InitialiseAgents(gameParams);
        }

        /// <summary>
        ///     Set up environment with parameters given by the specified curriculum
        /// </summary>
        private void InitCurrSetup()
        {
            Random rand = new Random();
            var curriculumParam = randomiseParameters ? rand.Next(1, 11) :
                Academy.Instance.EnvironmentParameters.GetWithDefault(curriculum.ToString(), 1.0f);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
            InitialiseAgents(gameParams);
        }


        /// <summary>
        ///     Spawns spy and guard agents, and move them into the correct position
        /// </summary>
        /// <param name="gameParams">Parameters of the training scenario (exit count, map scale, difficulty, guard count)</param>
        public void InitialiseAgents(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            SpawnGuardAgent(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount], trainingScenario);
        }

        /// <summary>
        ///     Called by one agent at the end of each episode, resets environment and spawns agents
        /// </summary>
        public void Restart()
        {
            try
            {
                if (debugSetup) DebugRestart();
                else CurriculumRestart();
            }
            catch (MapCreationException)
            {
                ClearChildrenOf(envParent);
                Restart();
            }
        }

        /// <summary>
        ///     Resets environment and agent positions with given parameters, in the inspector or otherwise
        /// </summary>
        private void DebugRestart()
        {
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            ClearChildrenOf(envParent);
            var tileLogic = GetTileLogicDebug(tileLogicBuilder);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
            SetAgentPosition(gameParams);
        }

        /// <summary>
        ///     Resets the environment and agent positions with parameters from curriculum
        /// </summary>
        private void CurriculumRestart()
        {
            Random rand = new Random();
            var curriculumParam = randomiseParameters ? rand.Next(1, 11) :
                Academy.Instance.EnvironmentParameters.GetWithDefault(curriculum.ToString(), 1.0f);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            ClearChildrenOf(envParent);
            var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
            SetAgentPosition(gameParams);
        }


        /// <summary>
        ///     Returns the matrix of tiles which dictates spawning of environment and agents, and the game parameters of the tile
        ///     logic,
        ///     this is based on the game parameters given by curriculum
        /// </summary>
        /// <remarks>
        ///     Two side effects: TileDict, used by agents to know about the current map is updated, and the AgentMapScale
        ///     is updated which is used to normalise the distances of the level by the agents - these need to be updated
        ///     whenever the level changes
        /// </remarks>
        /// <param name="tileLogicBuilder">tile logic builder which organises the classes used to create the tile logic</param>
        /// <param name="gameParams">game parameters from curriculum</param>
        /// <returns>Logic matrix, and map scale</returns>
        private (IEnvTile[,] tileLogic, int gameParamMapScale) GetTileLogicAndGameParamMapScaleCurr(
            ITileLogicBuilder tileLogicBuilder,
            Dictionary<GameParam, int> gameParams)
        {
            var tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            var tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            var gameParamMapScale = gameParams[GameParam.MapScale];
            AgentMapScale = gameParamMapScale;
            return (tileLogic, gameParamMapScale);
        }

        /// <summary>
        ///     Returns tile logic matrix
        /// </summary>
        /// <remarks>
        ///     Two side effects: updates TileDict and AgentMapScale which are used by agents - need to be updated each time map
        ///     changes
        /// </remarks>
        /// >
        /// <param name="tileLogicBuilder">tile logic builder which organises the classes used to create the tile logic</param>
        /// <returns>tile logic matrix</returns>
        private IEnvTile[,] GetTileLogicDebug(ITileLogicBuilder tileLogicBuilder)
        {
            var tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            var tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            AgentMapScale = mapScale;
            return tileLogic;
        }

        /// <summary>
        ///     Spawn agents based on the scenario
        /// </summary>
        /// <param name="gameParams">game parameters</param>
        private void SetAgentPosition(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
        }

        /// <summary>
        ///     Facade injector gets the TileLogicFacade, this facade is then used to interpret the curriculum parameters
        ///     and return the tile logic builder and the game parameters as interpreted by the the facade
        /// </summary>
        /// <param name="curriculumParam">The float given by the Academy curriculum</param>
        /// <returns>Tile logic builder and interpreted game parameters</returns>
        private (ITileLogicBuilder tileLogicBuilder, Dictionary<GameParam, int> gameParams)
            GetTileLogicBuilderAndGameParamsFromCurr(float curriculumParam)
        {
            var facadeInjector = new TileLogicFacadeInjector();
            var logicBuilderFacade = facadeInjector.GetTileLogicFacade(curriculum, curriculumParam);
            var gameParams = logicBuilderFacade.EnvParams;
            var tileLogicBuilder =
                logicBuilderFacade.GetTileLogicBuilder(curriculumParam, _parentObjects);
            return (tileLogicBuilder, gameParams);
        }


        /// <summary>
        ///     Returns the tile builder. There is no need to interpret the parameters given by curriculum - the code calling
        ///     this method already has access to the game parameters. A game parameter dictionary is created anyway
        ///     to encourage code reuse between the curriculum and debug sections of code
        /// </summary>
        /// <returns>tile logic builder and game parameters</returns>
        private (ITileLogicBuilder tileLogicBuilder, Dictionary<GameParam, int> gameParams)
            GetTileLogicBuilderAndGameParamsDebug()
        {
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                mapScale,
                mapDifficulty,
                exitCount,
                guardAgentCount,
                _parentObjects,
                hasMiddleTiles: hasMiddleTiles
            );
            var gameParams = new Dictionary<GameParam, int>
            {
                {GameParam.MapScale, mapScale},
                {GameParam.MapDifficulty, mapDifficulty},
                {GameParam.ExitCount, exitCount},
                {GameParam.GuardAgentCount, guardAgentCount}
            };
            return (tileLogicBuilder, gameParams);
        }

        /// <summary>
        ///     Checks if the spy can spawn based on the training scenario
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool SpyCanSpawn(TrainingScenario inputTrainingScenario)
        {
            return inputTrainingScenario == TrainingScenario.SpyEvade
                   || inputTrainingScenario == TrainingScenario.GuardAlert
                   || inputTrainingScenario == TrainingScenario.SpyPathFinding
                   || inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy;
        }

        /// <summary>
        ///     Checks if training scenario wants a patrol guard
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool TrainingScenarioWantsPatrol(TrainingScenario inputTrainingScenario)
        {
            return inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy
                   || inputTrainingScenario == TrainingScenario.GuardPatrol
                   || inputTrainingScenario == TrainingScenario.SpyEvade;
        }

        /// <summary>
        ///     Checks if training scenario wants an alert
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool TrainingScenarioWantsAlert(TrainingScenario inputTrainingScenario)
        {
            return inputTrainingScenario == TrainingScenario.GuardAlert;
        }


        /// <summary>
        ///     Adds guards to the list of guards property, will only spawn up to the number of exits - 1
        /// </summary>
        /// <param name="numberOfGuards"></param>
        /// <param name="inputExitCount"></param>
        /// <param name="inputTrainingScenario"></param>
        public void SpawnGuardAgent(int numberOfGuards, int inputExitCount, TrainingScenario inputTrainingScenario)
        {
            if (TileDict[TileType.GuardTiles].Count < numberOfGuards)
                throw new MapCreationException("Number of guards has exceeded the number of spawn places");

            var maxNumOfGuard = MaxNumberOfGuards(numberOfGuards, inputExitCount);

            var indexes =
                RandomHelper.GetUniqueRandomList(maxNumOfGuard,
                    TileDict[TileType.GuardTiles].Count);
            
            for (var i = 0; i < maxNumOfGuard; i++)
                if (inputTrainingScenario != TrainingScenario.SpyEvade)
                {
                    if (TrainingScenarioWantsPatrol(inputTrainingScenario))
                    {
                        var instantiatedPatrolPrefab = 
                            Instantiate(guardPatrolPrefab,
                            TileDict[TileType.GuardTiles][indexes[i]].Position,
                            Quaternion.identity, transform);
                        
                        Guards.Add(instantiatedPatrolPrefab);
                        GuardObservations.Add(instantiatedPatrolPrefab, new float[6]);
                    }

                    if (TrainingScenarioWantsAlert(inputTrainingScenario))
                    {
                        var freeTiles = TileDict[TileType.GuardTiles]
                            .Concat(TileDict[TileType.FreeTiles])
                            .Where(tile => tile.OnPath)
                            .ToList();
                        
                        var agentIndex = RandomHelper.GetUniqueRandomList(maxNumOfGuard,
                    freeTiles.Count);
                        
                        var instantiatedAlertPrefab = 
                            Instantiate(guardAlertPrefab,
                                freeTiles[agentIndex[i]].Position,
                                Quaternion.identity, transform);
                        
                        Guards.Add(instantiatedAlertPrefab);
                        GuardObservations.Add(instantiatedAlertPrefab, new float[6]);
                    }
                        
                }
                else
                {
                    {
                        var instantiatedPatrolPrefab = 
                            Instantiate(guardPatrolPrefab,
                                TileDict[TileType.GuardTiles][indexes[i]].Position,
                                Quaternion.identity, transform);

                        Guards.Add(instantiatedPatrolPrefab);
                        GuardObservations.Add(instantiatedPatrolPrefab, new float[6]);
                        
                        GuardsSwap.Add(Instantiate(
                                guardAlertPrefab,
                                TileDict[TileType.GuardTiles][indexes[i]].Position - new Vector3(0, 100, 0),
                                Quaternion.identity,
                                transform
                            )
                        );
                    }
                }
        }
        
        
        

        /// <summary>
        ///     Spawns guard at a random guard spawn tile
        /// </summary>
        /// <param name="potentialSpawnTiles"></param>
        /// <param name="gameParams"></param>
        private void MoveGuardAgents(List<IEnvTile> potentialSpawnTiles, Dictionary<GameParam, int> gameParams)
        {
            if (trainingScenario == TrainingScenario.SpyEvade)
                if (Guards.Any(guard => guard.CompareTag("alertguard")))
                    SwapAgents();
            if (potentialSpawnTiles.Count < Guards.Count)
                throw new MapCreationException("Number of guards has exceeded the number of spawn places");
            var indexes =
                RandomHelper.GetUniqueRandomList(
                    MaxNumberOfGuards(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount]),
                    potentialSpawnTiles.Count);
            for (var i = 0; i < Guards.Count; i++)
            {
                if (trainingScenario != TrainingScenario.GuardAlert)
                {
                    Guards[i].transform.position = potentialSpawnTiles[indexes[i]].Position;
                    Guards[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    
                    var freeTiles = TileDict[TileType.GuardTiles]
                        .Concat(TileDict[TileType.FreeTiles])
                        .Where(tile => tile.OnPath)
                        .ToList();
                    var maxNumOfGuard = MaxNumberOfGuards(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount]); 
                    var agentIndex = RandomHelper.GetUniqueRandomList(maxNumOfGuard,
                    freeTiles.Count);
                        
                    Guards[i].transform.position = freeTiles[agentIndex[i]].Position;
                    Guards[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }

        /// <summary>
        ///     Used to get the possible number of agents in each training scenario
        ///     three possible combinations, both spy and guard, just spy, just guards
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <param name="guardCount"></param>
        /// <returns></returns>
        private static int NumberOfAgentsIn(TrainingScenario inputTrainingScenario, int guardCount)
        {
            switch (inputTrainingScenario)
            {
                case TrainingScenario.GuardAlert:
                case TrainingScenario.GuardPatrolWithSpy:
                    return 1 + guardCount;
                case TrainingScenario.SpyPathFinding:
                    return 1;
                case TrainingScenario.GuardPatrol:
                    return guardCount;
                case TrainingScenario.SpyEvade:
                    return 1 + guardCount * 2;
                default:
                    return guardCount + 1;
            }
        }

        /// <summary>
        ///     will return the maximum number of guards that the training scenario will allow for
        /// </summary>
        /// <param name="guardCount"></param>
        /// <param name="exitCount"></param>
        /// <returns></returns>
        private static int MaxNumberOfGuards(int guardCount, int exitCount) =>
            guardCount >= exitCount ? exitCount - 1 : guardCount;
      

        /// <summary>
        ///     Clears children of given GameObject in hierarchy
        /// </summary>
        /// <param name="parentObject">Parent GameObject</param>
        private void ClearChildrenOf(GameObject parentObject)
        {
            foreach (Transform child in parentObject.transform) Destroy(child.gameObject);
        }

        /// <summary>
        ///     Checks if SpyPrefab has been made - if not then it instantiates one, otherwise its position is set
        ///     at the given spawn tile from the tile dictionary (delete me and change debug mode)
        /// </summary>
        private void SpawnSpyAgent()
        {
            if (Spy is null)
            {
                Spy = Instantiate(spyPrefab, TileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                Spy.transform.parent = transform;
            }
            else
            {
                Spy.transform.position = TileDict[TileType.SpyTile][0].Position;
                Spy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Swaps patrol for alert guard
        /// </summary>
        public void SwapAgents()
        {
            Guards
                .Zip(GuardsSwap, Tuple.Create)
                .ToList()
                .ForEach(t =>
                {
                    var (inPlay, outPlay) = t;
                    Rigidbody outPlayRb = outPlay.GetComponent<Rigidbody>();
                    Rigidbody inPlayRb = inPlay.GetComponent<Rigidbody>();

                    outPlayRb.isKinematic = true;
                    inPlayRb.isKinematic = true;
                    
                    //move agents
                    outPlayRb.transform.position = inPlay.transform.position;
                    inPlayRb.transform.position -=  new Vector3(0, 100, 0);
                    
                    outPlayRb.isKinematic = false;
                    inPlayRb.isKinematic = false;
                    
                    // stop their movement
                    outPlayRb.velocity = Vector3.zero;
                    inPlayRb.velocity = Vector3.zero;
                    //swap lists
                    Guards.Remove(inPlay);
                    Guards.Add(outPlay);
                    GuardsSwap.Remove(outPlay);
                    GuardsSwap.Add(inPlay);
                    
                    // change Can Move
                    inPlay.GetComponent<AbstractGuard>().CanMove =false;
                    outPlay.GetComponent<AbstractGuard>().CanMove = true;
                    
                });
        }
        
    }
}