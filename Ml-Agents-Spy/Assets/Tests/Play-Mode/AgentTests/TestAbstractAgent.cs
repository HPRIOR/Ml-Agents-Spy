using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agents;
using Enums;
using NUnit.Framework;
using Tests.TestSetup;
using Training;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AgentTests
{
    public class TestAbstractAgent : AbstractAgentTestSetup
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

        [UnityTest] public IEnumerator Tile_Centre_Is_0_Point_5_Map_Scale_1()
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
        public IEnumerator Tile_Centre_Is_0_Point_5_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            var agentObject = trainingInstanceController.Spy;
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
           
            var middleTile = trainingInstanceController.TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (5, 5));
            
            
            agentScript.transform.position = middleTile.Position;
            
            var y = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[1];
            var x = agentScript.GetNearestTilePositions(1, trainingInstanceController.TileDict[TileType.FreeTiles])[0];
            Debug.Log(y);
            Assert.That(y, Is.EqualTo(0.5).Within(0.001));
            Assert.That(x, Is.EqualTo(0.5).Within(0.001));
        }

        [UnityTest]
        public IEnumerator Tile_Surrounding_Agent_Are_Correctly_Given()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            var agentObject = trainingInstanceController.Spy;
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
           
            var middleTile = trainingInstanceController.TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (5, 5));
            
            
            agentScript.transform.position = middleTile.Position;

            var surroundingTilesPositions =
                agentScript.GetNearestTilePositions(9, trainingInstanceController.TileDict[TileType.FreeTiles]);
            
            var surroundingTilePositionTuples = new List<(float, float)>();

            for (int i = 0; i < surroundingTilesPositions.Count - 1; i++)
            {
                surroundingTilePositionTuples.Add((surroundingTilesPositions[i],surroundingTilesPositions[i+1]));
            }
            
            var positionsAroundCenter = GetPositionsAroundCenter(trainingInstanceController, 5, 2);

            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[0]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[1]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[2]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[3]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[4]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[5]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[6]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[7]));
            
        }

        
        
        [UnityTest]
        public IEnumerator Offset_Tile_Surrounding_Agent_Are_Correctly_Given()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            trainingInstanceController.transform.position = new Vector3(100, 100, 100);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            
            var agentObject = trainingInstanceController.Spy;
            var agentScript = trainingInstanceController.Spy.GetComponent<SpyAgent>();
           
            var middleTile = trainingInstanceController.TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (5, 5));
            
            
            agentScript.transform.position = middleTile.Position;

            var surroundingTilesPositions =
                agentScript.GetNearestTilePositions(9, trainingInstanceController.TileDict[TileType.FreeTiles]);
            
            var surroundingTilePositionTuples = new List<(float, float)>();

            for (int i = 0; i < surroundingTilesPositions.Count - 1; i++)
            {
                surroundingTilePositionTuples.Add((surroundingTilesPositions[i],surroundingTilesPositions[i+1]));
            }

            var positionsAroundCenter = GetPositionsAroundCenter(trainingInstanceController, 5, 2);

            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[0]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[1]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[2]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[3]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[4]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[5]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[6]));
            Assert.True(surroundingTilePositionTuples.Any(x => x == positionsAroundCenter[7]));
            
        }
        
        
        [UnityTest]
        public IEnumerator Normalised_Center_Position_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (3, 3));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());

        }
        
        [UnityTest]
        public IEnumerator Normalised_Center_Position_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (5, 5));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());
            
        }
        
        [UnityTest]
        public IEnumerator Normalised_Center_Position_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstance(3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (8, 8));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());
            
        }
        
        [UnityTest]
        public IEnumerator Normalised_Offset_Center_Position_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstance(1);
            trainingInstanceController.transform.position += new Vector3(100, 100, 100);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (3, 3));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());

        }
        
        [UnityTest]
        public IEnumerator Normalised_Offset_Center_Position_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstance(2);
            trainingInstanceController.transform.position += new Vector3(100, 100, 100);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (5, 5));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());
            
        }
        
        [UnityTest]
        public IEnumerator Normalised_Offset_Center_Position_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstance(3);
            trainingInstanceController.transform.position += new Vector3(100, 100, 100);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var agentObject = trainingInstanceController.Spy;
            var agentScript = agentObject.GetComponent<SpyAgent>();
            var midTile = trainingInstanceController.TileDict[TileType.FreeTiles].First(t => t.Coords == (8, 8));
            agentObject.transform.position = midTile.Position;
            
            Assert.AreEqual(0.5,agentScript.NormalisedPositionX());
            Assert.AreEqual(0.5,agentScript.NormalisedPositionY());
            
        }
        
        
        
        
        
        
        
        
        
        


    }
}