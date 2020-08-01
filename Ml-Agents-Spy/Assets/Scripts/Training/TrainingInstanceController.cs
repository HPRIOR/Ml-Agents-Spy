using System;
using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;

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

        private int _restartCount;


        public GameObject topParent;
        public GameObject envParent;
        public GameObject debugParent;

        public GameObject spyPrefab;
        public GameObject SpyPrefabClone { get; private set; }
        public GameObject guardPatrolPrefab;
        public GameObject guardAlertPrefab;
        public List<GameObject> GuardClones { get; } = new List<GameObject>();
        
        private GameObject _debugGuard;

        /// <summary>
        /// Contains the various tiles created during environment setup
        /// </summary>
        public Dictionary<TileType, List<IEnvTile>> TileDict { get; private set; }

        /// <summary>
        /// Contains GameObjects from scene hierarchy
        /// </summary>
        private Dictionary<ParentObject, GameObject> _parentObjects;

        public int mapScale = 5;
        public int mapDifficulty = 100;
        public int exitCount = 3;
        public int guardAgentCount = 5;
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

        private void InitSetup()
        {
            try
            {
                if (debugSetup) InitDebugSetup();
                else InitCurrSetup();
            }
            catch (MapCreationException e)
            {
                Debug.Log(e);
            }
            
        }

        private void InitDebugSetup()
        {
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            var tileLogic = GetTileLogicDebug(tileLogicBuilder);
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
            InitialiseAgents(gameParams);
        }
        
        private void InitCurrSetup()
        {
            float curriculumParam =
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            var (tileLogicBuilder,gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
            
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
            
            InitialiseAgents(gameParams);
        }
        
        private void InitialiseAgents(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            SpawnGuardAgent(gameParams[GameParam.GuardAgentCount], gameParams[GameParam.ExitCount]);
            MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
        }

        public void Restart()
        {
            try
            {
                if (debugSetup) DebugRestart();
                else RestartCurriculum();
            }
            catch (MapCreationException e)
            {
                Debug.Log(e);
            }
        }

        private void DebugRestart()
        {
            ClearChildrenOf(envParent);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsDebug();
            if (_restartCount == NumberOfAgentsIn(trainingScenario, GuardClones.Count))
            {
                var tileLogic = GetTileLogicDebug(tileLogicBuilder);
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, materials);
                SpawnAgents(gameParams);
            }
            else
            {
                _restartCount++;
            }
        }
        

        private void RestartCurriculum()
        {
            ClearChildrenOf(envParent);
            float curriculumParam =
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            var (tileLogicBuilder, gameParams) = GetTileLogicBuilderAndGameParamsFromCurr(curriculumParam);
            
            if (_restartCount == NumberOfAgentsIn(trainingScenario, GuardClones.Count))
            {
                var (tileLogic, gameParamMapScale) = GetTileLogicAndGameParamMapScaleCurr(tileLogicBuilder, gameParams);
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, gameParamMapScale, materials);
                SpawnAgents(gameParams);
            }
            else
            {
                _restartCount++;
            }
        }

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

        private void SpawnAgents(Dictionary<GameParam, int> gameParams)
        {
            if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
            MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
            _restartCount = 0;
        }
        
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
                {GameParam.GuardAgentCount, guardAgentCount}
            };
            return (tileLogicBuilder, gameParams);
        }
        
        private IEnvTile[,] GetTileLogicDebug(ITileLogicBuilder tileLogicBuilder)
        {
            ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            AgentMapScale = mapScale;
            return tileLogic;
        }

        private bool SpyCanSpawn(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.SpyEvade
            || inputTrainingScenario == TrainingScenario.GuardAlert
            || inputTrainingScenario == TrainingScenario.SpyPathFinding
            || inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy;

        private bool TrainingScenarioWantsPatrol(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.GuardPatrolWithSpy
            || inputTrainingScenario == TrainingScenario.GuardPatrol
            || inputTrainingScenario == TrainingScenario.SpyEvade;

        private bool TrainingScenarioWantsAlert(TrainingScenario inputTrainingScenario) =>
            inputTrainingScenario == TrainingScenario.GuardAlert;


        private void SpawnGuardAgent(int numberOfGuards, int inputExitCount)
        {
            for (int i = 0; i < MaxNumberOfGuards(numberOfGuards, inputExitCount); i++)
            {
                if (TrainingScenarioWantsPatrol(trainingScenario))
                    GuardClones.Add(Instantiate(guardPatrolPrefab, TileDict[TileType.GuardTiles][0].Position,
                        Quaternion.identity));
                if (TrainingScenarioWantsAlert(trainingScenario))
                    GuardClones.Add(Instantiate(guardAlertPrefab, TileDict[TileType.GuardTiles][0].Position,
                        Quaternion.identity));
            }

            GuardClones.ForEach(guard => guard.transform.parent = transform);
        }

        private void MoveGuardAgents(List<IEnvTile> potentialSpawnTiles, Dictionary<GameParam, int> gameParams)
        {
            var indexes =
                RandomHelper.GetUniqueRandomList(gameParams[GameParam.GuardAgentCount], potentialSpawnTiles.Count);
            for (int i = 0; i < GuardClones.Count; i++)
            {
                GuardClones[i].transform.position = potentialSpawnTiles[indexes[i]].Position;
            }
        }

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
            if (SpyPrefabClone is null)
            {
                SpyPrefabClone = Instantiate(spyPrefab, TileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                SpyPrefabClone.transform.parent = transform;
            }
            else
            {
                SpyPrefabClone.transform.position = TileDict[TileType.SpyTile][0].Position;
            }
        }
    }
}