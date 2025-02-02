﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestPadList
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Test_Small_List_Of_Ints_Pad_1()
        {
            var l1 = new List<int>
            {
                1, 2, 3, 4
            };
            Debug.Log(l1.PadList(5, 0));

            Assert.AreEqual( new List<int> {1, 2, 3, 4, 0}, l1.PadList(5, 0).ToList());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void Test_Small_List_Of_Ints_Pad_5()
        {
            var l1 = new List<int>
            {
                1, 2, 3, 4, 5
            };

            Assert.AreEqual(new List<int> {1, 2, 3, 4, 5, 0, 0, 0, 0, 0}, l1.PadList(10, 0).ToList());
        }


        // A Test behaves as an ordinary method
        [Test]
        public void Test_Small_List_Of_float_pad_5()
        {
            var l1 = new List<float>
            {
                1.1f, 2.1f, 3.1f, 4.1f, 5.1f
            };

            Assert.AreEqual(new List<float> {1.1f, 2.1f, 3.1f, 4.1f, 5.1f, 1.1f, 1.1f, 1.1f, 1.1f, 1.1f}, 
                l1.PadList(10, 1.1f).ToList());
        }
    }
}