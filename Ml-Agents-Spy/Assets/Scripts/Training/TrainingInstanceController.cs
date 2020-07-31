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
        
        public bool debugSetup;
        public CurriculumEnum curriculum;
        public TrainingScenario trainingScenario;
        
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
            if (debugSetup)
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

            // called once by Academy
            if (_initialSetup)
            {
                Debug.Log("Restart called in initial setup");
                ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
                IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, MapScale, Materials);
                AgentMapScale = MapScale;
                TileDict = tileLogicSetup.GetTileTypes();
                if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
                SpawnGuardAgent(gameParams[GameParam.GuardAgentCount]);
                MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
                _initialSetup = false;
            }
            if (_restartCount == MaxNumberOfAgentsIn(trainingScenario, GuardClones.Count))
            {
                Debug.Log("Restart called by agent");
                ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
                IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, MapScale, Materials);
                AgentMapScale = MapScale;
                TileDict = tileLogicSetup.GetTileTypes();
                if (SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
                MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
                _restartCount = 0;
            }
            else
            {
                _restartCount++;
            }
        }
        private void RestartCurriculum()
        {
            ClearChildrenOf(EnvParent);
            
            float curriculumParam = 
                Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            TileLogicFacadeInjector facadeInjector = new TileLogicFacadeInjector();
            ITileLogicFacade logicBuilderFacade = facadeInjector.GetTileLogicFacade(curriculum, curriculumParam);
            Dictionary<GameParam, int> gameParams = logicBuilderFacade.EnvParams;
            ITileLogicBuilder tileLogicBuilder = logicBuilderFacade.GetTileLogicBuilder(curriculumParam, _parentObjects);

            if (_initialSetup)
            {
                
                ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
                // Get Tile Logic modifies the logic of the tiles
                IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
                TileDict = tileLogicSetup.GetTileTypes();
                int mapScale = gameParams[GameParam.MapScale];
                AgentMapScale = mapScale;
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, Materials);
                if(SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
                SpawnGuardAgent(gameParams[GameParam.GuardAgentCount]);
                MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
                _initialSetup = false;
            }

            if (_restartCount == MaxNumberOfAgentsIn(trainingScenario, GuardClones.Count))
            {
                ITileLogicSetup tileLogicSetup = tileLogicBuilder.GetTileLogicSetup();
                IEnvTile[,] tileLogic = tileLogicSetup.GetTileLogic();
                TileDict = tileLogicSetup.GetTileTypes();
                int mapScale = gameParams[GameParam.MapScale];
                AgentMapScale = mapScale;
                CreateEnv.PopulateEnv(tileLogic, _parentObjects, mapScale, Materials);
                if(SpyCanSpawn(trainingScenario)) SpawnSpyAgent();
                MoveGuardAgents(TileDict[TileType.GuardTiles], gameParams);
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
               if (TrainingScenarioWantsPatrol(trainingScenario))
                    GuardClones.Add(Instantiate(GuardPatrolPrefab, TileDict[TileType.GuardTiles][0].Position, Quaternion.identity));
               if (TrainingScenarioWantsAlert(trainingScenario))
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

        private int MaxNumberOfAgentsIn(TrainingScenario trainingScenario, int guardCount)
        {
            switch (trainingScenario)
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
