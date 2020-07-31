using System.Collections;
using System.Collections.Generic;
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
    public class NewTestScript
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
        
        GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
        private TrainingInstanceController ConfigureCurriculum(TrainingScenario inputTrainingScenario, CurriculumEnum curriculum)
        {
            
            GameObject trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.trainingScenario = inputTrainingScenario;
            trainingInstanceController.curriculum = curriculum;
            trainingInstanceController.debugSetup = false;
            return trainingInstanceController;
        }

        private  TrainingInstanceController ConfigureDebug(TrainingScenario trainingScenario)
        {
            GameObject trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
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
        // Guard Patrol
       
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Spawn_Count_Basic_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
            
        }

        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Spawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Respawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Respawn_Count_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Spawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Spawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Respawn_Basic_Debug()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.SimpleTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Spawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Spawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Respawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_Guard_Respawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(2, trainingInstanceController.GuardClones.Count);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Spawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitForSeconds(0.1f);
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Spawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Respawn_Advanced_Debug()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetAdvancedDebug(trainingInstanceController);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
        
        [UnityTest]
        public IEnumerator Test_Guard_Patrol_No_Spy_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                ConfigureCurriculum(TrainingScenario.GuardPatrol, CurriculumEnum.AdvancedTestCurriculum);
            yield return  null;
            var guard = trainingInstanceController.GuardClones[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNull(trainingInstanceController.SpyPrefabClone);
        }
    
    }
}
