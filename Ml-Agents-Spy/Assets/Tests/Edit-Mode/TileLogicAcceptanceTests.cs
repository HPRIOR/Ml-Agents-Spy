using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /*
     * Exit count of 100 given to all Envs to push exit count to their maximum 
     */

    public class TileLogicAcceptanceTests
    {
        private int _range = 100;
        Dictionary<ParentObject, GameObject> GetDictionary()
        {
            GameObject TopParent = new GameObject();
            GameObject EnvParent = new GameObject();
            GameObject SpyParent = new GameObject();
            GameObject GuardParent = new GameObject();
            GameObject DebugParent = new GameObject();

            return new Dictionary<ParentObject, GameObject>()
            {
                {ParentObject.TopParent, TopParent},
                {ParentObject.EnvParent, EnvParent},
                {ParentObject.SpyParent, SpyParent},
                {ParentObject.GuardParent, GuardParent},
                {ParentObject.DebugParent, DebugParent}

            };
        }
        
        [Test]
        public void TestScaleOne()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 1,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
            

        }

        [Test]
        public void TestScaleTwo()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 2,
                    mapDifficulty: 3,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }


        }

        [Test]
        public void TestScaleThree()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 3,
                    mapDifficulty: 25,
                    exitCount: 2,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }

        [Test]
        public void TestScaleFour()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 4,
                    mapDifficulty: 50,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }


        }

        [Test]
        public void TestScaleFive()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 5,
                    mapDifficulty: 100,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }


        }

        [Test]
        public void TestScaleSix()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 6,
                    mapDifficulty: 125,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }

        [Test]
        public void TestScaleSeven()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 7,
                    mapDifficulty: 200,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }

        [Test]
        public void TestScaleEight()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 8,
                    mapDifficulty: 250,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }

        [Test]
        public void TestScaleNine()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 9,
                    mapDifficulty: 300,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }

        [Test]
        public void TestScaleTen()
        {
            var dict = GetDictionary();
            foreach (int i in Enumerable.Range(0, _range))
            {
                EnvSetup env = new EnvSetup(
                    mapScale: 10,
                    mapDifficulty: 350,
                    exitCount: 100,
                    guardAgentCount: 2,
                    parentDictionary: dict
                );
                Assert.DoesNotThrow(() => env.CreateEnv());
            }
        }
    }
}
