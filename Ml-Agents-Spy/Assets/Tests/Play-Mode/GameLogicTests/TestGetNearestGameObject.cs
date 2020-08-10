using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.GameLogicTests
{
    public class TestGetNearestGameObject : AbstractPlayModeTest
    {
        [UnityTest]
        public IEnumerator Test_Nearest_GO_is_INIT()
        {
            var agentStub = new GameObject();

            var initInstantiatedStubb = Object.Instantiate(agentStub);
            initInstantiatedStubb.transform.position = new Vector3(0, 0, 0);

            var secondStubb = Object.Instantiate(agentStub);
            secondStubb.transform.position = new Vector3(0, 0, 1f);

            var thirdStubb = Object.Instantiate(agentStub);
            thirdStubb.transform.position = new Vector3(0, 0, 2f);

            var fourthStubb = Object.Instantiate(agentStub);
            fourthStubb.transform.position = new Vector3(0, 0, 3f);

            var stubs = new List<GameObject>
            {
                initInstantiatedStubb, secondStubb, thirdStubb, fourthStubb
            };

            var nearestStub = initInstantiatedStubb.GetNearest(1, stubs, x => true)[0];

            Assert.AreEqual(initInstantiatedStubb.transform.position, nearestStub.transform.position);


            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_Nearest_GO_other_than_Init()
        {
            var agentStub = new GameObject();

            var initInstantiatedStubb = Object.Instantiate(agentStub);
            initInstantiatedStubb.transform.position = new Vector3(0, 0, 0);

            var secondStubb = Object.Instantiate(agentStub);
            secondStubb.transform.position = new Vector3(0, 0, 1f);

            var thirdStubb = Object.Instantiate(agentStub);
            thirdStubb.transform.position = new Vector3(0, 0, 2f);

            var fourthStubb = Object.Instantiate(agentStub);
            fourthStubb.transform.position = new Vector3(0, 0, 3f);

            var stubs = new List<GameObject>
            {
                initInstantiatedStubb, secondStubb, thirdStubb, fourthStubb
            };

            var nearestStub = initInstantiatedStubb.GetNearest(1, stubs,
                x => x.gameObjectDistance.GetInstanceID() != initInstantiatedStubb.GetInstanceID())[0];

            Assert.AreEqual(nearestStub.transform.position, secondStubb.transform.position);

            yield return null;
        }

        
    }
}