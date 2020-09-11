using System;
using Agents;
using Enums;
using Training;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public GameObject instanceControllerPref;
    private TrainingInstanceController _instanceController;
    public void Start()
    {
        _instanceController = instanceControllerPref.GetComponent<TrainingInstanceController>();
    } 
    public void ResetEnv()
    {
        switch (_instanceController.trainingScenario)
        {
            case TrainingScenario.SpyEvade:
            case TrainingScenario.GuardPatrolWithSpy:
            case TrainingScenario.GuardAlert:
            case TrainingScenario.SpyPathFinding:
                _instanceController.Spy.GetComponent<SpyAgent>().EndEpisode();
                break;
            case TrainingScenario.GuardPatrol:
                _instanceController.Guards[0].GetComponent<PatrolGuardAgent>().EndEpisode();
                break;
        }

    }
}
