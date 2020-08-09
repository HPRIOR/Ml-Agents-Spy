using System.Collections;
using System.Linq;
using Agents;
using Enums;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Training;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NormalisedFloatsTest : AbstractTestTrainingScenarioSetup
    {

        private TrainingInstanceController GetTrainingInstance(int mapScale)
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, mapScale, 0,
                2, 1, false);
            return trainingInstanceController;
        }

        [UnityTest]
        public IEnumerator Nearest_Exit_Tile_Is_Roughly_1_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.ExitTiles])[1];
            Debug.Log(x);
            Assert.That(x, Is.EqualTo(1).Within(0.001));
        }

        [UnityTest]
        public IEnumerator Nearest_Exit_Tile_Is_Roughly_1_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.ExitTiles])[1];
            Debug.Log(x);
            Assert.That(x, Is.EqualTo(1).Within(0.001));
        }

        [UnityTest]
        public IEnumerator Nearest_Exit_Tile_Is_Roughly_1_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstance(3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.ExitTiles])[1];
            Debug.Log(x);
            Assert.That(x, Is.EqualTo(1).Within(0.001));
        }

        [UnityTest] public IEnumerator Tile_Is_Centre_Is_0_Point_5_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            var agentObject = trainingInstanceController.Spy;
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
           
            var middleTile = trainingInstanceController.TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (3, 3));
            
            agentScript.transform.position = middleTile.Position;
            
            var y = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[1];
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[0];
            Debug.Log(y);
            Assert.That(y, Is.EqualTo(0.5).Within(0.001));
            Assert.That(x, Is.EqualTo(0.5).Within(0.001));
        }
        
        [UnityTest]
        public IEnumerator Tile_Is_Centre_Is_0_Point_5_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            var agentObject = trainingInstanceController.Spy;
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
           
            var middleTile = trainingInstanceController.TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (3, 3));

            //TODO get middle tile for scale 2 and run the same test 
            
            agentScript.transform.position = middleTile.Position;
            
            var y = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[1];
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[0];
            Debug.Log(y);
            Assert.That(y, Is.EqualTo(0.5).Within(0.001));
            Assert.That(x, Is.EqualTo(0.5).Within(0.001));
        }
        
        // Todo Specific Guard position tests 
        
    }
}