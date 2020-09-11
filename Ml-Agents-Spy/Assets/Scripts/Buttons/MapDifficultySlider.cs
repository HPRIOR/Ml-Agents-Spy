using System.Collections;
using System.Collections.Generic;
using Training;
using UnityEngine;

public class MapDifficultySlider : MonoBehaviour
{
    public GameObject instanceControllerPrefab;
    private TrainingInstanceController _instanceController;
    void Start()
    {
        _instanceController = instanceControllerPrefab.GetComponent<TrainingInstanceController>();
    }

    public void ChangeMapDifficulty(float value)
    {
        _instanceController.mapDifficulty = (int) value;
    }
}

