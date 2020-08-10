using Enums;
using Training;
using UnityEditor;
using UnityEngine;

namespace Tests.TestSetup
{
    public abstract class AbstractTestTrainingScenarioSetup : AbstractPlayModeTest
    {
        readonly GameObject _trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
        protected TrainingInstanceController ConfigureCurriculum(TrainingScenario inputTrainingScenario, CurriculumEnum curriculum)
        {
            GameObject trainingInstance = Object.Instantiate(_trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.waitForTestSetup = true;
            trainingInstanceController.trainingScenario = inputTrainingScenario;
            trainingInstanceController.curriculum = curriculum;
            trainingInstanceController.debugSetup = false;
            trainingInstanceController.waitForTestSetup = false;
            return trainingInstanceController;
        }

        protected  TrainingInstanceController ConfigureDebug(TrainingScenario trainingScenario)
        {
            GameObject trainingInstance = Object.Instantiate(_trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.waitForTestSetup = true;
            trainingInstanceController.trainingScenario = trainingScenario;
            trainingInstanceController.debugSetup = true;
            return trainingInstanceController;
        }

        protected static void SetBasicDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.exitCount = 2;
            trainingInstanceController.mapScale = 1;
            trainingInstanceController.mapDifficulty = 0;
            trainingInstanceController.guardAgentCount = 1;
            trainingInstanceController.hasMiddleTiles = true;
            trainingInstanceController.waitForTestSetup = false;
        }

        protected static void SetAdvancedDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.exitCount = 3;
            trainingInstanceController.mapScale = 3;
            trainingInstanceController.mapDifficulty = 10;
            trainingInstanceController.guardAgentCount = 2;
            trainingInstanceController.hasMiddleTiles = true;
            trainingInstanceController.waitForTestSetup = false;
        }

        protected static void SetDebugParameters(TrainingInstanceController trainingInstanceController, int mapScale,
            int mapDifficulty, int exitCount, int guardAgentCount, bool hasMiddleTiles = true)
        {
            trainingInstanceController.mapDifficulty = mapDifficulty;
            trainingInstanceController.mapScale = mapScale;
            trainingInstanceController.exitCount = exitCount;
            trainingInstanceController.guardAgentCount = guardAgentCount;
            trainingInstanceController.hasMiddleTiles = hasMiddleTiles;
            trainingInstanceController.waitForTestSetup = false;
        }
        
    }
}