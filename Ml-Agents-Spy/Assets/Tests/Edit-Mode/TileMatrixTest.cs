using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TileMatrixTest
    {

        private static int MatrixSizeCalc(int mapScale) => 
            mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;

        [Test]
        public void MatrixSizeTest()
        {
            TileMatrix tmOne = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(1));
            Assert.AreEqual(7, tmOne.Tiles[0].Count);
            Assert.AreEqual(7, tmOne.Tiles.Count);

            TileMatrix tmTwo = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(2));
            Assert.AreEqual(11, tmTwo.Tiles[0].Count);
            Assert.AreEqual(11, tmTwo.Tiles.Count);

            TileMatrix tmThree = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(3));
            Assert.AreEqual(11, tmTwo.Tiles[0].Count);
            Assert.AreEqual(11, tmTwo.Tiles.Count);

            TileMatrix tmFour = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(4));
            Assert.AreEqual(17, tmThree.Tiles[0].Count);
            Assert.AreEqual(17, tmThree.Tiles.Count);

        }

        [Test]
        public void VectorTest()
        {
            TileMatrix tmOne = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(1));
            Assert.AreEqual(7, tmOne.Tiles[0].Count);
            Assert.AreEqual(7, tmOne.Tiles.Count);

            TileMatrix tmTwo = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(2));
            Assert.AreEqual(11, tmTwo.Tiles[0].Count);
            Assert.AreEqual(11, tmTwo.Tiles.Count);

            TileMatrix tmThree = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(3));
            Assert.AreEqual(11, tmTwo.Tiles[0].Count);
            Assert.AreEqual(11, tmTwo.Tiles.Count);

            TileMatrix tmFour = new TileMatrix(new Vector3(0, 0, 0), MatrixSizeCalc(4));
            Assert.AreEqual(17, tmThree.Tiles[0].Count);
            Assert.AreEqual(17, tmThree.Tiles.Count);

        }



    }
}
