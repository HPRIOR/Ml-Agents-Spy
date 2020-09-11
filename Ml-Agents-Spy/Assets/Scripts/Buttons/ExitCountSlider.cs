using UnityEngine;

namespace Buttons
{
    public class ExitCountSlider : GetTrainingInstance
    {

        public void ChangeExitCount(float value)
        {
            _instanceController.exitCount = (int) value;
            _instanceController.guardAgentCount = (int) value - 1;
        }
    }
}
