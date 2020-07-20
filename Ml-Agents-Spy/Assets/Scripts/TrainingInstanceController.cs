using System;
using Unity.MLAgents;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is attached to the TrainingInstance and controls the generation of the env for each instance
/// </summary>
public class TrainingInstanceController : MonoBehaviour
{
    public GameObject TopParent;
    public GameObject EnvParent;
    public GameObject DebugParent;
    public GameObject SpyPrefab;
    private GameObject _spyPrefabClone;

    public Dictionary<TileType, List<Tile>> TileDict;
    private Dictionary<ParentObject, GameObject> _parentObjects;

    public int MapScale = 5;
    public int MapDifficulty = 100;
    public int ExitCount = 3;
    public int GuardAgentCount = 5;
    public bool HasMiddleTiles = true;

    // Start is called before the first frame update
    public void Awake()
    {
        _parentObjects = new Dictionary<ParentObject, GameObject>()
        {
            {ParentObject.TopParent, TopParent},
            {ParentObject.EnvParent, EnvParent},
            {ParentObject.DebugParent, DebugParent}

        };
        Academy.Instance.OnEnvironmentReset += RestartEnv;
        //RestartEnv();

    }

    /*
     * this is not called OnEpisodeBegin
     */
    public void RestartEnv()
    {
        float curriculumParam = Academy.Instance.EnvironmentParameters.GetWithDefault("spy_curriculum", 1.0f);
        ClearEnv();
        
        IEnvSetup env = new EnvSetup(
            mapScale: MapScale,
            mapDifficulty: MapDifficulty,
            exitCount: ExitCount,
            guardAgentCount: GuardAgentCount,
            parentDictionary: _parentObjects,
            hasMiddleTiles: HasMiddleTiles
        );
        
        env.SetUpEnv();
        TileDict = env.GetTileTypes();
        SpawnAgent();
    }

    void ClearEnv()
    {
        foreach (Transform child in EnvParent.transform) Destroy(child.gameObject);
    }
    

    void SpawnAgent()
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
