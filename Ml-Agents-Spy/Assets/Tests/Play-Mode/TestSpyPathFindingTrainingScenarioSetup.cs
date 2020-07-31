using System.Collections;
using Enums;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSpyPathFindingTrainingScenarioSetup : AbstractTestTrainingScenarioSetup
    {
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
