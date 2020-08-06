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
        
        private TrainingInstanceController ConfigTrainingInstanceController(TrainingScenario trainingScenario, int mapDifficulty, int mapScale, int exitCount, int guardAgentCount)
        {
            GameObject trainingInstance =
                GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.trainingScenario = trainingScenario;
            trainingInstanceController.debugSetup = true;
            trainingInstanceController.mapDifficulty = mapDifficulty;
            trainingInstanceController.mapScale = mapScale;
            trainingInstanceController.exitCount = exitCount;
            trainingInstanceController.guardAgentCount = guardAgentCount;
            trainingInstanceController.hasMiddleTiles = true;
            return trainingInstanceController;
        }
        
        // guard alert tests
        
        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardAlert, 100, 5, 6, 5);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardAlert, 50, 4, 5, 4);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardAlert, 20, 3, 4, 3);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardAlert, 10, 2, 3, 2);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardAlert, 5, 1, 2, 1);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrol, 100, 5, 6, 5);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrol, 50, 4, 5, 4);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrol, 20, 3, 4, 3);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrol, 10, 2, 3, 2);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrol, 5, 1, 2, 1);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy, 100, 5, 6, 5);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy, 50, 4, 5, 4);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy, 20, 3, 4, 3);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy, 10, 2, 3, 2);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.GuardPatrolWithSpy, 5, 1, 2, 1);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyPathFinding, 100, 5, 6, 5);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyPathFinding, 50, 4, 5, 4);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyPathFinding, 20, 3, 4, 3);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyPathFinding, 10, 2, 3, 2);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyPathFinding, 5, 1, 2, 1);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyEvade, 100, 5, 6, 5);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyEvade, 50, 4, 5, 4);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyEvade, 20, 3, 4, 3);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyEvade, 10, 2, 3, 2);
            yield return null;
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
            var trainingInstanceController = ConfigTrainingInstanceController(TrainingScenario.SpyEvade, 5, 1, 2, 1);
            yield return null;
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