using System.Collections;
using Agents;
using Enums;
using NUnit.Framework;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestGuardPatrolWithSpyTrainingScenarioSetup : AbstractTestTrainingScenarioSetup
    {
       
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
            
        }

        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Respawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Respawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.1f);
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Alert_Agent_Is_Alert()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return null;
            Assert.IsNotNull( trainingInstanceController.GuardClones[0].GetComponent<GuardPatrolAgent>());
        }
        
        
    }
}