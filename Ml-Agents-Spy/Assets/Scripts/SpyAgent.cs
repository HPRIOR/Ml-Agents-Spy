using System.Linq;
using Unity.MLAgents;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SpyAgent : Agent
{
    private SceneController _parentSceneController;
    private Vector3[] _exitPositions;
    private Transform[] _guardPositions;
    public GameObject SpyPrefab;
    private GameObject _spyPrefabClone;
    private Rigidbody _spyRigidBody;

    // called at start - may be used like a constructor
    void Start()
    {
        _parentSceneController = GetComponentInParent<SceneController>();
    }
    
    public override void OnEpisodeBegin()
    {
        SetSpyPrefab();
        // GetGuardPositions (Transforms)
        _exitPositions = getExitPositions;
    }

    public override void OnActionReceived(float[] action)
    {
        base.OnActionReceived(action);
    }

    

    /// <summary>
    /// Returns an array of each exit tile Vector3 position 
    /// </summary>
    private Vector3[] getExitPositions => 
        _parentSceneController.ExitTiles.Select(tile => tile.Position).ToArray();
    

    /// <summary>
    /// Creates new Agent prefab if non existed, or destroys old prefab and creates a new one
    /// </summary>
    void SetSpyPrefab()
    {
        if (_spyPrefabClone is null) InstantiateSpyPrefab();
        else
        {
            DestroyImmediate(_spyPrefabClone);
            InstantiateSpyPrefab();
        }
    }

    /// <summary>
    /// Instantiates prefab object at given place in the scene
    /// </summary>
    void InstantiateSpyPrefab()
    {
        _spyPrefabClone = Instantiate(SpyPrefab, _parentSceneController.SpyTile[0].Position, Quaternion.identity);
        _spyPrefabClone.transform.parent = transform;
        _spyRigidBody = _spyPrefabClone.GetComponent<Rigidbody>();
    }


   
}
