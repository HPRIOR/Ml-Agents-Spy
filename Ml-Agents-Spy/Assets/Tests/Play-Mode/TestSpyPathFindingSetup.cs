using System.Collections;
using Enums;
using JetBrains.Annotations;
using NUnit.Framework;
using Training;
using Unity.Collections;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSpyPathFindingSetup
    {
        [SetUp]
        public void Init()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        [TearDown]
        public void TearDown()
        {
            Academy.Instance.Dispose();
        }

        GameObject trainingInstancePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");

        private TrainingInstanceController ConfigureCurriculum(TrainingScenario inputTrainingScenario,
            CurriculumEnum curriculum)
        {

            GameObject trainingInstance =
                GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController =
                trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.trainingScenario = inputTrainingScenario;
            trainingInstanceController.curriculum = curriculum;
            trainingInstanceController.debugSetup = false;
            return trainingInstanceController;
        }

        private TrainingInstanceController ConfigureDebug(TrainingScenario trainingScenario)
        {
            GameObject trainingInstance =
                GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController =
                trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.trainingScenario = trainingScenario;
            trainingInstanceController.debugSetup = true;
            return trainingInstanceController;
        }



        private void SetBasicDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.ExitCount = 2;
            trainingInstanceController.MapScale = 1;
            trainingInstanceController.MapDifficulty = 0;
            trainingInstanceController.GuardAgentCount = 1;
            trainingInstanceController.HasMiddleTiles = true;
        }

        private void SetAdvancedDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.ExitCount = 3;
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.MapDifficulty = 10;
            trainingInstanceController.GuardAgentCount = 2;
            trainingInstanceController.HasMiddleTiles = true;
        }

        // Spy Path Finding 

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return null;
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return null;
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);

        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetAdvancedDebug(trainingInstanceController);
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agents_Respawned_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return null;
            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.SpyPrefabClone.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agents_Respawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return null;
            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.SpyPrefabClone.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

//
        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return null;
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return null;
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
        }

        [UnityTest]
        public IEnumerator Test_Spy_PathFinding_Agent_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return null;
            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.SpyPrefabClone.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_PathFinding_Agent_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return null;
            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.SpyPrefabClone.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.SpyPrefabClone);
            Assert.AreEqual(0, trainingInstanceController.GuardClones.Count);
        }

    }
}
