﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using static StaticFunctions;
using static CreateEnv;



/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup
{
    /// <summary>
    /// Scale of map that has been created
    /// </summary>
    public int MapScale { get; }
    private readonly int _mapDifficulty;
    private readonly int _matrixSize;
    private readonly int _exitCount;
    private readonly int _guardAgentCount;
    private readonly int _mapCreationTolerance;
    private readonly bool _hasMiddleTiles;
    private readonly Dictionary<ParentObject, GameObject> _parentDictionary;
    private TileMatrix _tileMatrixProducer;
    private IEnvTile[,] _tileMatrix;

    public EnvSetup(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
        Dictionary<ParentObject, GameObject> parentDictionary, int mapCreationTolerance = 500, bool hasMiddleTiles = true)
    {
        MapScale = mapScale;
        _mapDifficulty = mapDifficulty;
        _matrixSize = MapScaleToMatrixSize(mapScale);
        _exitCount = exitCount;
        _guardAgentCount = guardAgentCount;
        _parentDictionary = parentDictionary;
        _tileMatrixProducer = new TileMatrix(_parentDictionary[ParentObject.TopParent].transform.position, _matrixSize);
        _tileMatrix = _tileMatrixProducer.Tiles;
        _mapCreationTolerance = mapCreationTolerance;
        _hasMiddleTiles = hasMiddleTiles;
        
    }

    /// <summary>
    /// Called by Academy class in the SceneController to produce a new env for training instance
    /// </summary>
    public void CreateEnv()
    {
        ModifyTileLogic();
        PopulateEnv(_tileMatrix, _parentDictionary, MapScale);

        // DebugFirstInstance(_tileMatrix, _parentDictionary, tile => tile.HasSpy);
        // DebugAll(_tileMatrix, _parentDictionary, tile => tile.HasGuard);
        // DebugAll(_tileMatrix, _parentDictionary, tile => tile.OnPath);

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
            TileMatrix matrixClone = (TileMatrix)_tileMatrixProducer.Clone();
            IEnvTile[,] tilesCopy = matrixClone.Tiles;

            SpyTileLogic spyTileLogic = new SpyTileLogic(_matrixSize);
            IEnvTile spyEnvTile = spyTileLogic.SetSpyTile(tilesCopy);
            
            IEnvTileLogic envTileLogic = new EnvTileLogic(_matrixSize, _mapDifficulty, _hasMiddleTiles);
            envTileLogic.SetEnvTiles(tilesCopy);
            
            IPathFinder pathFinder = new PathFinder();
            pathFinder.GetPath(spyEnvTile);
            
            IExitFinder exitFinder = new ExitFinder(_matrixSize, _exitCount);
            
            // this needs to be called before ExitFinder's other methods 
            exitFinder.CheckMatrix(tilesCopy);
            
            int maxExits = exitFinder.ExitCount;
            // ensures there is at most -1 guards to exits
            // refactor, this can be passed onto a method istead of the constructor
            int maxGuards = _guardAgentCount >= maxExits ? maxExits - 1 : _guardAgentCount;
            
            IGuardLogic guardLogic = new GuardLogic(tilesCopy, MapScale, _matrixSize, maxGuards);

            if (exitFinder.ExitsAreAvailable())
            {
                exitFinder.SetExitTiles();
                if (guardLogic.GuardPlacesAreAvailable())
                {
                    guardLogic.SetGuardTiles();
                    _tileMatrix = tilesCopy;
                    flag = false;
                }
                else
                {
                    count += 1;
                    if (count > _mapCreationTolerance) throw new MapCreationException("Not enough free tiles to place guards", _matrixSize, _mapDifficulty, 
                            maxExits, maxGuards, _exitCount, _guardAgentCount);
                }
            }
            else
            {
                count += 1;
                if (count > _mapCreationTolerance) throw new MapCreationException("Exits cannot be created: \n either the map is too small for the number of exits, or the spy cannot reach enough exit tiles", _matrixSize, _mapDifficulty,
                        maxExits, maxGuards, _exitCount, _guardAgentCount);
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


    /// <summary>
    /// Creates 3D object on first tile matching predicate - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    static void DebugFirstInstance(EnvTile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<EnvTile, bool> tilePredicate) =>
      CreateBox(new Vector3 (1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, (
          from EnvTile tile in tileMatrix
          select tile).Where(tilePredicate).ToList()[0].Position);
   
    /// <summary>
    /// Creates 3D objects on all tiles matching predicates - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    static void DebugAll(EnvTile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<EnvTile, bool> tilePredicate) =>
       (from EnvTile tile in tileMatrix
           select tile).Where(tilePredicate).ToList().ForEach(tile =>
           CreateBox(new Vector3(1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, tile.Position));

}
