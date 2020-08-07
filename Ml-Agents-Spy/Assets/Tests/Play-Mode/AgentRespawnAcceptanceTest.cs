using System.Collections;
using Agents;
using Enums;
using EnvSetup;
using NUnit.Framework;
using Training;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class AgentRespawnAcceptanceTest 
    {
        
        [SetUp]
        protected void Init()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        [TearDown]
        protected void TearDown()
        {
            Academy.Instance.Dispose();
        }
        
        private int _iterations = 1000;
        GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
        
        private TrainingInstanceController GetTrainingInstanceController(TrainingScenario trainingScenario)
        {
            GameObject trainingInstance =
                GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            
            
            return trainingInstanceController;
        }

        private void ConfigureTrainingScenario(TrainingInstanceController trainingInstanceController, int mapDifficulty, int mapScale, int exitCount, int guardAgentCount)
        {
            
            trainingInstanceController.debugSetup = true;
            trainingInstanceController.mapDifficulty = mapDifficulty;
            trainingInstanceController.mapScale = mapScale;
            trainingInstanceController.exitCount = exitCount;
            trainingInstanceController.guardAgentCount = guardAgentCount;
            trainingInstanceController.hasMiddleTiles = true;
        }
        
        
        

        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardAlert);
            
            
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardAlert);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardAlert);
          
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardAlert);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardAlert);
            yield return new WaitForSeconds(1);
            ConfigureTrainingScenario(trainingInstanceController, 5, 1, 2, 1);
            yield return new WaitForSeconds(1);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        // guard patrol
        
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrol);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrol);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrol);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrol);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrol);
         
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }
        
        // guard patrol with spy
        
        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        // spy path finding
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyPathFinding);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyPathFinding);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyPathFinding);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyPathFinding);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyPathFinding);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        // spy evade
        
         [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyEvade);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyEvade);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        
        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyEvade);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyEvade);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }
        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = GetTrainingInstanceController(TrainingScenario.SpyEvade);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (int i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }

        
    }
}