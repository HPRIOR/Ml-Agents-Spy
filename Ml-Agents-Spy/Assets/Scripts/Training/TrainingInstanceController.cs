﻿using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;
using static StaticFunctions;

namespace Training
{
    /// <summary>
    /// This is attached to the TrainingInstance and controls the generation of the env for each instance
    /// </summary>
    public class TrainingInstanceController : MonoBehaviour
    {
        public Material[] Materials;
        
        public bool DebugEnvSetup;
        public Curriculum Curriculum;
    
        public GameObject TopParent;
        public GameObject EnvParent;
        public GameObject DebugParent;

        public GameObject SpyPrefab;
        private GameObject _spyPrefabClone;

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
            Academy.Instance.OnEnvironmentReset += RestartEnv;
        }
        
        public void RestartEnv()
        {
            if (DebugEnvSetup)
            {
                try
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

                    ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
                    IEnvTile[,] tileMatrix = tileLogic.GetTileLogic();
                    CreateEnv.PopulateEnv(tileMatrix, _parentObjects, MapScale, Materials);
                    AgentMapScale = MapScale;
                    TileDict = tileLogic.GetTileTypes();
                    SpawnSpyAgent();
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
                    ClearChildrenOf(EnvParent);
                    // Get parameter from curriculum
                    float curriculumParam = Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);

                    // get logic matrix, tiletype dictionary, and gameparam dictionary
                    var (tileMatrix, tileDict, gameParams) = GetTileLogic((int) curriculumParam);
                    
                    // get the MapScale from based on the matrix (So that we don't need to expose this class to the setup classes)
                    int mapScale = gameParams[GameParam.MapScale];
                    
                    // Create the 3D env based on Tile logic
                    CreateEnv.PopulateEnv(tileMatrix, _parentObjects, mapScale, Materials);

                    // update class fields so that Agent knows the mapscale and tiletypes
                    AgentMapScale = mapScale;
                    TileDict = tileDict;
                    
                    IAgentSpawner agentSpawner = new AgentSpawner(TileDict, gameParams, TrainingScenario.SpyPathFinding, new []{SpyPrefab}, transform);
                    agentSpawner.SpawnAgents();
                    
                }
                catch (MapCreationException e)
                {
                    Debug.Log(e);
                }
            }
        }
        
        private (IEnvTile[,] tileMatrix, Dictionary<TileType, List<IEnvTile>> tileDict, Dictionary<GameParam, int> 
            gameParams) GetTileLogic(int curriculumParam)
        {
            TileLogicFacadeInjector facadeInjector = new TileLogicFacadeInjector();
            ITileLogicFacade envFacade = facadeInjector.GetTileLogicFacade(Curriculum);
            ITileLogicBuilder tileLogicBuilder = envFacade.GetTileLogicBuilder(curriculumParam, _parentObjects);
            ITileLogicSetup tileLogic = tileLogicBuilder.GetTileLogicSetup();
            return (tileLogic.GetTileLogic(), tileLogic.GetTileTypes(), envFacade.EnvParamDict);
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
            if (_spyPrefabClone is null)
            {
                _spyPrefabClone = Instantiate(SpyPrefab, TileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                _spyPrefabClone.transform.parent = transform;
            }
            else
            {
                _spyPrefabClone.transform.position = TileDict[TileType.SpyTile][0].Position;
            }
        }

    }
}
