using Training;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
    public class ExitCountSlider : MonoBehaviour
    {
        public GameObject instanceControllerPref;
        public GameObject guardCount;
        
        private Slider _guardCountSlider;
        private TrainingInstanceController _instanceController;


        public void Start()
        {
            _guardCountSlider = guardCount.GetComponent<Slider>();
            _instanceController = instanceControllerPref.GetComponent<TrainingInstanceController>();
        }

        public void ChangeExitCount(float value)
        {
            _instanceController.exitCount = (int) value;
            _instanceController.guardAgentCount = (int) value - 1;
            _guardCountSlider.maxValue = (int) value - 1;
        }
    }
}
