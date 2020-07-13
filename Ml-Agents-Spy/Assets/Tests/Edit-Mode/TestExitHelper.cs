using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests
{
    public class TestExitHelper
    {

        

        List<Tile> getAdjecentTiles(int numTiles)
        {
            List<Tile> tiles = new List<Tile>();
            foreach (int i in Enumerable.Range(0, numTiles + 1))
            {
                tiles.Add(new Tile(
                    new Vector3(0, 0, 0),
                    (i, 0)
                ));
            }

            return tiles;
        }
        [Test]
        public void TestCalculateMaxForGroup()
        {

            
            // Tile list for scale 1
            var twoTiles = getAdjecentTiles(2);
            var threeTiles = getAdjecentTiles(3);
            var fourTiles = getAdjecentTiles(4);
            var fiveTiles = getAdjecentTiles(5);
            var sixTiles = getAdjecentTiles(6);
            var eightTiles = getAdjecentTiles(8);
            var twelveTiles = getAdjecentTiles(12);
            var thirteenTiles = getAdjecentTiles(13);
            var twentyTiles = getAdjecentTiles(20);

            ExitHelper e = new ExitHelper(twoTiles);
            


            



        }

    }
}
