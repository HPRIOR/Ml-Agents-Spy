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
    public List<Tile> SpyTile;
    public List<Tile> GuardTiles;
    public List<Tile> ExitTiles;

    private GameObject _spyPrefabClone;

    private Dictionary<ParentObject, GameObject> _parentObjects;

    public int MapScale = 5;
    public int MapDifficulty = 100;
    public int ExitCount = 3;
    public int GuardAgentCount = 5;

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
        Debug.Log("RestartEnv Called");
        // pass in parents as a names tuples
        IEnvSetup env = new EnvSetup(
            mapScale: MapScale,
            mapDifficulty: MapDifficulty,
            exitCount: ExitCount,
            guardAgentCount: GuardAgentCount,
            parentDictionary: _parentObjects
        );
        // need to clear tiles here
        SetEnvTiles(env);
        SpawnAgent();
    }

    void SetEnvTiles(IEnvSetup env)
    {
        env.SetUpEnv();
        SpyTile = env.GetSpyTile();
        GuardTiles = env.GetGuardTiles();
        ExitTiles = env.GetExitTiles();
    }

    

    void SpawnAgent()
    {
        if (_spyPrefabClone is null)
        {
            _spyPrefabClone = Instantiate(SpyPrefab, SpyTile[0].Position, Quaternion.identity);
            _spyPrefabClone.transform.parent = transform;
        }
        else _spyPrefabClone.transform.localPosition = SpyTile[0].Position;
        
        
    }
}
