using System;
using Agents;
using Enums;
using Training;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    TrainingInstanceController _instanceController;
    void Start()
    {
        var trainingInstanceObject = GameObject.FindWithTag("traininginstance");
        _instanceController = trainingInstanceObject.GetComponent<TrainingInstanceController>();
    }

    public void ResetEnv()
    {
        switch (_instanceController.trainingScenario)
        {
            case TrainingScenario.SpyEvade:
            case TrainingScenario.GuardPatrolWithSpy:
            case TrainingScenario.GuardAlert:
                _instanceController.Spy.GetComponent<SpyAgent>().EndEpisode();
                break;
            case TrainingScenario.GuardPatrol:
                _instanceController.Guards[0].GetComponent<PatrolGuardAgent>().EndEpisode();
                break;
        }

    }
}
