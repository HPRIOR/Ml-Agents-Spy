using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestGetNearestGameObject
    {
       
        
        public (GameObject, List<GameObject>) GetThisGameObjectAndNearest()
        {
            GameObject thisGameObject = new GameObject();
            thisGameObject.transform.position = new Vector3(0, 0, 0);
            
            GameObject nearestGameObject = new GameObject();
            nearestGameObject.transform.position = new Vector3(1f, 0,0);
            
            GameObject secondNearestGameObject = new GameObject();
            secondNearestGameObject.transform.position = new Vector3(2f, 0,0);
            
            GameObject thirdNearestGameObject = new GameObject();
            thirdNearestGameObject.transform.position = new Vector3(3f, 0,0);
            
            GameObject fourthNearestGameObject = new GameObject();
            fourthNearestGameObject.transform.position = new Vector3(4f, 0,0);
            
            GameObject fifthNearestGameObject = new GameObject();
            fifthNearestGameObject.transform.position = new Vector3(5f, 0,0);
            
            GameObject sixthNearestGameObject = new GameObject();
            sixthNearestGameObject.transform.position = new Vector3(6f, 0,0);
            
            GameObject seventhNearestGameObject = new GameObject();
            seventhNearestGameObject.transform.position = new Vector3(7f, 0,0);

            return (thisGameObject, new List<GameObject>()
            {
                nearestGameObject, 
                secondNearestGameObject, 
                thirdNearestGameObject, 
                fourthNearestGameObject, 
                fifthNearestGameObject, 
                sixthNearestGameObject, 
                seventhNearestGameObject
            });
        }

        [Test]
        public void TestNearestCount()
        {
            (GameObject thisGameObject, List<GameObject> nearestList) = GetThisGameObjectAndNearest();
            
            thisGameObject.GetNearest()
        }
        
        
    }
}
