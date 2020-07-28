using System.Collections;
using System.Collections.Generic;
using System.Text;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GuardSpawnTest
    {
        int matrixSize(int scale) => scale % 2 == 0 ? (scale * 10) / 2 : ((scale * 10) / 2) + 1;

        IEnvTile[,] GetTileMatrix(int scale) =>
            new TileMatrixProducer(
                new Vector3(0, 0, 0), matrixSize(scale)
            ).Tiles;


        [Test]
        public void TestGuardPlacesFilledWithEnvTiles()
        {
            var matrix = GetTileMatrix(3);
            matrix[1, 15].HasEnv = true;
            matrix[2, 15].HasEnv = true;
            matrix[3, 15].HasEnv = true;
            matrix[4, 15].HasEnv = true;
            matrix[5, 15].HasEnv = true;
            matrix[6, 15].HasEnv = true;
            matrix[7, 15].HasEnv = true;
            matrix[8, 15].HasEnv = true;
            matrix[9, 15].HasEnv = true;
            matrix[10, 15].HasEnv = true;
            matrix[11, 15].HasEnv = true;
            matrix[12, 15].HasEnv = true;
            matrix[13, 15].HasEnv = true;
            matrix[14, 15].HasEnv = true;
            matrix[15, 15].HasEnv = true;
            GuardTileLogic guardTileLogic = new GuardTileLogic(3, matrixSize(3), 3);
            guardTileLogic.GetPotentialGuardPlaces(matrix);
            Assert.AreEqual(false, guardTileLogic.GuardPlacesAreAvailable());
        }

        [Test]
        public void TestIfCanSpawnBetweenPillars()
        {

           var matrix = GetTileMatrix(3);
            matrix[1, 15].HasEnv = true;
            matrix[2, 15].HasEnv = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].HasEnv = true;
            matrix[5, 15].OnPath = true;
            matrix[6, 15].HasEnv = true;
            matrix[7, 15].HasEnv = true;
            matrix[8, 15].HasEnv = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].HasEnv = true;
            matrix[11, 15].HasEnv = true;
            matrix[12, 15].HasEnv = true;
            matrix[13, 15].HasEnv = true;
            matrix[14, 15].HasEnv = true;
            matrix[15, 15].HasEnv = true;
            GuardTileLogic guardTileLogic = new GuardTileLogic(3, matrixSize(3), 3);
            guardTileLogic.GetPotentialGuardPlaces(matrix);
            Assert.AreEqual(false, guardTileLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestJustRightAmountOfAvailableTiles()
        {

            var matrix = GetTileMatrix(3);
            matrix[2, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            GuardTileLogic guardTileLogic = new GuardTileLogic(3, matrixSize(3), 3);
            guardTileLogic.GetMaxExitCount(3);
            guardTileLogic.GetPotentialGuardPlaces(matrix);
            Assert.AreEqual(true, guardTileLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestGreaterThanFourYAxis()
        {
            var matrix = GetTileMatrix(4);
            matrix[2, 17].OnPath = true;
            matrix[4, 18].OnPath = true;
            matrix[6, 19].OnPath = true;
            GuardTileLogic guardTileLogic = new GuardTileLogic(4, matrixSize(4), 3);
            guardTileLogic.GetMaxExitCount(3);
            guardTileLogic.GetPotentialGuardPlaces(matrix);
            Assert.AreEqual(true, guardTileLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestSetGuardTiles()
        {
            var matrix = GetTileMatrix(4);
            matrix[2, 17].OnPath = true;
            matrix[4, 18].OnPath = true;
            matrix[6, 19].OnPath = true;
            GuardTileLogic guardTileLogic = new GuardTileLogic(4, matrixSize(4), 3);
            guardTileLogic.GetMaxExitCount(4);
            guardTileLogic.GetPotentialGuardPlaces(matrix);
            guardTileLogic.SetGuardTiles();
            Assert.AreEqual(true, matrix[2, 17].HasGuard);
            Assert.AreEqual(true, matrix[4, 18].HasGuard);
            Assert.AreEqual(true, matrix[6, 19].HasGuard);

        }
    }
}
