using System.Collections.Generic;
using UnityEngine;

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

    public int MapScale = 5;
    public int MapDifficulty = 100;
    public int ExitCount = 3;
    public int GuardAgentCount = 4;
    


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
        IEnvSetup env = new EnvSetup(
            mapScale: MapScale,
            mapDifficulty: MapDifficulty,
            exitCount: ExitCount,
            guardAgentCount: GuardAgentCount,
            parents: parentObjects
            );

        env.SetUpEnv();
        
    }
}
