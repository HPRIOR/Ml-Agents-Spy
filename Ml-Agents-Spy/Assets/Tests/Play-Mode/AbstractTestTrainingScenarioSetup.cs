using Enums;
using NUnit.Framework;
using Training;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests
{
    public abstract class AbstractTestTrainingScenarioSetup : AbstractPlayModeTest
    {

        GameObject trainingInstancePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TrainingInstance.prefab");
        protected TrainingInstanceController ConfigureCurriculum(TrainingScenario inputTrainingScenario, CurriculumEnum curriculum)
        {
            
            GameObject trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
            trainingInstanceController.trainingScenario = inputTrainingScenario;
            trainingInstanceController.curriculum = curriculum;
            trainingInstanceController.debugSetup = false;
            return trainingInstanceController;
        }

        protected  TrainingInstanceController ConfigureDebug(TrainingScenario trainingScenario)
        {
            GameObject trainingInstance = GameObject.Instantiate(trainingInstancePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            TrainingInstanceController trainingInstanceController = trainingInstance.GetComponent<TrainingInstanceController>();
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
        }

        protected static void SetAdvancedDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.exitCount = 3;
            trainingInstanceController.mapScale = 3;
            trainingInstanceController.mapDifficulty = 10;
            trainingInstanceController.guardAgentCount = 2;
            trainingInstanceController.hasMiddleTiles = true;
        }

        protected static void SetDebugParameters(TrainingInstanceController trainingInstanceController, int mapScale,
            int mapDifficulty, int exitCount, int guardAgentCount)
        {
            trainingInstanceController.mapDifficulty = mapDifficulty;
            trainingInstanceController.mapScale = mapScale;
            trainingInstanceController.exitCount = exitCount;
            trainingInstanceController.guardAgentCount = guardAgentCount;
        }
        
    }
}