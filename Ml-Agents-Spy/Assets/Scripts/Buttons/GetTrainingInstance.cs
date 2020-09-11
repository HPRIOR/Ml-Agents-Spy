using System.Collections;
using System.Collections.Generic;
using Training;
using UnityEngine;

public abstract class GetTrainingInstance : MonoBehaviour

{
     TrainingInstanceController _instanceController;
     void Start()
     {
         var trainingInstanceObject = GameObject.FindWithTag("traininginstance");
         _instanceController = trainingInstanceObject.GetComponent<TrainingInstanceController>();
     }

}
