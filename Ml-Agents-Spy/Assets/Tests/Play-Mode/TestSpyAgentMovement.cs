using System.Collections;
using Agents;
using JetBrains.Annotations;
using NUnit.Framework;
using Training;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.MLAgents;
using static TileHelper;
using Enums;
using System.Linq;
using Interfaces;


namespace Tests
{
    public class TestSpyAgentMovement
    {
        GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
        
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [NotNull]
        public IEnumerator TestInitialPositionMapScale1()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [NotNull]
        public IEnumerator TestInitialPositionMapScale2()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 2;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
            
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestInitialPositionMapScale3()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        [UnityTest]
        [NotNull]
        public IEnumerator TestMoveUp()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionY();
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            Assert.Greater(agentScript.PositionY(), initialPosition);

        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestMoveDown()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionY();
            agentScript.MoveAgent(2f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(2f);
            yield return new  WaitForSeconds(0.1f);
            Assert.Less(agentScript.PositionY(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestMoveLeft()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionX();
            agentScript.MoveAgent(4f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(4f);
            yield return new  WaitForSeconds(0.1f);
            Assert.Less(agentScript.PositionX(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestMoveRight()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionX();
            agentScript.MoveAgent(3f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(3f);
            yield return new  WaitForSeconds(0.1f);
            Assert.Greater(agentScript.PositionX(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestDistanceFromExitChange()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var exitInitialDistance = agentScript.DistanceToNearestExit(GetNearestTile(tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            agentScript.MoveAgent(3f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(3f);
            yield return new  WaitForSeconds(0.1f);
            var newExitDistance = agentScript.DistanceToNearestExit(GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            Assert.Less(newExitDistance, exitInitialDistance);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestCloseToExit()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            trainingInstanceController.TrainingScenario = TrainingScenario.SpyPathFinding;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Vector3 exitTile = GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position;

            spyPrefab.transform.position = exitTile - new Vector3(0, 0,1.5f);
            yield return new  WaitForSeconds(0.1f);
            var exitDistance = agentScript.DistanceToNearestExit(GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            
            Assert.That(exitDistance, Is.EqualTo(0).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestFallOffEdge()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();

            var transform = spyPrefab.transform;
            transform.position -= new Vector3(0, 10, 0);
            yield return new  WaitForSeconds(0.1f);
            
            
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestRespawnPoint()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            yield return new  WaitForSeconds(0.1f);
            var transform = spyPrefab.transform;
            transform.position = GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position;
            
            yield return new  WaitForSeconds(0.1f);
            
            
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
        }
        
        
        [UnityTest]
        [NotNull]
        public IEnumerator TestCollision()
        {
            var trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.AreEqual( 0, agentScript.IsColliding);
            agentScript.MoveAgent(2f);
            for (int i = 0; i < 20; i++)
            {
                yield return new  WaitForSeconds(0.1f);
                agentScript.MoveAgent(2f);
            }
            Assert.AreEqual( 1, agentScript.IsColliding);
        }
    }
}
