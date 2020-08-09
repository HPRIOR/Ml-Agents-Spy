using System.Collections;
using Enums;
using NUnit.Framework;
using UnityEngine;
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
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(0, trainingInstanceController.Guards.Count);

        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.NotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.NotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agents_Respawned_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.Spy.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.Spy);
            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agents_Respawned_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.Spy.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.Spy);
            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

//
        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_No_Guards_Spawned_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.NotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Path_Finding_Agent_Spawned_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            Assert.NotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_PathFinding_Agent_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.Spy.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.Spy);
            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_PathFinding_Agent_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.SpyPathFinding, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var firstExitPosition = trainingInstanceController.TileDict[TileType.ExitTiles][0];
            trainingInstanceController.Spy.transform.position = firstExitPosition.Position;
            yield return null;
            Assert.NotNull(trainingInstanceController.Spy);
            Assert.AreEqual(0, trainingInstanceController.Guards.Count);
        }

    }
}
