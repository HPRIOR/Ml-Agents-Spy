using System.Collections;
using Agents;
using Enums;
using NUnit.Framework;
using Training;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AgentRespawnAcceptanceTest : AbstractTestTrainingScenarioSetup
    {
        private int _iterations = 500;
        
        
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            yield return new WaitForSeconds(1f);
            SetAdvancedDebug(trainingInstanceController);
            
            yield return new WaitForSeconds(1f);
            var spy = trainingInstanceController.Spy;
            var spyScript = spy.GetComponent<SpyAgent>();
            
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < _iterations; i++)
            {
                yield return new WaitForSeconds(0.1f);
                spyScript.EndEpisode();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}