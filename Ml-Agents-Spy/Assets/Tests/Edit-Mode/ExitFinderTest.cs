using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ExitFinderTest
    {
        int matrixSize(int scale) => scale % 2 == 0 ? (scale * 10) / 2 : ((scale * 10) / 2) + 1;

        IEnvTile[,] GetTileMatrix(int scale)=>
            new TileMatrix(
                new Vector3(0, 0, 0), matrixSize(scale)
            ).Tiles;


        
        // A Test behaves as an ordinary method
        [Test]
        public void TestExitCountOneContentiousBlock()
        {
            // all exit tiles available: 5
            var matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            matrix[5, 15].OnPath = true;
            matrix[6, 15].OnPath = true;
            matrix[7, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].OnPath = true;
            matrix[11, 15].OnPath = true;
            matrix[12, 15].OnPath = true;
            matrix[13, 15].OnPath = true;
            matrix[14, 15].OnPath = true;
            matrix[15, 15].OnPath = true;
            ExitFinder exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            Assert.AreEqual(5, exitFinder.ExitCount);

            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            matrix[5, 15].OnPath = true;
            matrix[6, 15].OnPath = true;
            matrix[7, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            Assert.AreEqual(4, exitFinder.ExitCount);

            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            Assert.AreEqual(2, exitFinder.ExitCount);

            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 1);
            Assert.AreEqual(1, exitFinder.ExitCount);

        }

        [Test]
        public void TestExitsAreAvailable()
        {
            
            var matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            ExitFinder exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            Assert.AreEqual(false, exitFinder.ExitsAreAvailable());

            /*
             * This returns false because the total this situation can only produce 1 exit (in worst case)
             * There must be more than 2 exits otherwise one agent would be able to block the exit points
             */
            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 2);
            Assert.AreEqual(false, exitFinder.ExitsAreAvailable());

            /*
             * Despite having a reduced total number of potential exit points this will pass because they are seperate
             * allowing for at least 2 exit points in the worst case
             */
            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 2);
            Assert.AreEqual(true, exitFinder.ExitsAreAvailable());

        }

        [Test]
        public void TestTileGroupsExitCount()
        {
            var matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            
            matrix[6, 15].OnPath = true;
            matrix[7, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].OnPath = true;
            
            matrix[13, 15].OnPath = true;
            matrix[14, 15].OnPath = true;
            matrix[15, 15].OnPath = true;
            ExitFinder exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            Assert.AreEqual(4, exitFinder.ExitCount);
        }

        [Test]
        public void TestSetExitTiles()
        {
            var matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;

            matrix[6, 15].OnPath = true;
            matrix[7, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].OnPath = true;

            matrix[13, 15].OnPath = true;
            matrix[14, 15].OnPath = true;
            matrix[15, 15].OnPath = true;
            ExitFinder exitFinder = new ExitFinder(matrix, matrixSize(3), 100);
            
            exitFinder.SetExitTiles();

            int matrixExitCount = (from EnvTile tile in matrix
                where tile.IsExit
                select tile).Count();

            Assert.AreEqual(4, matrixExitCount);


            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;

            matrix[6, 15].OnPath = true;
            matrix[7, 15].OnPath = true;
            matrix[8, 15].OnPath = true;
            matrix[9, 15].OnPath = true;
            matrix[10, 15].OnPath = true;

            matrix[13, 15].OnPath = true;
            matrix[14, 15].OnPath = true;
            matrix[15, 15].OnPath = true;
            exitFinder = new ExitFinder(matrix, matrixSize(3), 2);

            exitFinder.SetExitTiles();

            matrixExitCount = (from EnvTile tile in matrix
                where tile.IsExit
                select tile).Count();

            Assert.AreEqual(2, matrixExitCount);
        }
    }
}
