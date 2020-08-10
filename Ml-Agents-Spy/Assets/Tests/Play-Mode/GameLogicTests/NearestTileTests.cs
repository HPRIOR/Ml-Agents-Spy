using System.Collections;
using Agents;
using Enums;
using NUnit.Framework;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.GameLogicTests
{
    public class NearestTileTests : AbstractTestTrainingScenarioSetup
    {

        // test padding 
        [UnityTest]
        public IEnumerator Tile_Count_Padding()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 1, 0, 2, 1,false);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var exitCount = trainingInstanceController
                .Guards[0]
                .GetComponent<AlertGuardAgent>()
                .GetNearestTilePositions(3, trainingInstanceController.TileDict[TileType.ExitTiles]).Count;
            
            Assert.AreEqual(6, exitCount);
        }
    }
}