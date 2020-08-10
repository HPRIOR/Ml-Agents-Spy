using System.Collections;
using Agents;
using Enums;
using JetBrains.Annotations;
using NUnit.Framework;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AgentTests
{
    public class AlertGuardTests : AbstractAgentTestSetup
    {
        [UnityTest]
        public IEnumerator Spy_Local_Position_Observation()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var max = StaticFunctions.GetMaxLocalDistance(1);
            var frontRow = StaticFunctions.NormalisedFloat(-max, max, trainingInstanceController.TileDict[TileType.SpyTile][0].Position.z);
            
            Assert.AreEqual(frontRow, 
                trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetSpyLocalPositions().Item2);
            
        }
        
        [UnityTest]
        public IEnumerator Spy_Local_Position_Observation_OffSet()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            trainingInstanceController.transform.position = new Vector3(100,100, 100);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var max = StaticFunctions.GetMaxLocalDistance(1);
            var frontRow = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController.TileDict[TileType.SpyTile][0].Position, trainingInstanceController).z);
            
            Assert.AreEqual(frontRow, 
                trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetSpyLocalPositions().Item2);
            
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Guard_Count_Map_Scale_1_1_Guard()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            Assert.AreEqual(6, 
                trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetGuardPositions(3).Count);
            
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Guard_Count_Map_Scale_2_2_Guards()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 2, 5, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            Assert.AreEqual(6, 
                trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetGuardPositions(3).Count);
            
        }
        
        
        
    }
}