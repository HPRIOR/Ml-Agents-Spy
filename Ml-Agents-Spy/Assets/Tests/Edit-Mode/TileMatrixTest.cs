using Enums;
using EnvSetup;
using NUnit.Framework;
using UnityEngine;
using static StaticFunctions;

namespace Tests
{
    public class TileMatrixTest
    {
        [Test]
        public void VectorTest()
        {
            TileMatrixProducer tmOne = new TileMatrixProducer(new Vector3(0, 0, 0), MapScaleToMatrixSize(1));
            
            // (0, 0) 
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1), 0.5f, -MapScaleToMatrixSize(1)), tmOne.Tiles[0,0].Position);
            // (0, 6)
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1), 0.5f, MapScaleToMatrixSize(1)), tmOne.Tiles[0,6].Position);
            //(1, 6)
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1) + 2, 0.5f, MapScaleToMatrixSize(1)), tmOne.Tiles[1,6].Position);

            // Test relative to centre 
            TileMatrixProducer tmOneRelative = new TileMatrixProducer(new Vector3(2, 1.5f, 2), MapScaleToMatrixSize(1));
            // (0, 0) 
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1)+2, 2, -MapScaleToMatrixSize(1)+2), tmOneRelative.Tiles[0,0].Position);
            // (0, 6)
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1)+2, 2, MapScaleToMatrixSize(1)+2), tmOneRelative.Tiles[0,6].Position);
            //(1, 6)
            Assert.AreEqual(new Vector3(-MapScaleToMatrixSize(1) + 4, 2, MapScaleToMatrixSize(1)+2), tmOneRelative.Tiles[1,6].Position);

        }

        [Test]
        public void CoordsTest()
        {
            TileMatrixProducer tmOne = new TileMatrixProducer(new Vector3(0, 0, 0), MapScaleToMatrixSize(1));

            // (0, 0) 
            Assert.AreEqual((0, 0), tmOne.Tiles[0,0].Coords);
            // (0, 6)
            Assert.AreEqual((0, 6), tmOne.Tiles[0,6].Coords);
            //(1, 6)
            Assert.AreEqual((1, 6), tmOne.Tiles[1,6].Coords);
            //(3, 3)
            Assert.AreEqual((3, 3), tmOne.Tiles[3,3].Coords);

        }

        [Test]
        public void AdjacencyMatrixTest()
        {
            TileMatrixProducer tmOne = new TileMatrixProducer(new Vector3(0, 0, 0), MapScaleToMatrixSize(1));

            // test min
            Assert.AreEqual(tmOne.Tiles[0,0], tmOne.Tiles[1,0].AdjacentTile[Direction.W]);
            Assert.AreEqual(tmOne.Tiles[0,0], tmOne.Tiles[0,1].AdjacentTile[Direction.S]);

            // test mutually referencing tiles above
            Assert.AreEqual(tmOne.Tiles[0,0].AdjacentTile[Direction.N], tmOne.Tiles[0,1]);
            Assert.AreEqual(tmOne.Tiles[0,0].AdjacentTile[Direction.E], tmOne.Tiles[1,0]);

            // test for null on edges
            Assert.AreEqual(tmOne.Tiles[0,0].AdjacentTile[Direction.S], null);
            Assert.AreEqual(tmOne.Tiles[0,0].AdjacentTile[Direction.W], null);

            // test max edge
            Assert.AreEqual(tmOne.Tiles[6,6].AdjacentTile[Direction.N], null);
            Assert.AreEqual(tmOne.Tiles[6,6].AdjacentTile[Direction.E], null);
            Assert.AreEqual(tmOne.Tiles[6,6].AdjacentTile[Direction.S], tmOne.Tiles[6,5]);
            Assert.AreEqual(tmOne.Tiles[6,6].AdjacentTile[Direction.W], tmOne.Tiles[5,6]);

            //test middle all directions 
            Assert.AreEqual(tmOne.Tiles[3,3].AdjacentTile[Direction.N], tmOne.Tiles[3,4]);
            Assert.AreEqual(tmOne.Tiles[3,3].AdjacentTile[Direction.E], tmOne.Tiles[4,3]);
            Assert.AreEqual(tmOne.Tiles[3,3].AdjacentTile[Direction.S], tmOne.Tiles[3,2]);
            Assert.AreEqual(tmOne.Tiles[3,3].AdjacentTile[Direction.W], tmOne.Tiles[2,3]);

        }
    }
}
