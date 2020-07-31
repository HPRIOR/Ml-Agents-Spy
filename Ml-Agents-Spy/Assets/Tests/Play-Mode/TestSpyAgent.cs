using System.Collections;
using Agents;
using JetBrains.Annotations;
using NUnit.Framework;
using Training;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static TileHelper;
using Enums;
using Interfaces;


namespace Tests
{
    public class TestSpyAgent : AbstractPlayModeTest
    {
        GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");

        
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Initial_Position_Map_Scale_1()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Initial_Position_Map_Scale_2()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;            
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 2;
            yield return null;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
            
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Initial_Position_Map_Scale_3()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return null;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Move_Up()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return null;   
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionY();
            agentScript.MoveAgent(1f);
            yield return null;   
            agentScript.MoveAgent(1f);
            yield return null;   
            Assert.Greater(agentScript.PositionY(), initialPosition);

        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Move_Down()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionY();
            agentScript.MoveAgent(2f);
            yield return null;   
            agentScript.MoveAgent(2f);
            yield return null;   
            Assert.Less(agentScript.PositionY(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Move_Left()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return null;   
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionX();
            agentScript.MoveAgent(4f);
            yield return null;   
            agentScript.MoveAgent(4f);
            yield return null;   
            Assert.Less(agentScript.PositionX(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Move_Right()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return null;   
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var initialPosition = agentScript.PositionX();
            agentScript.MoveAgent(3f);
            yield return null;   
            agentScript.MoveAgent(3f);
            yield return null;   
            Assert.Greater(agentScript.PositionX(), initialPosition);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Distance_From_Exit_Change()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return null;   
            var tileDict = trainingInstanceController.TileDict;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var exitInitialDistance = agentScript.DistanceToNearestExit(GetNearestTile(tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            var newExitDistance = agentScript.DistanceToNearestExit(
                GetNearestTile(
                    tileDict[TileType.ExitTiles].
                        ConvertAll(tile => (ITile) tile), spyPrefab)
                    .Position);
            Assert.Less(newExitDistance, exitInitialDistance);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Close_To_Exit()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            trainingInstanceController.trainingScenario = TrainingScenario.SpyPathFinding;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Vector3 exitTile = GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position;

            spyPrefab.transform.position = exitTile - new Vector3(0, 0,1.5f);
            yield return null;   
            var exitDistance = agentScript.DistanceToNearestExit(GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            
            Assert.That(exitDistance, Is.EqualTo(0).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Fall_Off_Edge()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale =1;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();

            var transform = spyPrefab.transform;
            transform.position -= new Vector3(0, 200, 0);
            yield return new  WaitForSeconds(0.1f);
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Respawn_Point()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 1;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            yield return new  WaitForSeconds(0.1f);
            var transform = spyPrefab.transform;
            yield return null;   
            agentScript.EndEpisode();
            yield return null;   
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Collision()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            yield return null;   
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
        
        // local position tests
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Initial_Position_Map_Scale_1()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Initial_Position_Map_Scale_2()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 2;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
            
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Initial_Position_Map_Scale_3()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            yield return new  WaitForSeconds(0.1f);
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));

        }
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Move_Up()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
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
        public IEnumerator Test_Relative_Move_Down()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
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
        public IEnumerator Test_Relative_Move_Left()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
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
        public IEnumerator Test_Relative_Move_Right()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
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
        public IEnumerator Test_Relative_Distance_From_Exit_Change()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            var exitInitialDistance = agentScript.DistanceToNearestExit(GetNearestTile(tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            agentScript.MoveAgent(1f);
            yield return new  WaitForSeconds(0.1f);
            var newExitDistance = agentScript.DistanceToNearestExit(
                GetNearestTile(
                    tileDict[TileType.ExitTiles].
                        ConvertAll(tile => (ITile) tile), spyPrefab)
                    .Position);
            Assert.Less(newExitDistance, exitInitialDistance);
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Close_To_Exit()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            trainingInstanceController.trainingScenario = TrainingScenario.SpyPathFinding;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            Vector3 exitTile = GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position;

            spyPrefab.transform.position = exitTile - new Vector3(0, 0,1.5f);
            yield return null;   
            var exitDistance = agentScript.DistanceToNearestExit(GetNearestTile(
                tileDict[TileType.ExitTiles].ConvertAll(tile => (ITile) tile),
                spyPrefab).Position);
            
            Assert.That(exitDistance, Is.EqualTo(0).Within(0.1));
        }
        
        [UnityTest]
        [NotNull]
        public IEnumerator Test_Relative_Respawn_Point()
        {
            var trainingInstance = Object.Instantiate(trainingInstancePrefab, new Vector3(100, 100, 100), Quaternion.identity);
            yield return null;   
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.MapScale = 1;
            trainingInstanceController.HasMiddleTiles = false;
            yield return new  WaitForSeconds(0.1f);
            var tileDict = trainingInstanceController.TileDict;
            Transform spyPrefab = trainingInstance.transform.Find("Spy(Clone)");
            SpyAgent agentScript = spyPrefab.GetComponent<SpyAgent>();
            yield return null;
            agentScript.EndEpisode();
            yield return null;   
            Assert.That(agentScript.PositionY(), Is.EqualTo(0f).Within(0.1));
        }
    }
}
