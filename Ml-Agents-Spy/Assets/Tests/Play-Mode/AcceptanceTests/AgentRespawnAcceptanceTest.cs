using System.Collections;
using Agents;
using Enums;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AcceptanceTests
{
    public class AgentRespawnAcceptanceTest : AbstractTestTrainingScenarioSetup
    {
        private readonly int _iterations = 1000;

        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 5, 100, 6, 5);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 6; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 4, 50, 5, 4);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            yield return null;
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 5; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 3, 20, 4, 3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 4; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 2, 10, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 3; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Alert_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 2; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        // guard patrol
        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetDebugParameters(trainingInstanceController, 5, 100, 6, 5);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 5; j++) guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetDebugParameters(trainingInstanceController, 4, 50, 5, 4);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 4; j++) guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetDebugParameters(trainingInstanceController, 3, 20, 4, 3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 3; j++) guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetDebugParameters(trainingInstanceController, 2, 10, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 2; j++) guardPatrolAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var guardPatrolAgent = trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>();
            for (var i = 0; i < _iterations; i++)
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
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetDebugParameters(trainingInstanceController, 5, 100, 6, 5);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 6; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetDebugParameters(trainingInstanceController, 4, 50, 5, 4);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 5; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetDebugParameters(trainingInstanceController, 3, 20, 4, 3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 4; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetDebugParameters(trainingInstanceController, 2, 10, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 3; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Guard_Patrol_With_Spy_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 2; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        // spy path finding
        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_5()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 5, 100, 6, 5);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 4, 50, 5, 4);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 3, 20, 4, 3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 2, 10, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Path_Finding_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyPathFinding);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
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
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            SetDebugParameters(trainingInstanceController, 5, 100, 6, 5);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 6; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_4()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            SetDebugParameters(trainingInstanceController, 4, 50, 5, 4);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 5; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_3()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            SetDebugParameters(trainingInstanceController, 3, 20, 4, 3);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 4; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_2()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            SetDebugParameters(trainingInstanceController, 2, 10, 3, 2);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 3; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Spy_Evade_Respawn_Acceptance_Map_Scale_1()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            SetDebugParameters(trainingInstanceController, 1, 5, 2, 1);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);
            var spyAgent = trainingInstanceController.Spy.GetComponent<SpyAgent>();
            for (var i = 0; i < _iterations; i++)
            {
                yield return null;
                for (var j = 0; j < 2; j++) spyAgent.EndEpisode();
                yield return null;
            }
        }
    }
}