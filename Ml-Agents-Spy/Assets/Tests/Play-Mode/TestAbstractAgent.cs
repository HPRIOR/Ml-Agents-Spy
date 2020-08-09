using System.Collections;
using Agents;
using Enums;
using NUnit.Framework;
using Training;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestAbstractAgent : AbstractTestTrainingScenarioSetup
    {
        private TrainingInstanceController GetTrainingInstance(int mapScale)
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, mapScale, 0, 
                2, 1, false);
            return trainingInstanceController;
        }
        
        
        [UnityTest]
        public IEnumerator Move_Up()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var initialPositionY = agentScript.NormalisedPositionY();
            
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(1);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(1);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(1);

            var postPositionY = agentScript.NormalisedPositionY();
            Assert.Greater(postPositionY, initialPositionY);

        }
        
        [UnityTest]
        public IEnumerator Move_Down()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            GameObject agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();

            agentObject.transform.position = trainingInstanceController.TileDict[TileType.GuardTiles][0].Position;
            
            var initialPositionY = agentScript.NormalisedPositionY();
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(2);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(2);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(2);

            var postPositionY = agentScript.NormalisedPositionY();
            Assert.Less(postPositionY, initialPositionY);

        }

        
        [UnityTest]
        public IEnumerator Move_Right()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var initialPositionX = agentScript.NormalisedPositionX();
            
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(3);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(3);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(3);

            var postPositionX = agentScript.NormalisedPositionX();
            Assert.Greater(postPositionX, initialPositionX);
        }
        
        [UnityTest]
        public IEnumerator Move_Left()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var initialPositionX = agentScript.NormalisedPositionX();
            
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(4);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(4);
            yield return new WaitForSeconds(0.1f);
            agentScript.MoveAgent(4);

            var postPositionX = agentScript.NormalisedPositionX();
            Assert.Less(postPositionX, initialPositionX);
        }
        
        // TODO Spawn Position (different map scales)
        
        // TODO spawn Test Position offset (localPosition)
        
        // TODO test specific application of normalisation both offset and for nearest guard and tile 
        
        
        
        
        
        
        
        


    }
}