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

        protected void SetBasicDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.ExitCount = 2;
            trainingInstanceController.MapScale = 1;
            trainingInstanceController.MapDifficulty = 0;
            trainingInstanceController.GuardAgentCount = 1;
            trainingInstanceController.HasMiddleTiles = true;
        }

        protected void SetAdvancedDebug(TrainingInstanceController trainingInstanceController)
        {
            trainingInstanceController.ExitCount = 3;
            trainingInstanceController.MapScale = 3;
            trainingInstanceController.MapDifficulty = 10;
            trainingInstanceController.GuardAgentCount = 2;
            trainingInstanceController.HasMiddleTiles = true;
        }
        
    }
}