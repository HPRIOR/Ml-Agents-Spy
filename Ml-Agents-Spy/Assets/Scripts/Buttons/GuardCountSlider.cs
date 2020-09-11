using System.Collections;
using System.Collections.Generic;
using Training;
using UnityEngine;
using UnityEngine.UI;

public class GuardCountSlider : MonoBehaviour
{
        public GameObject instanceControllerPref;
        private TrainingInstanceController _instanceController;        


        public void Start()
        {
            _instanceController = instanceControllerPref.GetComponent<TrainingInstanceController>();
        }

        public void ChangeGuardCount(float value)
        {
            _instanceController.guardAgentCount = (int) value;
        }
}
