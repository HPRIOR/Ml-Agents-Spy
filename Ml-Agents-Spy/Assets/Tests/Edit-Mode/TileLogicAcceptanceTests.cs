using System.Collections.Generic;
using System.Linq;
using Enums;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    /*
     * Exit count of 100 given to all Envs to push exit count to their maximum 
     */

    public class TileLogicAcceptanceTests
    {
        private const int Range = 500;

        private Dictionary<ParentObject, GameObject> GetDictionary()
        {
            var TopParent = new GameObject();
            var EnvParent = new GameObject();
            var SpyParent = new GameObject();
            var GuardParent = new GameObject();
            var DebugParent = new GameObject();

            return new Dictionary<ParentObject, GameObject>
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
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                1,
                1,
                100,
                2,
                dict
            );

            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleTwo()
        {
            var dict = GetDictionary();

            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                2,
                3,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleThree()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                3,
                25,
                2,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleFour()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                4,
                50,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleFive()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                5,
                100,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleSix()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                6,
                125,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleSeven()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                7,
                200,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleEight()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                8,
                250,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleNine()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                9,
                300,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }

        [Test]
        public void TestScaleTen()
        {
            var dict = GetDictionary();
            ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
                10,
                350,
                100,
                2,
                dict
            );
            foreach (var i in Enumerable.Range(0, Range))
            {
                var tileLogic = tileLogicBuilder.GetTileLogicSetup();

                Assert.DoesNotThrow(() => tileLogic.GetTileLogic());
            }
        }
    }
}