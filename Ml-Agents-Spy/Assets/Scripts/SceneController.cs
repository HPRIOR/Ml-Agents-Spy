using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Playables;

public class SceneController : MonoBehaviour
{
    public GameObject TopLevelParent;
    // Start is called before the first frame update
    public void Awake()
    {
        //Academy.Instance.OnEnvironmentReset += RestartEnv;
        RestartEnv();
    }


    void RestartEnv()
    {
        IEnvSetup env = new EnvSetup(2, 2, TopLevelParent);
        env.CreateEnv();
        
    }
}
