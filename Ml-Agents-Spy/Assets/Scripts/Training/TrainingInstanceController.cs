using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Enums;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.WSA;
using static StaticFunctions;

namespace Training
{
    /// <summary>
    /// This is attached to the TrainingInstance and controls the generation of the env for each instance
    /// </summary>
    public class TrainingInstanceController : MonoBehaviour
    {
        public Material[] Materials;
        
        public bool DebugSetup;
        public Curriculum Curriculum;
        public TrainingScenario TrainingScenario;
        
        private int _restartCount = 0;
        private bool _initialSetup = true;
        
        
        public GameObject TopParent;
        public GameObject EnvParent;
        public GameObject DebugParent;

        public GameObject SpyPrefab;
        public GameObject SpyPrefabClone { get; private set; }
        public GameObject GuardPatrolPrefab;
        public GameObject GuardAlertPrefab;
        
        public List<GameObject> GuardClones { get; } = new List<GameObject>();
        

        private GameObject _debugGuard;

        /// <summary>
        /// Contains the various tiles created during environment setup
        /// </summary>
        public Dictionary<TileType, List<IEnvTile>> TileDict;
        /// <summary>
        /// Contains GameObjects from scene hierarchy
        /// </summary>
        private Dictionary<ParentObject, GameObject> _parentObjects;

        public int MapScale = 5;
        public int MapDifficulty = 100;
        public int ExitCount = 3;
        public int GuardAgentCount = 5;
        public bool HasMiddleTiles = true;

        [HideInInspector]
        public int AgentMapScale;

        /// <summary>
        /// Called before the first frame update - used to set up the academy
        /// </summary>
        public void Awake()
        {
            _parentObjects = new Dictionary<ParentObject, GameObject>()
            {
                {ParentObject.TopParent, TopParent},
                {ParentObject.EnvParent, EnvParent},
                {ParentObject.DebugParent, DebugParent}
            };
            Academy.Instance.OnEnvironmentReset += Restart;
            
        }

        public void Restart()
        {
            if (DebugSetup)
            {
                try
                {
                    DebugRestart();
                }
                catch (MapCreationException e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                try
                {
                    RestartCurriculum();
                }
                catch (MapCreationException e)
                {
                    Debug.Log(e);
                }
            }
        }

        private void DebugRestart()
        {
            ClearChildrenOf(EnvParent);
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                mapScale: MapScale,
                mapDifficulty: MapDifficulty,
                exitCount: ExitCount,
                guardAgentCount: GuardAgentCount,
                parentDictionary: _parentObjects,
                hasMiddleTiles: HasMiddleTiles
            );
            Dictionary<GameParam, int> gameParams = new Dictionary<GameParam, int>()
            {
                {GameParam.MapScale, MapScale},
                {GameParam.MapDifficulty, MapDifficulty},
                {GameParam.ExitCount, ExitCount},
                {GameParam.GuardAgentCount, GuardAgentCount}
            };
            Debug.Log(_restartCount);
            Debug.Log(_initialSetup);

            if (_initialSetup)
            {
                Debug.Log("Restart called in initial setup");
                ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
                IEnvTile[,] tileMatrix = tileLogic.GetTileLogic();
                CreateEnv.PopulateEnv(tileMatrix, _parentObjects, MapScale, Materials);
                AgentMapScale = MapScale;
                TileDict = tileLogic.GetTileTypes();
                
                IAgentSpawner agentSpawner = 
                    new AgentSpawner(TrainingScenario,
                        SpyPrefab, 
                        GuardPatrolPrefab, 
                        GuardAlertPrefab, 
                        transform, 
                        TileDict, 
                        gameParams, 
                        _initialSetup, 
                        GuardClones);
                agentSpawner.SpawnAgents();
                //if (SpyCanSpawn(TrainingScenario)) SpawnSpyAgent();
                //SpawnGuardAgent(gameParams[GameParam.GuardAgentCount]);
                //MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
                //_initialSetup = false;
            }

            // this should work but number of guards in scene needs to be accounted for
            // in the initial setup, create the required list of guards, then move these around in
            // subsequent runs.
            // e.g. if (_restartCount == amount of guards depending on the scenario)
            if (_restartCount == GuardClones.Count)
            {
                Debug.Log("Restart called by agent");
                ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
                IEnvTile[,] tileMatrix = tileLogic.GetTileLogic();
                CreateEnv.PopulateEnv(tileMatrix, _parentObjects, MapScale, Materials);
                AgentMapScale = MapScale;
                TileDict = tileLogic.GetTileTypes();
                IAgentSpawner agentSpawner = 
                    new AgentSpawner(TrainingScenario,
                        SpyPrefab, 
                        GuardPatrolPrefab, 
                        GuardAlertPrefab, 
                        transform, 
                        TileDict, 
                        gameParams, 
                        _initialSetup, 
                        GuardClones);
                agentSpawner.SpawnAgents();
                //if (SpyCanSpawn(TrainingScenario)) SpawnSpyAgent();
                //MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
                _restartCount = 0;
            }
            else
            {
                _restartCount++;
            }
        }

        private bool SpyCanSpawn(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.SpyEvade
            || trainingScenario == TrainingScenario.GuardAlert
            || trainingScenario == TrainingScenario.SpyPathFinding
            || trainingScenario == TrainingScenario.GuardPatrolWithSpy;

        private bool TrainingScenarioWantsPatrol(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.GuardPatrolWithSpy
            || trainingScenario == TrainingScenario.GuardPatrol
            || trainingScenario == TrainingScenario.SpyEvade;

        private bool TrainingScenarioWantsAlert(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.GuardAlert;
        
        
        private void SpawnGuardAgent(int numberOfGuards)
        {
            for (int i = 0; i < numberOfGuards; i++)
            {
               if (TrainingScenarioWantsPatrol(TrainingScenario))
                    GuardClones.Add(Instantiate(GuardPatrolPrefab, TileDict[TileType.GuardTiles][0].Position, Quaternion.identity));
               if (TrainingScenarioWantsAlert(TrainingScenario))
                   GuardClones.Add(Instantiate(GuardAlertPrefab, TileDict[TileType.GuardTiles][0].Position, Quaternion.identity));
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

        private void RestartCurriculum()
        {
            float curriculumParam = 
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            TileLogicFacadeInjector facadeInjector = new TileLogicFacadeInjector();
            ITileLogicFacade logicBuilderFacade = facadeInjector.GetTileLogicFacade(Curriculum, curriculumParam);
            Dictionary<GameParam, int> envParams = logicBuilderFacade.EnvParams;
            ClearChildrenOf(EnvParent);
            ITileLogicBuilder tileLogicBuilder = logicBuilderFacade.GetTileLogicBuilder(curriculumParam, _parentObjects);
            ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
            IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
            TileDict = tileLogicSetup.GetTileTypes();
            int mapScale = envParams[GameParam.MapScale];
            AgentMapScale = mapScale;
            CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, Materials);
            SpawnSpyAgent();

        }

        

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
                SpyPrefabClone = Instantiate(SpyPrefab, TileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                SpyPrefabClone.transform.parent = transform;
            }
            else
            {
                SpyPrefabClone.transform.position = TileDict[TileType.SpyTile][0].Position;
            }
        }
        

        

    }
}
