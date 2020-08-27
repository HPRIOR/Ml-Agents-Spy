using System.Linq;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class ExitFinderTest
    {
        private int MatrixSize(int scale) => scale % 2 == 0 ? scale * 10 / 2 : scale * 10 / 2 + 1;

        private IEnvTile[,] GetTileMatrix(int scale) =>
            new TileMatrixProducer(
                new Vector3(0, 0, 0), MatrixSize(scale)
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
            var exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(5, exitTileLogic.ExitCount);

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
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(4, exitTileLogic.ExitCount);

            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(2, exitTileLogic.ExitCount);

            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            matrix[4, 15].OnPath = true;
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 1);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(1, exitTileLogic.ExitCount);
        }

        [Test]
        public void TestExitsAreAvailable()
        {
            var matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            var exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(false, exitTileLogic.ExitsAreAvailable());

            /*
             * This returns false because the total this situation can only produce 1 exit (in worst case)
             * There must be more than 2 exits otherwise one agent would be able to block the exit points
             */
            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[2, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 2);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(false, exitTileLogic.ExitsAreAvailable());

            /*
             * Despite having a reduced total number of potential exit points this will pass because they are seperate
             * allowing for at least 2 exit points in the worst case
             */
            matrix = GetTileMatrix(3);
            matrix[1, 15].OnPath = true;
            matrix[3, 15].OnPath = true;
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 2);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(true, exitTileLogic.ExitsAreAvailable());
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
            var exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            Assert.AreEqual(4, exitTileLogic.ExitCount);
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
            var exitTileLogic = new ExitTileLogic(MatrixSize(3), 100);
            exitTileLogic.CheckMatrix(matrix);
            exitTileLogic.SetExitTiles();

            var matrixExitCount = (from EnvTile tile in matrix
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
            exitTileLogic = new ExitTileLogic(MatrixSize(3), 2);
            exitTileLogic.CheckMatrix(matrix);

            exitTileLogic.SetExitTiles();

            matrixExitCount = (from EnvTile tile in matrix
                where tile.IsExit
                select tile).Count();

            Assert.AreEqual(2, matrixExitCount);
        }
    }
}