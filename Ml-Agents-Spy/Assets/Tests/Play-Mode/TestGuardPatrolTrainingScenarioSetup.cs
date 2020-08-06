using System.Collections;
using System.Collections.Generic;
using Agents;
using Enums;
using NUnit.Framework;
using Training;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestGuardPatrolTrainingScenarioSetup : AbstractTestTrainingScenarioSetup
    {
       // Guard Patrol
       
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
            
        }

        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Spawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Spawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Spawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.1f);
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Spawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.Spy);
        }
        
        [UnityTest]
        public IEnumerator Test_Agent_Is_Patrol()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return null;
            Assert.IsNotNull( trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>());
        }
    
    }
}
