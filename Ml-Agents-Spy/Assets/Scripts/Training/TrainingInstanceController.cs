﻿using System;
using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;
using NUnit;

namespace Training
{
    /// <summary>
    /// This is attached to the TrainingInstance and controls the generation of the env for each instance
    /// </summary>
    public class TrainingInstanceController : MonoBehaviour
    {
        public Material[] materials;

        public bool debugSetup;
        public CurriculumEnum curriculum;
        public TrainingScenario trainingScenario;

        /// <summary>
        /// The amount of times a restart has been requested by an agent during one training episode
        /// </summary>
        private int _agentRequestRestartCount;
        
        public GameObject topParent;
        public GameObject envParent;
        public GameObject debugParent;
        public GameObject spyPrefab;
        
        public GameObject guardPatrolPrefab;
        public GameObject guardAlertPrefab;
        public GameObject coroutineSurrogate;
        
        /// <summary>
        /// Instantiated spy in the scene
        /// </summary>
        public GameObject Spy { get; private set; }
        
        /// <summary>
        /// Instantiated guards in the scene
        /// </summary>
        public List<GameObject> Guards { get; } = new List<GameObject>();
        
        /// <summary>
        /// Contains the various tiles created during environment setup
        /// </summary>
        public Dictionary<TileType, List<IEnvTile>> TileDict { get; private set; }

        /// <summary>
        /// Contains GameObjects from scene hierarchy
        /// </summary>
        private Dictionary<ParentObject, GameObject> _parentObjects;

        public int mapScale;
        public int mapDifficulty ;
        public int exitCount;
        public int guardAgentCount;
        public bool hasMiddleTiles = true;

        public int AgentMapScale { get; private set; }

        /// <summary>
        /// Called before the first frame update - used to set up the academy
        /// </summary>
        public void Awake()
        {
            _parentObjects = new Dictionary<ParentObject, GameObject>()
            {
                {ParentObject.TopParent, topParent},
                {ParentObject.EnvParent, envParent},
                {ParentObject.DebugParent, debugParent}
            };
            Academy.Instance.OnEnvironmentReset += InitSetup;
        }

        /// <summary>
        /// Called once by Academy.Instance at the start of training. Sets up environment, and Spawns agents
        /// </summary>
        private void InitSetup()
        {
            try
            {
                if (debugSetup) InitDebugSetup();
                else InitCurrSetup();
            }
            catch (MapCreationException)
            {
                //Debug.Log(e);
                ClearChildrenOf(envParent);
                InitSetup();
            }
            
        }

        /// <summary>
        /// This sets up environment and agents based on parameters given in inspector, or otherwise 
        /// </summary>
        private void InitDebugSetup()
        {
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            var tileLogic = GetTileLogicDebug(tileLogicBuilder);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
            InitialiseAgents(gameParams);
        }
        
        /// <summary>
        /// Set up environment with parameters given by the specified curriculum
        /// </summary>
        private void InitCurrSetup()
        {
            float curriculumParam =
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            var (tileLogicBuilder,gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
            
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
            
            InitialiseAgents(gameParams);
        }
        
        
        /// <summary>
        /// Spawns spy and guard agents, and move them into the correct position
        /// </summary>
        /// <param name="gameParams">Parameters of the training scenario (exit count, map scale, difficulty, guard count)</param>
        private void InitialiseAgents(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            SpawnGuardAgent(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount]);
        }

        /// <summary>
        /// Called by one agent at the end of each episode, resets environment and spawns agents
        /// </summary>
        public void Restart()
        {
            try
            {
                if (debugSetup) DebugRestart();
                else RestartCurriculum();
            }
            catch (MapCreationException)
            {
                //Debug.Log(e);
                ClearChildrenOf(envParent);
                Restart();
            }
        }

        /// <summary>
        /// Resets environment and agent positions with given parameters, in the inspector or otherwise
        /// </summary>
        private void DebugRestart()
        {
            ClearChildrenOf(envParent);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            if (_agentRequestRestartCount == NumberOfAgentsIn(trainingScenario, Guards.Count))
            {
                var tileLogic = GetTileLogicDebug(tileLogicBuilder);
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
                SetAgentPosition(gameParams);
                _agentRequestRestartCount = 0;
            }
            else
            {
                _agentRequestRestartCount++;
            }
        }
        
        /// <summary>
        /// Resets the environment and agent positions with parameters from curriculum
        /// </summary>
        private void RestartCurriculum()
        {
            ClearChildrenOf(envParent);
            float curriculumParam =
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            
            if (_agentRequestRestartCount == NumberOfAgentsIn(trainingScenario, Guards.Count))
            {
                var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
                SetAgentPosition(gameParams);
                _agentRequestRestartCount = 0;

            }
            else
            {
                _agentRequestRestartCount++;
            }
        }
        
        
        /// <summary>
        /// Returns the matrix of tiles which dictates spawning of environment and agents, and the game parameters of the tile logic,
        /// this is based on the game parameters given by curriculum
        /// </summary>
        /// <remarks>
        /// Two side effects: TileDict, used by agents to know about the current map is updated, and the AgentMapScale
        /// is updated which is used to normalise the distances of the level by the agents - these need to be updated
        /// whenever the level changes
        /// </remarks>
        /// <param name="tileLogicBuilder">tile logic builder which organises the classes used to create the tile logic</param>
        /// <param name="gameParams">game parameters from curriculum</param>
        /// <returns>Logic matrix, and map scale</returns>
        private (IEnvTile[,] tileLogic, int gameParamMapScale) GetTileLogicAndGameParamMapScaleCurr(ITileLogicBuilder tileLogicBuilder,
            Dictionary<GameParam, int> gameParams)
        {
            ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            int gameParamMapScale = gameParams[GameParam.MapScale];
            AgentMapScale = gameParamMapScale;
            return (tileLogic, gameParamMapScale);
        }
        
        /// <summary>
        /// Returns tile logic matrix
        /// </summary>
        /// <remarks>
        /// Two side effects: updates TileDict and AgentMapScale which are used by agents - need to be updated each time map changes
        /// </remarks>>
        /// <param name="tileLogicBuilder">tile logic builder which organises the classes used to create the tile logic</param>
        /// <returns>tile logic matrix</returns>
        private IEnvTile[,] GetTileLogicDebug(ITileLogicBuilder tileLogicBuilder)
        {
            ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            AgentMapScale = mapScale;
            return tileLogic;
        }

        /// <summary>
        /// Spawn agents based on the scenario
        /// </summary>
        /// <param name="gameParams">game parameters</param>
        private void SetAgentPosition(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
        }
        
        /// <summary>
        /// Facade injector gets the TileLogicFacade, this facade is then used to interpret the curriculum parameters
        /// and return the tile logic builder and the game parameters as interpreted by the the facade
        /// </summary>
        /// <param name="curriculumParam">The float given by the Academy curriculum</param>
        /// <returns>Tile logic builder and interpreted game parameters</returns>
        private (ITileLogicBuilder tileLogicBuilder, Dictionary<GameParam, int> gameParams)
            GetTileLogicBuilderAndGameParamsFromCurr(float curriculumParam)
        {
            TileLogicFacadeInjector facadeInjector = new TileLogicFacadeInjector();
            ITileLogicFacade logicBuilderFacade = facadeInjector.GetTileLogicFacade(curriculum, curriculumParam);
            Dictionary<GameParam, int> gameParams = logicBuilderFacade.EnvParams;
            ITileLogicBuilder tileLogicBuilder =
                logicBuilderFacade.GetTileLogicBuilder(curriculumParam, _parentObjects);
            return (tileLogicBuilder, gameParams);
        }


        /// <summary>
        /// Returns the tile builder. There is no need to interpret the parameters given by curriculum - the code calling
        /// this method already has access to the game parameters. A game parameter dictionary is created anyway
        /// to encourage code reuse between the curriculum and debug sections of code
        /// </summary>
        /// <returns>tile logic builder and game paremeters</returns>
        private (ITileLogicBuilder tileLogicBuilder, Dictionary<GameParam, int> gameParams) GetTileLogicBuilderAndGameParamsDebug()
        {
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                mapScale: mapScale,
                mapDifficulty: mapDifficulty,
                exitCount: exitCount,
                guardAgentCount: guardAgentCount,
                parentDictionary: _parentObjects,
                hasMiddleTiles: hasMiddleTiles
            );
            Dictionary<GameParam, int> gameParams = new Dictionary<GameParam, int>()
            {
                {GameParam.MapScale, mapScale},
                {GameParam.MapDifficulty, mapDifficulty},
                {GameParam.ExitCount, exitCount},
                {GameParam.GuardAgentCount,guardAgentCount}
            };
            return (tileLogicBuilder, gameParams);
        }
        
        
        /// <summary>
        /// Checks if the spy can spawn based on the training scenario
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool SpyCanSpawn(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.SpyEvade
            || inputTrainingScenario == TrainingScenario.GuardAlert
            || inputTrainingScenario == TrainingScenario.SpyPathFinding
            || inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy;

        /// <summary>
        /// Checks if training scenario wants a patrol guard
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool TrainingScenarioWantsPatrol(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy
            || inputTrainingScenario == TrainingScenario.GuardPatrol
            || inputTrainingScenario == TrainingScenario.SpyEvade;

        /// <summary>
        /// Checks if training scenario wants an alert
        /// </summary>
        /// <param name="inputTrainingScenario"></param>
        /// <returns></returns>
        private bool TrainingScenarioWantsAlert(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.GuardAlert;

        
        /// <summary>
        /// Adds guards to the list of guards property, will only spawn up to the number of exits - 1
        /// </summary>
        /// <param name="numberOfGuards"></param>
        /// <param name="inputExitCount"></param>
        private void SpawnGuardAgent(int numberOfGuards, int inputExitCount)
        {
            if (TileDict[TileType.GuardTiles].Count < numberOfGuards)
                throw new MapCreationException("Number of guards has exceeded the number of spawn places");

            
            var indexes =
                RandomHelper.GetUniqueRandomList(MaxNumberOfGuards(numberOfGuards, exitCount), 
                    TileDict[TileType.GuardTiles].Count);
            
            for (int i = 0; i < MaxNumberOfGuards(numberOfGuards, inputExitCount); i++)
            {
                if (TrainingScenarioWantsPatrol(trainingScenario))
                {
                    Guards.Add(Instantiate(guardPatrolPrefab, TileDict[TileType.GuardTiles][indexes[i]].Position, 
                        Quaternion.identity));
                }

                if (TrainingScenarioWantsAlert(trainingScenario))
                {
                    Guards.Add(Instantiate(guardAlertPrefab, TileDict[TileType.GuardTiles][indexes[i]].Position,
                        Quaternion.identity));

                }
            }
            Guards.ForEach(guard => guard.transform.parent = transform);
        }
        
        /// <summary>
        /// Spawns guard at a random guard spawn tile
        /// </summary>
        /// <param name="potentialSpawnTiles"></param>
        /// <param name="gameParams"></param>
        private void MoveGuardAgents(List<IEnvTile> potentialSpawnTiles, Dictionary<GameParam, int> gameParams)
        {
            if (potentialSpawnTiles.Count < Guards.Count)
            {
                throw new MapCreationException("Number of guards has exceeded the number of spawn places");
            }
            var indexes =
                RandomHelper.GetUniqueRandomList(MaxNumberOfGuards(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount]), potentialSpawnTiles.Count);
            for (int i = 0; i < Guards.Count; i++)
            {
                Guards[i].transform.position = potentialSpawnTiles[indexes[i]].Position;
            }
        }

        /// <summary>
        /// Used to get the possible number of agents in each training scenario
        /// three possible combinations, both spy and guard, just spy, just guards
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
                case TrainingScenario.SpyEvade:
                    return 1 + guardCount;
                case TrainingScenario.SpyPathFinding:
                    return 1;
                case TrainingScenario.GuardPatrol:
                    return guardCount;
                default:
                    return guardCount + 1;
            }
        }

        /// <summary>
        /// will return the maximum number of guards that the training scenario will allow for 
        /// </summary>
        /// <param name="guardCount"></param>
        /// <param name="exitCount"></param>
        /// <returns></returns>
        private static int MaxNumberOfGuards(int guardCount, int exitCount) =>
            guardCount >= exitCount ? exitCount - 1 : guardCount;

        /// <summary>
        /// Clears children of given GameObject in hierarchy
        /// </summary>
        /// <param name="parentObject">Parent GameObject</param>
        private void ClearChildrenOf(GameObject parentObject)
        {
            foreach (Transform child in parentObject.transform) Destroy(child.gameObject);
        }
        
        /// <summary>
        /// Checks if SpyPrefab has been made - if not then it instantiates one, otherwise its position is set
        /// at the given spawn tile from the tile dictionary (delete me and change debug mode)
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
            }
        }
    }
}