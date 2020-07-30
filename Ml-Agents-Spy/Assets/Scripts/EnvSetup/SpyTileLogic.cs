using Enums;
using Interfaces;
using static RandomHelper;

namespace EnvSetup
{
    public class SpyTileLogic : ISpyTileLogic
    {
        private readonly IEnvTile[,] _tileMatrix;
        private readonly int _matrixSize;
        public SpyTileLogic(int matrixSize)
        {
            _matrixSize = matrixSize;
        }

        /// <summary>
        /// Set the spawn tile of the Spy agent in the first row of the tile matrix
        /// </summary>
        /// <param name="tileMatrix">Matrix of Tiles</param>
        /// <param name="matrixSize">Size of matrix</param>
        /// <returns></returns>
        public IEnvTile SetSpyTile(IEnvTile[,] tileMatrix)
        {
            int y = 1;
            int x = GetParityRandom(1, _matrixSize - 1, ParityEnum.Even);
            tileMatrix[x, y].HasSpy = true;
            return tileMatrix[x, y];
        }
    }
}
