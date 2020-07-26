using System;
using Unity.MLAgents;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is attached to the TrainingInstance and controls the generation of the env for each instance
/// </summary>
public class TrainingInstanceController : MonoBehaviour
{
    public bool Debug;
    public string Curriculum;
    public GameObject TopParent;
    public GameObject EnvParent;
    public GameObject DebugParent;

    public GameObject SpyPrefab;
    private GameObject _spyPrefabClone;

    /// <summary>
    /// Contains the various tile created during environment setup
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
        if (Debug)
        {
            ClearChildrenOf(EnvParent);
            IEnvSetup env = new EnvSetup(
              mapScale: MapScale,
              mapDifficulty: MapDifficulty,
              exitCount: ExitCount,
              guardAgentCount: GuardAgentCount,
              parentDictionary: _parentObjects,
              hasMiddleTiles: HasMiddleTiles
              );
            env.CreateEnv();
            AgentMapScale = env.MapScale;
            TileDict = env.GetTileTypes();
            SpawnSpyAgent();
        }
        else
        {
            ClearChildrenOf(EnvParent);
            float curriculumParam = Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
            EnvSetupFacadeInjector facadeInjector = new EnvSetupFacadeInjector();
            IEnvSetupFacade envFacade = facadeInjector.GetEnvSetupFacade(Curriculum);
            IEnvSetup env = envFacade.GetEnvSetup(curriculumParam, _parentObjects);
            env.CreateEnv();
            AgentMapScale = env.MapScale;
            TileDict = env.GetTileTypes();
            SpawnSpyAgent();
        }
    }

    /// <summary>
    /// Clears children of given GameObject in hierarchy
    /// </summary>
    /// <param name="parentObject">Parent GameObject</param>
    void ClearChildrenOf(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform) Destroy(child.gameObject);
    }
    

    /// <summary>
    /// Checks if SpyPrefab has been made - if not then it instantiates one, otherwise its position is set
    /// at the given spawn tile from the tile dictionary
    /// </summary>
    void SpawnSpyAgent()
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
