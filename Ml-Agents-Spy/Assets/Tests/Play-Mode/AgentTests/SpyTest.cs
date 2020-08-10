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
    public class SpyTest : AbstractAgentTestSetup
    {
        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_All_Guards()
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
                StaticFunctions.NormalisedFloat(-max, max,agentPosition1.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition1.z),
                StaticFunctions.NormalisedFloat(-max, max,agentPosition2.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition2.z),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.z)
            };
            
            Assert.AreEqual(nearestPosToAgent1, trainingInstanceController
                .Spy
                .GetComponent<SpyAgent>()
                .GetGuardPositions(3));
        }
        
        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_All_Guards_Padded()
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
                StaticFunctions.NormalisedFloat(-max, max,agentPosition1.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition1.z),
                StaticFunctions.NormalisedFloat(-max, max,agentPosition2.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition2.z),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.z),
                0,0,0,0
            };
            
            Assert.AreEqual(nearestPosToAgent1, trainingInstanceController
                .Spy
                .GetComponent<SpyAgent>()
                .GetGuardPositions(5));
        }
        
        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_All_Guards_OffSet()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4,0,4,3,false);
            trainingInstanceController.transform.position = new Vector3(100,100,100);
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

            agentPosition1 =  VectorConversions.GetLocalPosition(agentPosition1, trainingInstanceController);
            agentPosition2 = VectorConversions.GetLocalPosition(agentPosition2, trainingInstanceController);
            agentPosition3 = VectorConversions.GetLocalPosition(agentPosition3, trainingInstanceController);
            
            List<float> nearestPosToAgent1 = new List<float>()
            {
                StaticFunctions.NormalisedFloat(-max, max,agentPosition1.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition1.z),
                StaticFunctions.NormalisedFloat(-max, max,agentPosition2.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition2.z),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.x),
                StaticFunctions.NormalisedFloat(-max, max, agentPosition3.z)
            };
            
            Assert.AreEqual(nearestPosToAgent1, trainingInstanceController
                .Spy
                .GetComponent<SpyAgent>()
                .GetGuardPositions(3));
        }

        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_Padding_3_Guards()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4, 0, 4, 3, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            Assert.AreEqual(12, trainingInstanceController.Spy.GetComponent<SpyAgent>().GetGuardPositions(6).Count);
            
        }
        
        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_Padding_1_Guards()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4, 0, 4, 1, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            Assert.AreEqual(12, trainingInstanceController.Spy.GetComponent<SpyAgent>().GetGuardPositions(6).Count);
            
        }
        
        [UnityTest]
        public IEnumerator Spy_Nearest_Guards_Padding_0_Guards()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 4, 0, 4, 1, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            Assert.AreEqual(12, trainingInstanceController.Spy.GetComponent<SpyAgent>().GetGuardPositions(6).Count);
            
        }
        
        [UnityTest]
        public IEnumerator Spy_Distance_To_Exit_Map_Scale_4()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 4, 0, 4, 1, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyObject = trainingInstanceController.Spy;
            var spyScript = spyObject.GetComponent<SpyAgent>();
            // move spy in line with exit 
            spyObject.transform.position = trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .Where(x => x.Coords.x == trainingInstanceController.TileDict[TileType.ExitTiles][0].Coords.x)
                .OrderBy(x => x.Position.z)
                .First()
                .Position;
            
            var initDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            for (int i = 0; i < 5; i++)
            {
                spyScript.MoveAgent(1);
            }
            var postDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            
            Assert.Less(postDistance, initDistance);
            Assert.That(initDistance, Is.GreaterThan(0));
            // move spy to last tile 
            spyObject.transform.position = trainingInstanceController.TileDict[TileType.FreeTiles]
                .OrderBy(x => x.Position.z).Last().Position;
            Assert.That(initDistance, Is.LessThan(1));



        }
        
        [UnityTest]
        public IEnumerator Spy_Distance_To_Exit_Map_Scale_3()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 3, 0, 2, 1, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyObject = trainingInstanceController.Spy;
            var spyScript = spyObject.GetComponent<SpyAgent>();
            // move spy in line with exit 
            spyObject.transform.position = trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .Where(x => x.Coords.x == trainingInstanceController.TileDict[TileType.ExitTiles][0].Coords.x)
                .OrderBy(x => x.Position.z)
                .First()
                .Position;
            
            var initDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            for (int i = 0; i < 5; i++)
            {
                spyScript.MoveAgent(1);
            }
            var postDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            
            Assert.Less(postDistance, initDistance);
            Assert.That(initDistance, Is.GreaterThan(0));
            // move spy to last tile 
            spyObject.transform.position = trainingInstanceController.TileDict[TileType.FreeTiles]
                .OrderBy(x => x.Position.z).Last().Position;
            Assert.That(initDistance, Is.LessThan(1));

        }
        
        [UnityTest]
        public IEnumerator Spy_Distance_To_Exit_Map_Scale_1()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 1, 0, 2, 1, false);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyObject = trainingInstanceController.Spy;
            var spyScript = spyObject.GetComponent<SpyAgent>();
            
            // move spy in line with exit 
            spyObject.transform.position = trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .Where(x => x.Coords.x == trainingInstanceController.TileDict[TileType.ExitTiles][0].Coords.x)
                .OrderBy(x => x.Position.z)
                .First()
                .Position;
            
            var initDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            for (int i = 0; i < 5; i++)
            {
                spyScript.MoveAgent(1);
            }
            var postDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            
            Assert.Less(postDistance, initDistance);
            Assert.That(initDistance, Is.GreaterThan(0));
            // move spy to last tile 
            spyObject.transform.position = trainingInstanceController.TileDict[TileType.FreeTiles]
                .OrderBy(x => x.Position.z).Last().Position;
            Assert.That(initDistance, Is.LessThan(1));
            
        }
        
        [UnityTest]
        public IEnumerator Spy_Distance_To_Exit_Map_Scale_4_Offset()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 4, 0, 4, 1, false);
            trainingInstanceController.transform.position = new Vector3(100,100,100);
            
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyObject = trainingInstanceController.Spy;
            var spyScript = spyObject.GetComponent<SpyAgent>();
            // move spy in line with exit 
            spyObject.transform.position = trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .Where(x => x.Coords.x == trainingInstanceController.TileDict[TileType.ExitTiles][0].Coords.x)
                .OrderBy(x => x.Position.z)
                .First()
                .Position;
            
            var initDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            for (int i = 0; i < 5; i++)
            {
                spyScript.MoveAgent(1);
            }
            var postDistance =
                spyScript.DistanceToNearestExit(trainingInstanceController.TileDict[TileType.ExitTiles][0].Position);
            
            Assert.Less(postDistance, initDistance);
            Assert.That(initDistance, Is.GreaterThan(0));
            // move spy to last tile 
            spyObject.transform.position = trainingInstanceController.TileDict[TileType.FreeTiles]
                .OrderBy(x => x.Position.z).Last().Position;
            Assert.That(initDistance, Is.LessThan(1));
            

        }
    }
}