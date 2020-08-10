using System;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;

namespace EnvSetup
{
    /// <summary>
    ///     Creates Tile Matrix
    /// </summary>
    public class TileMatrixProducer : ICloneable, ITileMatrixProducer
    {
        private readonly int _matrixSize;
        private readonly Vector3 _planeCentre;


        /// <summary>
        ///     plane center is local position of training instance
        /// </summary>
        /// <param name="planeCentre"></param>
        /// <param name="matrixSize"></param>
        public TileMatrixProducer(Vector3 planeCentre, int matrixSize)
        {
            _planeCentre = planeCentre;
            _matrixSize = matrixSize;
            Tiles = CreateTilesMatrix(matrixSize, planeCentre);
            GetAdjacentTiles(Tiles, matrixSize);
        }

        public object Clone()
        {
            return new TileMatrixProducer(_planeCentre, _matrixSize);
        }

        public IEnvTile[,] Tiles { get; }

        /// <summary>
        ///     Produces matrix of tiles
        /// </summary>
        /// <param name="gridMapSize">max height/width from centre of outermost tile</param>
        /// <param name="planeCentre">centre of the target plane</param>
        /// <returns></returns>
        private IEnvTile[,] CreateTilesMatrix(int gridMapSize, Vector3 planeCentre)
        {
            var tiles = new IEnvTile[gridMapSize + 1, gridMapSize + 1];
            var x = -gridMapSize;
            for (var i = 0; i < gridMapSize + 1; i++)
            {
                var z = -gridMapSize;
                for (var j = 0; j < gridMapSize + 1; j++)
                {
                    tiles[i, j] =
                        new EnvTile(
                            planeCentre + new Vector3(x, 0.5f, z),
                            (i, j)
                        );
                    z += 2;
                }

                x += 2;
            }

            return tiles;
        }


        /// <summary>
        ///     Populates the adjacency dictionary of each tile with NESW neighboring tiles
        /// </summary>
        /// <param name="tileMatrix">2D array of tiles</param>
        /// <param name="matrixSize">The maximum width and height of tiles</param>
        private void GetAdjacentTiles(IEnvTile[,] tileMatrix, int matrixSize)
        {
            foreach (var tile in tileMatrix) GetAllDirectionTiles(tile, tileMatrix, matrixSize);
        }


        private void GetAllDirectionTiles(IEnvTile envTile, IEnvTile[,] tileMatrix, int matrixSize)
        {
            Enum.GetValues(typeof(Direction)).Cast<Direction>()
                .ToList()
                .ForEach(direction => envTile.AdjacentTile[direction] =
                    GetDirectionTile(direction, envTile, tileMatrix, matrixSize));
        }

        private IEnvTile GetDirectionTile(Direction d, IEnvTile inputEnvTile, IEnvTile[,] tileMatrix, int matrixSize)
        {
            if (d == Direction.N)
                return inputEnvTile.Coords.y == matrixSize
                    ? null
                    : tileMatrix[inputEnvTile.Coords.x, inputEnvTile.Coords.y + 1];
            if (d == Direction.E)
                return inputEnvTile.Coords.x == matrixSize
                    ? null
                    : tileMatrix[inputEnvTile.Coords.x + 1, inputEnvTile.Coords.y];
            if (d == Direction.S)
                return inputEnvTile.Coords.y == 0 ? null : tileMatrix[inputEnvTile.Coords.x, inputEnvTile.Coords.y - 1];
            if (d == Direction.W)
                return inputEnvTile.Coords.x == 0 ? null : tileMatrix[inputEnvTile.Coords.x - 1, inputEnvTile.Coords.y];
            throw new Exception("No direction given in TileMatrix.GetDirectionTile");
        }
    }
}