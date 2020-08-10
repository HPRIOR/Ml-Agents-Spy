using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agents;
using Enums;
using NUnit.Framework;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AgentTests
{
    public class GuardTests : AbstractAgentTestSetup
    {
        [UnityTest]
        public IEnumerator Test_Nearest_Guards()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4,0,4,3,false);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var freeTiles = trainingInstanceController.TileDict[TileType.FreeTiles]
                .Concat(trainingInstanceController.TileDict[TileType.SpyTile])
                .Concat(trainingInstanceController.TileDict[TileType.GuardTiles]).ToList();
            
            var agentPosition1 = freeTiles.First(t => t.Coords == (7, 7)).Position;
            var agentPosition2 = freeTiles.First(t => t.Coords == (7, 8)).Position;
            var agentPosition3 = freeTiles.First(t => t.Coords == (7, 9)).Position;
            
            trainingInstanceController.Guards[0].transform.position = agentPosition1;
            trainingInstanceController.Guards[1].transform.position = agentPosition2;
            trainingInstanceController.Guards[2].transform.position = agentPosition3;

            var max = StaticFunctions.GetMaxLocalDistance(4);
            
            List<float> nearestPosToAgent1 = new List<float>()
            {
                StaticFunctions.NormalisedFloat(-max, max,agentPosition2.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition2.z),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.z)
            };
            
            Assert.AreEqual(nearestPosToAgent1, trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetGuardPositions(2));
        }
        
        [UnityTest]
        public IEnumerator Test_Nearest_Guards_Padding()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4,0,4,3,false);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var freeTiles = trainingInstanceController.TileDict[TileType.FreeTiles]
                .Concat(trainingInstanceController.TileDict[TileType.SpyTile])
                .Concat(trainingInstanceController.TileDict[TileType.GuardTiles]).ToList();
            
            var agentPosition1 = freeTiles.First(t => t.Coords == (7, 7)).Position;
            var agentPosition2 = freeTiles.First(t => t.Coords == (7, 8)).Position;
            var agentPosition3 = freeTiles.First(t => t.Coords == (7, 9)).Position;
            
            trainingInstanceController.Guards[0].transform.position = agentPosition1;
            trainingInstanceController.Guards[1].transform.position = agentPosition2;
            trainingInstanceController.Guards[2].transform.position = agentPosition3;

            var max = StaticFunctions.GetMaxLocalDistance(4);
            
            List<float> nearestPosToAgent1 = new List<float>()
            {
                StaticFunctions.NormalisedFloat(-max, max,agentPosition2.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition2.z),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.z),
                0,0,0,0
            };
            
            Assert.AreEqual(nearestPosToAgent1, trainingInstanceController.Guards[0].GetComponent<AlertGuardAgent>().GetGuardPositions(4));
        }
    }
}