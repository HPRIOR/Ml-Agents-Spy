using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;
using static StaticFunctions;


namespace EnvSetup
{
    public class TileLogicBuilder : ITileLogicBuilder
    {
        private readonly IEnvTileLogic _envTileLogic;
        private readonly IExitTileLogic _exitTileLogic;
        private readonly IGuardTileLogic _guardTileLogic;
        private readonly int _mapCreationTolerance;
        private readonly IPathFinder _pathFinder;
        private readonly ISpyTileLogic _spyTileLogic;
        private readonly ITileMatrixProducer _tileMatrixProducer;

        /// <summary>
        /// Instantiates each class which manipulates the EnvTile matrix with the relevant parameters
        /// </summary>
        /// <param name="mapScale">size of the map</param>
        /// <param name="mapDifficulty">number of randomly placed env blocks</param>
        /// <param name="exitCount">number of exits</param>
        /// <param name="guardAgentCount">number of guards</param>
        /// <param name="parentDictionary">Dictionary of gameobjects in hierarchy</param>
        /// <param name="mapCreationTolerance">number of times the algorithm should attempt to produce the map</param>
        /// <param name="hasMiddleTiles">determines if the map should have the middle tiles (corridors)</param>
        public TileLogicBuilder(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
            Dictionary<ParentObject, GameObject> parentDictionary, int mapCreationTolerance = 500,
            bool hasMiddleTiles = true)
        {
            var matrixSize = MapScaleToMatrixSize(mapScale);
            _tileMatrixProducer =
                new TileMatrixProducer(parentDictionary[ParentObject.TopParent].transform.position, matrixSize);
            _spyTileLogic = new SpyTileLogic(matrixSize);
            _envTileLogic = new EnvTileLogic(matrixSize, mapDifficulty, hasMiddleTiles);
            _pathFinder = new PathFinder();
            _exitTileLogic = new ExitTileLogic(matrixSize, exitCount);
            _guardTileLogic = new GuardTileLogic(mapScale, matrixSize, guardAgentCount);
            _mapCreationTolerance = mapCreationTolerance;
        }

        /// <summary>
        /// Returns the TileLogicSetup class which uses the fields of this class to produce the environment
        /// </summary>
        /// <returns></returns>
        public ITileLogicSetup GetTileLogicSetup()
        {
            return new TileLogicSetup(
                _tileMatrixProducer,
                _spyTileLogic,
                _envTileLogic,
                _guardTileLogic,
                _exitTileLogic,
                _pathFinder,
                _mapCreationTolerance
            );
        }
    }
}