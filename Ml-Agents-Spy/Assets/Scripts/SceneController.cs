using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// This is attached to the TrainingInstance and controls the generation of the env for each instance
/// </summary>
public class SceneController : MonoBehaviour
{
    public GameObject TopParent;
    public GameObject EnvParent;
    public GameObject SpyParent;
    public GameObject GuardParent;
    public GameObject DebugParent;

    private Dictionary<ParentObject, GameObject> parentObjects;


    // Start is called before the first frame update
    public void Awake()
    {
        parentObjects = new Dictionary<ParentObject, GameObject>()
        {
            {ParentObject.TopParent, TopParent},
            {ParentObject.EnvParent, EnvParent},
            {ParentObject.SpyParent, SpyParent},
            {ParentObject.GuardParent, GuardParent},
            {ParentObject.DebugParent, DebugParent}

        };
        //Academy.Instance.OnEnvironmentReset += RestartEnv;
        RestartEnv();
    }


    void RestartEnv()
    {
        // pass in parents as a names tuples
        IEnvSetup env = new EnvSetup(
            mapSize: 5,
            mapDifficulty: 150,
            exitCount: 3,
            guardAgentCount: 5,
            parents: parentObjects
        );

        env.SetUpEnv();
        
    }
}
