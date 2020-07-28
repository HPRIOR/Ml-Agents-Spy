using System.Collections;
using Agents;
using Training;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.MLAgents;




namespace Tests
{
    public class TestSpyAgentMovement
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestSpyAgentMovementWithEnumeratorPasses()
        {
            GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
            //var trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return new  WaitForSeconds(0.1f);
            var spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            var agentScript = spyPrefab.GetComponent<SpyAgent>();
            //var spyAgent = spyPrefab.GetComponent<SpyAgent>();
            agentScript.Heuristic(new[] {1.0f});
            
            

            
            
        }
    }
}
