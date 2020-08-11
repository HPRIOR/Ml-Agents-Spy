using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace TestScripts
{
    public class TestScript
    {

        public void GetSensorStuff()
        {
            
            // this is a GetComponent<> in the real code
            // RayPerceptionSensorComponent attached to child gameobject of agent gameobject
            // This object is rotated to move sensor around
            RayPerceptionSensorComponent3D rayComponent = new RayPerceptionSensorComponent3D();
        
            var lengthOfRayOutputs = RayPerceptionSensor
                .Perceive(rayComponent.GetRayPerceptionInput())
                .RayOutputs
                .Length;
            
            var rayDistances = new float[5];
            
            List<float[]> rayBuffers = new List<float[]>()
            {
                new float[(2 + 2) * lengthOfRayOutputs],
                new float[(2 + 2) * lengthOfRayOutputs],
                new float[(2 + 2) * lengthOfRayOutputs],
                new float[(2 + 2) * lengthOfRayOutputs],
                new float[(2 + 2) * lengthOfRayOutputs]
            };

            var rayOutputs = RayPerceptionSensor
                .Perceive(rayComponent.GetRayPerceptionInput())
                .RayOutputs;
                
            for (int i = 0; i < 5; i++)
                rayOutputs[i].ToFloatArray(2, 0, rayBuffers[i]);
            
            // add just the distances to a new float array which represents all the ray cast distances
            var distances1 = rayBuffers[0][3];
            var distances2 = rayBuffers[1][3];
            var distances3 = rayBuffers[2][3];
            var distances4 = rayBuffers[3][3];
            var distances5 = rayBuffers[4][3];
            
            // I want to convert these distances into Vector3's
            // assuming this script is attached to gameobject the rays are cast from
            //RayCastHitLocation(
            //    transform.rotation,
            //    transform.position,
            //    distances1.ReverseNormalise(rayComponent.RayLength)
            //);
            
        }
        
        public static Vector3 RayCastHitLocation(Quaternion rotation, Vector3 position, float distance)
        {
            Vector3 direction = rotation * Vector3.forward;
            return position - (direction  * distance);
        }
        
    }
}