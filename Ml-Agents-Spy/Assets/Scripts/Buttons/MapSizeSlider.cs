using Training;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
   public class MapSizeSlider : MonoBehaviour
   {
      public GameObject instanceControllerPref;
      public GameObject mapDifficultySlider;
      private TrainingInstanceController _instanceController;
      private Slider _difficultySliderScript;

      public void Start()
      {
         _instanceController = instanceControllerPref.GetComponent<TrainingInstanceController>();
         _difficultySliderScript = mapDifficultySlider.GetComponent<Slider>();
      }
      public void ChangeMapSize(float size)
      {
         switch (size)
         {
            case 1:
               _instanceController.mapScale = 1;
               _difficultySliderScript.maxValue = 5;
               break;
            case 2:
               _instanceController.mapScale = 2;
               _difficultySliderScript.maxValue = 20;
               break;
            case 3:
               _instanceController.mapScale = 3;
               _difficultySliderScript.maxValue = 20;
               break;
            case 4:
               _instanceController.mapScale = 4;
               _difficultySliderScript.maxValue = 50;
               break;
            case 5:
               _instanceController.mapScale = 5;
               _difficultySliderScript.maxValue = 100;
               break;
         }
      }
   }
}
