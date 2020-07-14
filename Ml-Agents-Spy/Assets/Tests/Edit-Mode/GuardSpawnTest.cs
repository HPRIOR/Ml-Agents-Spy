﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GuardSpawnTest
    {
        int matrixSize(int scale) => scale % 2 == 0 ? (scale * 10) / 2 : ((scale * 10) / 2) + 1;

        Tile[,] GetTileMatrix(int scale) =>
            new TileMatrix(
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
            GuardLogic guardLogic = new GuardLogic(matrix, 3, matrixSize(3), 3);
            Assert.AreEqual(false, guardLogic.GuardPlacesAreAvailable());
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
            GuardLogic guardLogic = new GuardLogic(matrix, 3, matrixSize(3), 3);
            Assert.AreEqual(false, guardLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestJustRightAmountOfAvailableTiles()
        {

            var matrix = GetTileMatrix(3);
            matrix[2, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            GuardLogic guardLogic = new GuardLogic(matrix, 3, matrixSize(3), 3);
            Assert.AreEqual(true, guardLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestGreaterThanFourYAxis()
        {
            var matrix = GetTileMatrix(4);
            matrix[2, 17].OnPath = true;
            matrix[4, 18].OnPath = true;
            matrix[6, 19].OnPath = true;
            GuardLogic guardLogic = new GuardLogic(matrix, 4, matrixSize(4), 3);
            Assert.AreEqual(true, guardLogic.GuardPlacesAreAvailable());

        }

        [Test]
        public void TestSetGuardTiles()
        {
            var matrix = GetTileMatrix(4);
            matrix[2, 17].OnPath = true;
            matrix[4, 18].OnPath = true;
            matrix[6, 19].OnPath = true;
            GuardLogic guardLogic = new GuardLogic(matrix, 4, matrixSize(4), 3);
            guardLogic.SetGuardTiles();
            Assert.AreEqual(true, matrix[2,17].HasGuard);
            Assert.AreEqual(true, matrix[4, 18].HasGuard);
            Assert.AreEqual(true, matrix[6, 19].HasGuard);

        }
    }
}
