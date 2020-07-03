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
    public GameObject PerimeterParent;
    public GameObject MiddleParent;
    public GameObject BoxComplexities;
    public GameObject InteractiveElements;

    private Dictionary<ParentObject, GameObject> parentObjects;


    // Start is called before the first frame update
    public void Awake()
    {
        parentObjects = new Dictionary<ParentObject, GameObject>()
        {
            {ParentObject.TopParent, TopParent},
            {ParentObject.PerimeterParent, PerimeterParent},
            {ParentObject.MiddleParent, MiddleParent},
            {ParentObject.ComplexitiesParent, BoxComplexities},
            {ParentObject.InteractiveParent, InteractiveElements}

        };
        //Academy.Instance.OnEnvironmentReset += RestartEnv;
        RestartEnv();
    }


    void RestartEnv()
    {
        // pass in parents as a names tuples
        IEnvSetup env = new EnvSetup(1, 0, parentObjects);
        env.CreateEnv();
        
    }
}
