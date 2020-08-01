using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;

namespace EnvSetup
{
    /// <summary>
    /// This class generates the environment environment 
    /// </summary>
    public class TileLogicSetup : ITileLogicSetup
    {
 
        private readonly int _mapCreationTolerance;
        private IEnvTile[,] _tileMatrix;

        private readonly ITileMatrixProducer _tileMatrixProducer;
        private readonly ISpyTileLogic _spyTileLogic;
        private readonly IEnvTileLogic _envTileLogic;
        private readonly IGuardTileLogic _guardTileLogic;
        private readonly IPathFinder _pathFinder;
        private readonly IExitTileLogic _exitTileLogic;

        public TileLogicSetup(ITileMatrixProducer tileMatrixProducer, ISpyTileLogic spyTileLogic, 
            IEnvTileLogic envTileLogic, IGuardTileLogic guardTileLogic, IExitTileLogic exitTileLogic, IPathFinder pathFinder,
            int mapCreationTolerance = 500, bool hasMiddleTiles = true)
        {
            _tileMatrixProducer = tileMatrixProducer;
            _tileMatrix = _tileMatrixProducer.Tiles;
            _spyTileLogic = spyTileLogic;
            _envTileLogic = envTileLogic;
            _guardTileLogic = guardTileLogic;
            _pathFinder = pathFinder;
            _exitTileLogic = exitTileLogic;
            _mapCreationTolerance = mapCreationTolerance;
        }

        /// <summary>
        /// Called by Academy class in the SceneController to produce a new env for training instance
        /// </summary>
        public IEnvTile[,] GetTileLogic()
        {
            ModifyTileLogic();
            // DebugFirstInstance(_tileMatrix, _parentDictionary, tile => tile.HasSpy);
            // DebugAll(_tileMatrix, _parentDictionary, tile => tile.HasGuard);
            // DebugAll(_tileMatrix, _parentDictionary, tile => tile.OnPath);
            return _tileMatrix;
        
        }

        /// <summary>
        /// Changes logic within each tile, helping to generate the environment and agent tiles
        /// Loops until appropriate environment found, or throws an error
        /// </summary> 
        void ModifyTileLogic()
        {
            var flag = true;
            int count = 0;

            while (flag)
            {
                ITileMatrixProducer matrixProducerClone = (ITileMatrixProducer)_tileMatrixProducer.Clone();
                IEnvTile[,] tilesCopy = matrixProducerClone.Tiles;
                IEnvTile spyEnvTile = _spyTileLogic.SetSpyTile(tilesCopy);
                _envTileLogic.SetEnvTiles(tilesCopy);
                _pathFinder.GetPath(spyEnvTile);
                _exitTileLogic.CheckMatrix(tilesCopy);
                _guardTileLogic.GetMaxExitCount(_exitTileLogic.ExitCount);
                _guardTileLogic.GetPotentialGuardPlaces(tilesCopy);
                if (_exitTileLogic.ExitsAreAvailable())
                {
                    _exitTileLogic.SetExitTiles();
                    if (_guardTileLogic.GuardPlacesAreAvailable())
                    {
                        _guardTileLogic.SetGuardTiles();
                        _tileMatrix = tilesCopy;
                    
                        flag = false;
                    }
                    else
                    {
                        count += 1;
                        if (count > _mapCreationTolerance) throw new MapCreationException("Not enough free tiles to place guards");
                    }
                }
                else
                {
                    count += 1;
                    _exitTileLogic.CanProceed = true;
                    if (count > _mapCreationTolerance) throw new MapCreationException("Exits cannot be created: \n either the map is too small for the number of exits, or the spy cannot reach enough exit tiles");
                }
            }
        }

    
        /// <summary>
        /// Returns dictionary of Tile type references and corresponding tiles
        /// </summary>
        /// <returns>Tile type references and corresponding tiles</returns>
        public Dictionary<TileType, List<IEnvTile>> GetTileTypes()
        {
            var tileTypesDictionary = new Dictionary<TileType, List<IEnvTile>>();
            Enum.GetValues(typeof(TileType)).Cast<TileType>().ToList().ForEach(tileType => tileTypesDictionary.Add(tileType, new List<IEnvTile>()));

            foreach (var tile in _tileMatrix)
            {
                if (tile.IsExit) tileTypesDictionary[TileType.ExitTiles].Add(tile);
                else if (tile.HasSpy) tileTypesDictionary[TileType.SpyTile].Add(tile);
                else if (tile.HasGuard) tileTypesDictionary[TileType.GuardTiles].Add(tile);
                else if (tile.HasEnv) tileTypesDictionary[TileType.EnvTiles].Add(tile);
                else tileTypesDictionary[TileType.FreeTiles].Add(tile);
            }
            return tileTypesDictionary;
        }


    

    }
}
