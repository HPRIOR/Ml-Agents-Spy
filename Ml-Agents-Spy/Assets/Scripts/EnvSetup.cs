using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.MLAgents;
using UnityEngine;
using static ClassExtensions;
using static RandomHelper;


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
    private readonly List<List<Tile>> _tiles;
    private List<Tile> _freeTiles = new List<Tile>();


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
        
        CreateInitialEnv(_tiles, _freeTiles, _parents, _gridMapSize);
        AddEnvBoxComplexity(_freeTiles, _mapComplexity, _parents);
        SetAgentTile(_tiles, _gridMapSize, _guardAgentCount, _mapSize);
    }

    private void SetAgentTile(List<List<Tile>> tiles, int maxGridMapSize, int guardCount, int mapSize)
    {
        System.Random r = new System.Random();
        // place spy
        int y = mapSize >= 1 & mapSize <= 3 ? 1 : GetParityRandom(1, 3, ParityEnum.Odd);
        int x = GetParityRandom(1, maxGridMapSize - 1, ParityEnum.Even);
        SetSpyTile(x, y, tiles);
        // uncomment to see placement of spy
        // CreateBox(new Vector3(1,1,1), _parents[ParentObject.PerimeterParent].transform, tiles[x][y].Position);
        // place guard - loop through guard guard count, check each time that the coordinates are different
        y = mapSize >= 1 & mapSize <= 3 ? maxGridMapSize - 1 : GetParityRandom(maxGridMapSize  - 4, maxGridMapSize - 1, ParityEnum.Odd);
        x = GetParityRandom(1, maxGridMapSize - 1, ParityEnum.Even);

    }

    private static void SetGuardTile(int x, int y, List<List<Tile>> tiles) => 
        tiles[x][y].HasGuard = true;


    private static void SetSpyTile(int x, int y, List<List<Tile>> tiles) =>
        tiles[x][y].HasSpy = true;

    private static void PlaceExits()
    {

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
        & !CanPlacePerimeter(tile, maxLen);

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

   

    /// <summary>
    /// Adds complexity to the map by filling in tiles and blocking off areas, reducing the possible the routes to the finish
    /// </summary>
    /// <param name="freeTiles"></param>
    /// <param name="mapComplexity"></param>
    /// <param name="parents"></param>
    private void AddEnvBoxComplexity(List<Tile> freeTiles, int mapComplexity, Dictionary<ParentObject, GameObject> parents)
    {
        // Defaults to max free tiles if complexity is higher. Ensures max 1 env-block per tile
        var checkComplexityCount = mapComplexity > freeTiles.Count ? freeTiles.Count : mapComplexity;

        // unique list of indexes up to the amount of tiles 
        List<int> randSequence = RandomHelper.GetUniqueRandomList(count: mapComplexity, maxVal: freeTiles.Count);
        List<(int, int)> tilesToDeleteCoords = new List<(int, int)>();
        /*
         *  Debug.Log("complexity count: " + checkComplexityCount);
         *  Debug.Log("count of free tiles: "+ freeTiles.Count);/
         *  Debug.Log("count of random sequence: " + randSequence.Count);
         *  randSequence.ForEach(x => Debug.Log(x));
         */


        for (int i = 0; i < checkComplexityCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            if (!tile.HasGuard || !tile.HasSpy)
            {
                tile.HasEnv = true;
                tilesToDeleteCoords.Add(tile.Coords);
                CreateBox(new Vector3(2, 2, 2), parents[ParentObject.ComplexitiesParent].transform, tile.Position);
            }
        }

        // nasty side effect -- adds complexity 
        _freeTiles = freeTiles.Where(tile => tilesToDeleteCoords.All(coords => tile.Coords != coords)).ToList();

    }


    private Dictionary<string, int> GetAgentCoords(int guardAgentcount)
    {
        return new Dictionary<string, int>();
    }

    private void CheckAgentCoords()
    {
        PathFinder p = new PathFinder();
        p.GetExitCount(_freeTiles[_freeTiles.Count - 1]);
        Debug.Log(p.ExitCount);
    }


    private static void PopulateEnv(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents) => tiles
        .ForEach(tileRow => tileRow
            .Where(tile => tile.HasEnv)
            .ToList()
            .ForEach(tile => CreateBox(new Vector3(2,2,2), parents[ParentObject.PerimeterParent].transform, tile.Position)));

   

}
