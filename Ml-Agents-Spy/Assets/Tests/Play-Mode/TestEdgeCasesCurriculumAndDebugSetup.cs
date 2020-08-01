using System.Collections;
using Enums;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestEdgeCasesCurriculumAndDebugSetup : AbstractTestTrainingScenarioSetup
    {
        
        [UnityTest]
        public IEnumerator Test_More_Agents_Than_Exits_No_Exception_Guard_Alert()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardAlert);
            trainingInstanceController.mapScale = 1;
            trainingInstanceController.mapDifficulty = 0;
            trainingInstanceController.exitCount = 2;
            trainingInstanceController.guardAgentCount = 2;
            yield return null;
        }
        [UnityTest]
        public IEnumerator Test_More_Agents_Than_Exits_No_Exception_Guard_Patrol()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrol);
            trainingInstanceController.mapScale = 1;
            trainingInstanceController.mapDifficulty = 0;
            trainingInstanceController.exitCount = 2;
            trainingInstanceController.guardAgentCount = 2;
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator Test_More_Agents_Than_Exits_No_Exception_Guard_SpyEvade()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.SpyEvade);
            trainingInstanceController.mapScale = 1;
            trainingInstanceController.mapDifficulty = 0;
            trainingInstanceController.exitCount = 2;
            trainingInstanceController.guardAgentCount = 2;
            yield return null;
        }
        [UnityTest]
        public IEnumerator Test_More_Agents_Than_Exits_No_Exception_Guard_Patrol_With_Spy()
        {
            var trainingInstanceController = ConfigureDebug(TrainingScenario.GuardPatrolWithSpy);
            trainingInstanceController.mapScale = 1;
            trainingInstanceController.mapDifficulty = 0;
            trainingInstanceController.exitCount = 2;
            trainingInstanceController.guardAgentCount = 2;
            yield return null;
        }
        
        
    }
}