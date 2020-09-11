using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSizeSlider : GetTrainingInstance
{
   
   public void ChangeMapSize(float size)
   {
      switch (size)
      {
         case 1:
            _instanceController.mapScale = 1;
            break;
         case 2:
            _instanceController.mapScale = 2;
            break;
         case 3:
            _instanceController.mapScale = 3;
            break;
         case 4:
            _instanceController.mapScale = 4;
            break;
         case 5:
            _instanceController.mapScale = 5;
            break;
      }
   }
}
