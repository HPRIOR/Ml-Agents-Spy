using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;
using static ClassExtensions;
using static RandomHelper;
using Random = System.Random;


/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup
{
    private readonly int _mapSize;
    private readonly int _mapComplexity;
    private readonly int _gridMapSize;
    private readonly int _exitCount;
    private readonly int _guardAgentCount;

    private readonly Dictionary<ParentObject, GameObject> _parents;
    
    private List<List<Tile>> _tiles;
    private List<Tile> _complexityTiles = new List<Tile>();
    private List<Tile> _envTiles = new List<Tile>();
    private List<Tile> _exitTiles = new List<Tile>();
    private List<Tile> _guardTiles = new List<Tile>();
    private Tile _spyTile;


    public EnvSetup(int mapSize, int mapComplexity, int exitCount, int guardAgentCount,
        Dictionary<ParentObject, GameObject> parents)
    {
        _mapSize = mapSize;
        _mapComplexity = mapComplexity;
        _gridMapSize = mapSize % 2 == 0 ? (mapSize * 10) / 2 : ((mapSize * 10) / 2) + 1;

        _exitCount = exitCount;
        _guardAgentCount = guardAgentCount;
        
        _parents = parents;
        _tiles = new TileManager(_mapSize, _parents[ParentObject.TopParent].transform.localPosition, _gridMapSize).Tiles;
    }


    public void SetUpEnv()
    {
        CreateEnvLogic();
    }

    /// <summary>
    /// This is called by the Academy in the SceneController to produce a new env
    /// </summary>
    private void CreateEnvLogic()
    {
        CreatePlane(
            scale: new Vector3(_mapSize, 1, _mapSize),
            parent: _parents[ParentObject.TopParent].transform
            );

        var spyTile = SetSpyTile(_tiles, _gridMapSize);
        CreateInitialEnv(_tiles, _complexityTiles, _parents, _gridMapSize);
        AddEnvComplexity(_tiles, _mapComplexity, _gridMapSize,_mapSize, _parents);
        PathFinder.GetSpyPathFrom(spyTile);

        if (ExitsAreAvailable(_tiles, _gridMapSize, _exitCount))
        {
            PlaceExits(_gridMapSize, _exitCount, _guardAgentCount, _tiles);
            var guardSpawnTiles = GetGuardSpawnTiles(_tiles, _mapSize, _gridMapSize);
            
            if (GuardPlacesAreAvailable(guardSpawnTiles, _guardAgentCount, _mapSize, _gridMapSize))
            {
                SetGuardTiles(guardSpawnTiles, _mapSize, _gridMapSize, _guardAgentCount);
            }
            else
            {
                // throw error
            }
        }
        else
        {
            // throw error
        }

        // build map here when loop succceeds

    }
    

    private Tile SetSpyTile(List<List<Tile>> tiles, int gridMapSize)
    {
        int y = 1;
        int x = GetParityRandom(1, gridMapSize - 1, ParityEnum.Even);
        tiles[x][y].HasSpy = true;
        CreateBox(new Vector3(1,1,1), _parents[ParentObject.PerimeterParent].transform, tiles[x][y].Position);
        return tiles[x][y];
    }

    private static List<Tile> GetGuardSpawnTiles(List<List<Tile>> tiles, int mapSize, int gridMapSize) => 
        (from tileRow in tiles
            from tile in tileRow
            where tile.OnSpyPath
            where tile.Coords.x % 2 == 0
            where InGuardSpawnAreaY(tile, mapSize, gridMapSize)
            select tile).ToList();

    private static bool GuardPlacesAreAvailable(List<Tile> guardSpawnTiles, int guardCount, int mapSize, int gridMapSize) => 
        guardCount <= guardSpawnTiles.Count;

    private void SetGuardTiles(List<Tile> guardSpawnTiles, int mapSize, int gridMapSize, int guardCount)
    {
        var randomList = GetUniqueRandomList(guardCount, guardSpawnTiles.Count);
        Enumerable.Range(0, guardCount - 1).ToList().ForEach(i => guardSpawnTiles[randomList[i]].HasGuard = true);
    }


    private static bool InGuardSpawnAreaY(Tile tile, int mapSize, int gridMapSize) =>
        mapSize >= 1 & mapSize <= 3 & tile.Coords.y == gridMapSize - 1 
        || mapSize > 3 & (tile.Coords.y == gridMapSize - 1 || tile.Coords.y == gridMapSize - 3);



    private void GetAgentTiles(List<List<Tile>> tiles)
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                if (tile.IsExit) _exitTiles.Add(tile);
                else if (tile.HasSpy) _spyTile = tile;
                else if (tile.HasGuard) _guardTiles.Add(tile);
                else if (tile.HasEnv) _envTiles.Add(tile);
                else _complexityTiles.Add(tile);
            }
        }
    }

    // should pass in the first row of tiles 
    /// <summary>
    /// Checks if there are at least twice as many potential exit point as there are desired exit points
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="gridMapSize"></param>
    /// <param name="exitCount"></param>
    /// <returns></returns>
    private static bool ExitsAreAvailable(List<List<Tile>> tiles, int gridMapSize, int exitCount) =>
        exitCount <= (from tileRow in tiles
            from tile in tileRow
            where tile.Coords.y == gridMapSize
            where tile.AdjacentTile[Direction.S].OnSpyPath
            select tile).ToList().Count/2;


    private void PlaceExits(int gridMapSize, int exitCount, int guardCount, List<List<Tile>> tiles, int guardExitDiff = 1)
    {
        System.Random r = new Random();
        var potentialExits =
            (from tileRow in tiles
                from tile in tileRow
                where tile.Coords.y == gridMapSize
                where tile.AdjacentTile[Direction.S].OnSpyPath
                select tile).ToList();

        for (int i = 0; i < exitCount; i++)
        {
            var selectedExit = potentialExits[r.Next(0, potentialExits.Count - 1)];
            if (!selectedExit.AdjacentTile[Direction.E].IsExit & !selectedExit.AdjacentTile[Direction.W].IsExit & !selectedExit.IsExit)
            {
                selectedExit.IsExit = true;
                selectedExit.HasEnv = false;
                CreateBox(new Vector3(3, 3, 3), _parents[ParentObject.ComplexitiesParent].transform, selectedExit.Position);
            }
            else
            {
                i -= 1;
            }
        }
    }

    private static void CreateInitialEnv(List<List<Tile>> tiles, List<Tile> freeTiles, Dictionary<ParentObject, GameObject> parents, int maxLen)
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                if (CanPlacePerimeter(tile, maxLen))
                {
                    CreateBox(
                        scale: new Vector3(2, 2, 2),
                        parent: parents[ParentObject.PerimeterParent].transform,
                        position: tile.Position
                    );
                    tile.HasEnv = true;
                }
                else if (CanPlaceMiddle(tile, maxLen))
                {
                    CreateBox(
                        scale: new Vector3(2, 2, 2),
                        parent: parents[ParentObject.MiddleParent].transform,
                        position: tile.Position
                    );
                    tile.HasEnv = true;
                }
                else
                {
                    if (!tile.IsExit & !tile.HasGuard & !tile.HasSpy) freeTiles.Add(tile);
                }
            }
        }
    }

    private void AddEnvComplexity(List<List<Tile>> tiles, int mapComplexity, int gridMapSize, int mapSize,
        Dictionary<ParentObject, GameObject> parents)
    {
        List<Tile> freeTiles =
            (from tileRow in tiles
            from tile in tileRow
            where EnvComplexityCanPlace(tile, mapSize, gridMapSize)
            select tile)
            .ToList();

        // Defaults to max free guardSpawnTiles if complexity is higher. Ensures max 1 env-block per tile
        var checkComplexityCount = mapComplexity > freeTiles.Count ? freeTiles.Count : mapComplexity;

        List<int> randSequence = RandomHelper.GetUniqueRandomList(count: mapComplexity, maxVal: freeTiles.Count);
        
        for (int i = 0; i < checkComplexityCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            tile.HasEnv = true;
            CreateBox(new Vector3(2, 2, 2), parents[ParentObject.ComplexitiesParent].transform, tile.Position);
        }
    }

    // Env placement logic:

    private static bool EnvComplexityCanPlace(Tile tile, int mapSize, int gridMapSize) =>
        !tile.IsExit & !tile.HasEnv & !tile.HasGuard & !tile.HasSpy & !CanPlacePerimeter(tile, gridMapSize) & !CanPlaceMiddle(tile, gridMapSize); 

    private static bool NotInAgentArea(Tile tile, int mapSize, int gridMapSize) =>
        mapSize < 4 ? tile.Coords.y != 1 && tile.Coords.y != gridMapSize - 1: 
            tile.Coords.y != 1 
            && tile.Coords.y != 2 
            && tile.Coords.y != 3 
            && tile.Coords.y != gridMapSize - 1 
            && tile.Coords.y != gridMapSize - 2 
            && tile.Coords.y != gridMapSize - 3;

    private static bool CanPlaceMiddle(Tile tile, int maxLen) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !(tile.Coords.x == 0
            || tile.Coords.x == maxLen
            || tile.Coords.y == 0
            || tile.Coords.y == maxLen);

    private static bool CanPlacePerimeter(Tile tile, int maxLen) =>
        (tile.Coords.x == 0
         || tile.Coords.x == maxLen
         || tile.Coords.y == 0
         || tile.Coords.y == maxLen)
        & !tile.IsExit;


    private static void CreateBox(Vector3 scale, Transform parent, Vector3 position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = scale;
        box.transform.parent = parent;
    }

    /// <summary>
    /// Produces a plane with a specified size and position (relative to the parent)
    /// </summary>
    /// <param name="scale">the size of the plane</param>
    /// <param name="parent">the parent in the hierarchy window</param>
    private static void CreatePlane(Vector3 scale, Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = parent.localPosition;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
    }


    //private void CheckAgentCoords()
    //{
    //    PathFinder p = new PathFinder();
    //    p.GetSpyPathFrom(_complexityTiles[_complexityTiles.Count - 1]);
    //    Debug.Log(p.ExitCount);
    //}


    private static void PopulateEnv(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents) => tiles
        .ForEach(tileRow => tileRow
            .Where(tile => tile.HasEnv)
            .ToList()
            .ForEach(tile => CreateBox(new Vector3(2,2,2), parents[ParentObject.PerimeterParent].transform, tile.Position)));

   

}
