using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;
using static StaticFunctions;



namespace EnvSetup
{
    public class TileLogicBuilder : ITileLogicBuilder
    {
        private readonly ITileMatrixProducer _tileMatrixProducer;
        private readonly ISpyTileLogic _spyTileLogic;
        private readonly IEnvTileLogic _envTileLogic;
        private readonly IGuardTileLogic _guardTileLogic;
        private readonly IPathFinder _pathFinder;
        private readonly IExitTileLogic _exitTileLogic;
        private readonly int _mapCreationTolerance;

        public TileLogicBuilder(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
            Dictionary<ParentObject, GameObject> parentDictionary, int mapCreationTolerance = 500,
            bool hasMiddleTiles = true)
        {
            var matrixSize = MapScaleToMatrixSize(mapScale);
            _tileMatrixProducer = new TileMatrixProducer(parentDictionary[ParentObject.TopParent].transform.position, matrixSize);
            _spyTileLogic = new SpyTileLogic(matrixSize);
            _envTileLogic = new EnvTileLogic(matrixSize, mapDifficulty, hasMiddleTiles);
            _pathFinder = new PathFinder();
            _exitTileLogic = new ExitTileLogic(matrixSize, exitCount);
            _guardTileLogic = new GuardTileLogic(mapScale, matrixSize, guardAgentCount);
            _mapCreationTolerance = mapCreationTolerance;
        }

        public ITileLogicSetup GetTileLogicSetup() =>
            new TileLogicSetup(
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
