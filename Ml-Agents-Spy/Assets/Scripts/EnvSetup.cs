using System;
using System.Collections.Generic;
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
    /// <summary>
    /// grid map size is half of the total map size in tiles
    /// </summary>
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
        PlaceExits(_gridMapSize, _exitCount, _guardAgentCount, _tiles);
        SetAgentTiles(_tiles, _mapSize, _gridMapSize, _guardAgentCount);
        CreateInitialEnv(_tiles, _complexityTiles, _parents, _gridMapSize);
        AddEnvBoxComplexity(_complexityTiles, _mapComplexity, _mapSize, _parents);
        
    }

    private void SetAgentTiles(List<List<Tile>> tiles, int mapSize, int gridMapSize, int guardCount)
    {
        // place spy
        SetSpyTile(tiles,  mapSize, gridMapSize);

        //place guards
        SetGuardTiles(tiles, mapSize, gridMapSize, guardCount);
    }


    private void SetSpyTile(List<List<Tile>> tiles, int mapSize, int gridMapSize)
    {
        int y = mapSize >= 1 & mapSize <= 3 ? 1 : GetParityRandom(1, 3, ParityEnum.Odd);
        int x = GetParityRandom(1, gridMapSize - 1, ParityEnum.Even);
        tiles[x][y].HasSpy = true;
        CreateBox(new Vector3(1,1,1), _parents[ParentObject.PerimeterParent].transform, tiles[x][y].Position);
    }

    private void SetGuardTiles(List<List<Tile>> tiles, int mapSize, int gridMapSize, int guardCount)
    {
        List<(int, int)> coordsList = new List<(int, int)>();

        // ensures the maximum amount of guards doesn't exceed the room for them in the map
        int loopCount = mapSize <= 3 & guardCount > mapSize * 2 ? mapSize * 2 :
            mapSize > 3 & guardCount > mapSize * 4 ? mapSize * 4 : guardCount;

        for (int i = 0; i < loopCount; i++)
        {
            int y = mapSize >= 1 & mapSize <= 3 ? gridMapSize - 1 : GetParityRandom(gridMapSize - 4, gridMapSize - 1, ParityEnum.Odd);
            int x = GetParityRandom(1, gridMapSize - 1, ParityEnum.Even);

            if (coordsList.Contains((x, y))) { i -= 1; }
            else
            {
                tiles[x][y].HasGuard = true;
                coordsList.Add((x, y));

                CreateBox(new Vector3(1, 1, 1), _parents[ParentObject.ComplexitiesParent].transform, tiles[x][y].Position);
            }

        }
    }

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


    private void PlaceExits(int gridMapSize, int exitCount, int guardCount, List<List<Tile>> tiles, int guardExitDiff = 1)
    {
        // ensures no# of exits > than no# guards and can fit onto the map
        int exitCountCheck = exitCount <= guardCount ? guardCount + guardExitDiff : exitCount > gridMapSize - 2 ? gridMapSize : exitCount;
        List<(int,int)> coordsList = new List<(int, int)>();

        System.Random r = new Random();
        for (int i = 0; i < exitCountCheck; i++)
        {
            int x = r.Next(1, gridMapSize - 1);
            int y = gridMapSize;
            if (coordsList.Contains((x, y))) i -= 1;
            else
            {
                tiles[x][y].IsExit = true;
                coordsList.Add((x, y));
                CreateBox(new Vector3(1, 1, 1), _parents[ParentObject.ComplexitiesParent].transform, tiles[x][y].Position);
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

    

    /// <summary>
    /// Adds complexity to the map by filling in tiles and blocking off areas, reducing the possible the routes to the finish
    /// </summary>
    /// <param name="complexityTiles"></param>
    /// <param name="mapComplexity"></param>
    /// <param name="mapSize"></param>
    /// <param name="parents"></param>
    private void AddEnvBoxComplexity(List<Tile> complexityTiles, int mapComplexity, int mapSize, Dictionary<ParentObject, GameObject> parents)
    {
        // Defaults to max free tiles if complexity is higher. Ensures max 1 env-block per tile
        var checkComplexityCount = mapComplexity > complexityTiles.Count ? complexityTiles.Count : mapComplexity;

        // unique list of indexes up to the amount of tiles 
        List<int> randSequence = RandomHelper.GetUniqueRandomList(count: mapComplexity, maxVal: complexityTiles.Count);
        List<(int, int)> tilesToDeleteCoords = new List<(int, int)>();
        
        for (int i = 0; i < checkComplexityCount; i++)
        {
            var tile = complexityTiles[randSequence[i]];
            
                tile.HasEnv = true;
                tilesToDeleteCoords.Add(tile.Coords);
                CreateBox(new Vector3(2, 2, 2), parents[ParentObject.ComplexitiesParent].transform, tile.Position);
            
        }

        // should change to loop through every tile, instead of relying on init env logic class above - also exlude perimeter 
        // nasty side effect -- adds complexity -- only used to communicate tile references between EnvComplexity and Envlogic setup 
        _complexityTiles = complexityTiles.Where(tile => tilesToDeleteCoords.All(coords => tile.Coords != coords)).ToList();

    }

    private static bool NotInAgentArea(Tile tile, int mapSize) =>
        mapSize < 4 ? tile.Coords.y != 1 : tile.Coords.y != 1 || tile.Coords.y != 3;

    

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


    private void CheckAgentCoords()
    {
        PathFinder p = new PathFinder();
        p.GetExitCount(_complexityTiles[_complexityTiles.Count - 1]);
        Debug.Log(p.ExitCount);
    }


    private static void PopulateEnv(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents) => tiles
        .ForEach(tileRow => tileRow
            .Where(tile => tile.HasEnv)
            .ToList()
            .ForEach(tile => CreateBox(new Vector3(2,2,2), parents[ParentObject.PerimeterParent].transform, tile.Position)));

   

}
