﻿using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestGetNearestGameObject
    {
        public (GameObject, List<GameObject>) GetThisGameObjectAndNearest()
        {
            var thisGameObject = new GameObject();
            thisGameObject.transform.position = new Vector3(0, 0, 0);

            var nearestGameObject = new GameObject();
            nearestGameObject.transform.position = new Vector3(1f, 0, 0);

            var secondNearestGameObject = new GameObject();
            secondNearestGameObject.transform.position = new Vector3(2f, 0, 0);

            var thirdNearestGameObject = new GameObject();
            thirdNearestGameObject.transform.position = new Vector3(3f, 0, 0);

            var fourthNearestGameObject = new GameObject();
            fourthNearestGameObject.transform.position = new Vector3(4f, 0, 0);

            var fifthNearestGameObject = new GameObject();
            fifthNearestGameObject.transform.position = new Vector3(5f, 0, 0);

            var sixthNearestGameObject = new GameObject();
            sixthNearestGameObject.transform.position = new Vector3(6f, 0, 0);

            var seventhNearestGameObject = new GameObject();
            seventhNearestGameObject.transform.position = new Vector3(7f, 0, 0);

            return (thisGameObject, new List<GameObject>
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
            (var thisGameObject, var nearestList) = GetThisGameObjectAndNearest();

            var nearestOne = thisGameObject.GetNearest(1, nearestList, x => true);
            var nearestThree = thisGameObject.GetNearest(3, nearestList, x => true);
            var nearestFour = thisGameObject.GetNearest(4, nearestList, x => true);


            Assert.AreEqual(1, nearestOne.Count);
            Assert.AreEqual(3, nearestThree.Count);
            Assert.AreEqual(4, nearestFour.Count);
        }

        [Test]
        public void TestZeroCount()
        {
            (var thisGameObject, var nearestList) = GetThisGameObjectAndNearest();

            var nearestZero = thisGameObject.GetNearest(0, nearestList, x => true);

            Assert.AreEqual(0, nearestZero.Count);
        }

        [Test]
        public void TestOverCount()
        {
            (var thisGameObject, var nearestList) = GetThisGameObjectAndNearest();

            var nearestEight = thisGameObject.GetNearest(8, nearestList, x => true);

            Assert.AreEqual(7, nearestEight.Count);
        }

        [Test]
        public void TestNearestDistances()
        {
            (var thisGameObject, var nearestList) = GetThisGameObjectAndNearest();

            var nearestThree = thisGameObject.GetNearest(3, nearestList, x => true);

            Assert.AreEqual(new Vector3(1f, 0, 0), nearestThree[0].transform.position);
        }

        [Test]
        public void TestThirdNearestDistances()
        {
            (var thisGameObject, var nearestList) = GetThisGameObjectAndNearest();

            var nearestThree = thisGameObject.GetNearest(3, nearestList, x => true);

            Assert.AreEqual(new Vector3(3f, 0, 0), nearestThree[2].transform.position);
        }
    }
}