using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RandomHelper;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;


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

    // dictionary with Enum.TileTypeEnum -> List<Tile>
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
        _tiles = new TileManager(_parents[ParentObject.TopParent].transform.localPosition, _gridMapSize).Tiles;
    }

    /// <summary>
    /// This is called by the Academy in the SceneController to produce a new env
    /// </summary>
    public void SetUpEnv()
    {
        ModifyTileLogic();
        PopulateEnv(_tiles, _parents);

        DebugFirstInstance(_tiles, _parents, tile => tile.HasSpy);
        DebugAll(_tiles, _parents, tile => tile.HasGuard);
        //DebugAll(_tiles, _parents, tile => tile.OnSpyPath);

    }

    /// <summary>
    /// This changes logic within each tile, generating environment and agent tiles
    /// </summary>
    private void ModifyTileLogic()
    {
        CreatePlane(
            scale: new Vector3(_mapSize, 1, _mapSize),
            parent: _parents[ParentObject.TopParent].transform
            );

        var spyTile = SetSpyTile(_tiles, _gridMapSize);
        CreateInitialEnv(_tiles, _gridMapSize);
        AddEnvComplexity(_tiles, _mapComplexity, _gridMapSize);
        PathFinder.GetSpyPathFrom(spyTile);
        List<Tile> potentialExitTiles = PotentialExitTiles(_tiles, _gridMapSize);


        int maxExits = _exitCount > potentialExitTiles.Count / 2 ? potentialExitTiles.Count / 2 : _exitCount;
        int maxGuards = _guardAgentCount >= maxExits ? maxExits - 1 : _guardAgentCount;

        Debug.Log(maxGuards);

        if (ExitsAreAvailableIn(potentialExitTiles, maxExits))
        {
            PlaceExits(potentialExitTiles, maxExits);
            List<Tile> potentialGuardSpawnTiles = PotentialGuardSpawnTiles(_tiles, _mapSize, _gridMapSize);
            
            if (GuardPlacesAreAvailableIn(potentialGuardSpawnTiles, maxGuards))
            {
                SetGuardTiles(potentialGuardSpawnTiles, maxGuards);
            }
            else
            {
                Debug.Log("Not enough free tiles to place guards");
            }
        }
        else
        {
            Debug.Log("Exits cannot be created: \n either the map is too small for the number of exits, or the spy cannot reach enough exit tiles");
        }
    }

    private Tile SetSpyTile(List<List<Tile>> tiles, int gridMapSize)
    {
        int y = 1;
        int x = GetParityRandom(1, gridMapSize - 1, ParityEnum.Even);
        tiles[x][y].HasSpy = true;
        return tiles[x][y];
    }
    
    private static void CreateInitialEnv(List<List<Tile>> tiles, int maxLen)
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                if (CanPlacePerimeter(tile, maxLen)) tile.HasEnv = true;
                else if (CanPlaceMiddle(tile, maxLen)) tile.HasEnv = true;
            }
        }
    }

    private static void AddEnvComplexity(List<List<Tile>> tiles, int mapComplexity, int gridMapSize)
    {
        List<Tile> freeTiles =
            (from tileRow in tiles
             from tile in tileRow
             where EnvComplexityCanPlace(tile, gridMapSize)
             select tile)
            .ToList();

        // Defaults to max free guardSpawnTiles if complexity is higher. Ensures max 1 env-block per tile
        var checkComplexityCount = mapComplexity > freeTiles.Count ? freeTiles.Count : mapComplexity;

        List<int> randSequence = RandomHelper.GetUniqueRandomList(mapComplexity, freeTiles.Count);

        for (int i = 0; i < checkComplexityCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            tile.HasEnv = true;
        }
    }

    // Env placement logic:
    private static bool EnvComplexityCanPlace(Tile tile, int gridMapSize) =>
        !tile.IsExit & !tile.HasEnv & !tile.HasGuard & !tile.HasSpy & !CanPlacePerimeter(tile, gridMapSize) & !CanPlaceMiddle(tile, gridMapSize);

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

    private static List<Tile> PotentialExitTiles(List<List<Tile>> tiles, int gridMapSize) =>
        (from tileRow in tiles
            from tile in tileRow
            where tile.Coords.y == gridMapSize
            where tile.AdjacentTile[Direction.S].OnSpyPath
            select tile).ToList();

    /// <summary>
    /// Checks if there are at least twice as many potential exit point as there are desired exit points
    /// </summary>
    /// <param name="potentialExitTiles"></param>
    /// <param name="exitCount"></param>
    /// <returns></returns>
    private static bool ExitsAreAvailableIn(List<Tile> potentialExitTiles, int exitCount) =>
        exitCount <= potentialExitTiles.Count / 2 & exitCount >= 1;

    private static void PlaceExits(List<Tile> potentialExitTiles, int exitCount)
    {
        // incorporate exits are avail logic here, and ensure exit count is smaller than number of potential exit
        System.Random r = new Random();
        for (int i = 0; i < exitCount; i++)
        {
            var selectedExit = potentialExitTiles[r.Next(0, potentialExitTiles.Count - 1)];
            if (!selectedExit.AdjacentTile[Direction.E].IsExit & !selectedExit.AdjacentTile[Direction.W].IsExit & !selectedExit.IsExit)
            {
                selectedExit.IsExit = true;
                selectedExit.HasEnv = false;
            }
            else
            {
                i -= 1;
            }
        }
    }

    private static List<Tile> PotentialGuardSpawnTiles(List<List<Tile>> tiles, int mapSize, int gridMapSize) => 
        (from tileRow in tiles
            from tile in tileRow
            where tile.OnSpyPath
            where tile.Coords.x % 2 == 0
            where InGuardSpawnAreaY(tile, mapSize, gridMapSize)
            select tile).ToList();

    private static bool GuardPlacesAreAvailableIn(List<Tile> guardSpawnTiles, int guardCount) => 
        guardCount <= guardSpawnTiles.Count & guardCount >= 1;

    private static void SetGuardTiles(List<Tile> guardSpawnTiles, int guardCount)
    {
        // guardplaceare avail logic here and ensure that guard count is less than number of avail
        var randomList = GetUniqueRandomList(guardCount, guardSpawnTiles.Count);
        for (int i = 0; i < guardCount; i++) guardSpawnTiles[randomList[i]].HasGuard = true;
        
    }

    private static bool InGuardSpawnAreaY(Tile tile, int mapSize, int gridMapSize) =>
        mapSize >= 1 & mapSize <= 3 & tile.Coords.y == gridMapSize - 1 
        || mapSize > 3 & (tile.Coords.y == gridMapSize - 1 || tile.Coords.y == gridMapSize - 3);

    private static void CreateBox(Transform parent, Vector3 position, Vector3 scale)
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
    
    private static void PopulateEnv(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents) => tiles
        .ForEach(tileRow => tileRow
            .Where(tile => tile.HasEnv)
            .ToList()
            .ForEach(tile => CreateBox(parents[ParentObject.EnvParent].transform, tile.Position, new Vector3(2,2,2))));

    private void GetAgentTiles(List<List<Tile>> tiles)
    {
        // this should return a dictionary with all the potentialExitTiles instead of fields
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

   private static void DebugFirstInstance(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents, Func<Tile, bool> tilePredicate) =>
      CreateBox(
          parents[ParentObject.DebugParent].transform, 
          (from tileRow in tiles
              from tile in tileRow
              select tile).Where(tilePredicate).ToList()[0].Position, new Vector3 (1, 1, 1)
          );

   private static void DebugAll(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents, Func<Tile, bool> tilePredicate) =>
       (from tileRow in tiles
           from tile in tileRow
           select tile).Where(tilePredicate).ToList().ForEach(tile =>
           CreateBox(parents[ParentObject.DebugParent].transform, tile.Position, new Vector3(1, 1, 1)));



}
