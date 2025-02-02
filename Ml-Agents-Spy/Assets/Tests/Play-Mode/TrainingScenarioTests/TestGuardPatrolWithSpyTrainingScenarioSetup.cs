﻿using System.Collections;
using Agents;
using Enums;
using NUnit.Framework;
using Tests.TestSetup;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.TrainingScenarioTests
{
    public class TestGuardPatrolWithSpyTrainingScenarioSetup : AbstractTestTrainingScenarioSetup
    {
        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Basic_Debug()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrolWithSpy);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Guard_Respawn_Count_Simple_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

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
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.AreEqual(1, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Basic_Debug()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Simple_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Basic_Debug()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            SetBasicDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_No_Spy_Respawn_Simple_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.SimpleTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }


        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Guard_Spawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Respawn_Count_Advanced_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }


        [UnityTest]
        public IEnumerator Test_Respawn_Count_Advanced_Debug()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.AreEqual(2, trainingInstanceController.Guards.Count);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Advanced_Debug()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Spawn_Advanced_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Advanced_Debug()
        {
            var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrolWithSpy);
            SetAdvancedDebug(trainingInstanceController);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Spy_Respawn_Advanced_Curr()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            var guard = trainingInstanceController.Guards[0].GetComponent<Agent>();
            yield return null;
            guard.EndEpisode();
            yield return null;
            Assert.IsNotNull(trainingInstanceController.Spy);
        }

        [UnityTest]
        public IEnumerator Test_Guard_Alert_Agent_Is_Alert()
        {
            var trainingInstanceController =
                GetCurriculumSetup(TrainingScenario.GuardPatrolWithSpy, CurriculumEnum.AdvancedTestCurriculum);
            yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

            Assert.IsNotNull(trainingInstanceController.Guards[0].GetComponent<PatrolGuardAgent>());
        }
    }
}