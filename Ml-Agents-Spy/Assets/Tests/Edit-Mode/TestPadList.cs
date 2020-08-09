using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestPadList
    {
        // A Test behaves as an ordinary method
        [Test] public void Test_Small_List_Of_Ints_Pad_1()
        {
            List<int> l1 = new List<int>()
            {
                1,2,3,4
            };
            Debug.Log(l1.PadList(5, 0));
            
            Assert.AreEqual(l1.PadList(5, 0), new List<int>(){1,2,3,4,0});
        }
        
        // A Test behaves as an ordinary method
        [Test] public void Test_Small_List_Of_Ints_Pad_5()
        {
            List<int> l1 = new List<int>()
            {
                1,2,3,4,5
            };

            Assert.AreEqual(l1.PadList(10, 0), new List<int>(){1,2,3,4,5,0,0,0,0,0});
        }
        
        
        // A Test behaves as an ordinary method
        [Test] public void Test_Small_List_Of_float_pad_5()
        {
            List<float> l1 = new List<float>()
            {
                1.1f,2.1f,3.1f,4.1f,5.1f
            };
            
            Assert.AreEqual(l1.PadList(10, 1.1f), 
                new List<float>(){1.1f,2.1f,3.1f,4.1f,5.1f, 1.1f,1.1f, 1.1f,1.1f,1.1f});
        }


        
    }
}
