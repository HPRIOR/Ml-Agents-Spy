using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static RandomHelper;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;



/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup, IGetTileTypes
{
    private readonly int _mapScale;
    private readonly int _mapDifficulty;
    private readonly int _matrixSize;
    private readonly int _exitCount;
    private readonly int _guardAgentCount;
    private readonly Dictionary<ParentObject, GameObject> _parentDictionary;
    private Dictionary<TileType, List<Tile>> _tileTypes = new Dictionary<TileType, List<Tile>>();
    private TileMatrix _tileMatrixProducer;
    private List<List<Tile>> _tileMatrix;

    public EnvSetup(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
        Dictionary<ParentObject, GameObject> parentDictionary)
    {
        _mapScale = mapScale;
        _mapDifficulty = mapDifficulty;
        _matrixSize = mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;
        _exitCount = exitCount;
        _guardAgentCount = guardAgentCount;
        _parentDictionary = parentDictionary;
        _tileMatrixProducer = new TileMatrix(_parentDictionary[ParentObject.TopParent].transform.localPosition, _matrixSize);
        _tileMatrix = _tileMatrixProducer.Tiles;
        Enum.GetValues(typeof(TileType)).Cast<TileType>().ToList().ForEach(tileType => _tileTypes.Add(tileType, new List<Tile>()));
    }

    /// <summary>
    /// Called by Academy class in the SceneController to produce a new env for training instance
    /// </summary>
    public void SetUpEnv()
    {
        ModifyTileLogic();
        PopulateEnv(_tileMatrix, _parentDictionary, _mapScale);

        // DebugFirstInstance(_tiles, _parents, tile => tile.HasSpy);
        // DebugAll(_tiles, _parents, tile => tile.HasGuard);
        // DebugAll(_tiles, _parents, tile => tile.OnSpyPath);

    }

    /// <summary>
    /// Changes logic within each tile, helping to generate the environment and agent tiles
    /// Loops until appropriate environment found, or throws an error
    /// </summary>
    private void ModifyTileLogic()
    {
        var flag = true;
        int count = 0;

        while (flag)
        {
            TileMatrix matrixClone = (TileMatrix)_tileMatrixProducer.Clone();
            List<List<Tile>> tilesCopy = matrixClone.Tiles;

            Tile spyTile = SetSpyTile(tilesCopy, _matrixSize);
            CreateInitialEnv(tilesCopy, _matrixSize);
            SetEnvDifficulty(tilesCopy, _matrixSize, _mapDifficulty);
            
            IPathFinder pathFinder = new PathFinder();
            pathFinder.GetPath(spyTile);
            
            List<Tile> potentialExitTiles = PotentialExitTiles(tilesCopy, _matrixSize);
            // ensures that there are no more exits than the potential number of exits
            int maxExits = _exitCount > potentialExitTiles.Count / 2 ? potentialExitTiles.Count / 2 : _exitCount;
            // ensures there is at most -1 guards to exits
            int maxGuards = _guardAgentCount >= maxExits ? maxExits - 1 : _guardAgentCount;

            if (maxExits > 1)
            {
                SetExits(potentialExitTiles, maxExits);
                List<Tile> potentialGuardSpawnTiles = PotentialGuardSpawnTiles(tilesCopy, _mapScale, _matrixSize);

                if (GuardPlacesAreAvailableIn(potentialGuardSpawnTiles, maxGuards))
                {
                    SetGuardTiles(potentialGuardSpawnTiles, maxGuards);
                    _tileMatrix = tilesCopy;
                    flag = false;
                }
                else
                {
                    count += 1;
                    if (count > 100) throw new MapCreationException("Not enough free tiles to place guards", _matrixSize, _mapDifficulty, 
                            maxExits, maxGuards, _exitCount, _guardAgentCount);
                    
                }
            }
            else
            {
                count += 1;
                if (count > 100) throw new MapCreationException("Exits cannot be created: \n either the map is too small for the number of exits, or the spy cannot reach enough exit tiles", _matrixSize, _mapDifficulty,
                        maxExits, maxGuards, _exitCount, _guardAgentCount);
                
            }
        }
    }

    /// <summary>
    /// Set the spawn tile of the Spy agent in the first row of the tile matrix
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    private Tile SetSpyTile(List<List<Tile>> tileMatrix, int matrixSize)
    {
        int y = 1;
        int x = GetParityRandom(1, matrixSize - 1, ParityEnum.Even);
        tileMatrix[x][y].HasSpy = true;
        return tileMatrix[x][y];
    }

    /// <summary>
    /// Sets environment tiles on the perimeter and on their default positions in the middle
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    private static void CreateInitialEnv(List<List<Tile>> tileMatrix, int matrixSize)
    {
        foreach (var tileRow in tileMatrix)
        {
            foreach (var tile in tileRow)
            {
                if (CanPlacePerimeter(tile, matrixSize)) tile.HasEnv = true;
                else if (CanPlaceMiddle(tile, matrixSize)) tile.HasEnv = true;
            }
        }
    }

    /// <summary>
    /// Sets a random group of environment tiles 
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <param name="mapDifficulty">Number of tiles to randomly set</param>
    private static void SetEnvDifficulty(List<List<Tile>> tileMatrix, int matrixSize, int mapDifficulty)
    {
        List<Tile> freeTiles =
            (from tileRow in tileMatrix
             from tile in tileRow
             where CanPlaceEnvDifficulty(tile, matrixSize)
             select tile)
            .ToList();

        // Defaults to max free guardSpawnTiles if difficulty is higher. Ensures max 1 env-block per tile
        var checkDifficultyCount = mapDifficulty > freeTiles.Count ? freeTiles.Count : mapDifficulty;

        List<int> randSequence = RandomHelper.GetUniqueRandomList(mapDifficulty, freeTiles.Count);

        for (int i = 0; i < checkDifficultyCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            tile.HasEnv = true;
        }
    }

    /// <summary>
    /// Checks if a tile can be used as an environment tile to increase difficulty 
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if appropriate tile</returns>
    private static bool CanPlaceEnvDifficulty(Tile tile, int matrixSize) =>
        !tile.IsExit & !tile.HasEnv & !tile.HasGuard & !tile.HasSpy & !CanPlacePerimeter(tile, matrixSize) & !CanPlaceMiddle(tile, matrixSize);

    /// <summary>
    /// Checks if tile can be used as a default environment tile
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    private static bool CanPlaceMiddle(Tile tile, int matrixSize) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !(tile.Coords.x == 0
            || tile.Coords.x == matrixSize
            || tile.Coords.y == 0
            || tile.Coords.y == matrixSize);

    /// <summary>
    /// Checks if tile is on the outside perimeter of matrix
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>true if tile is on the perimeter</returns>
    private static bool CanPlacePerimeter(Tile tile, int matrixSize) =>
        (tile.Coords.x == 0
         || tile.Coords.x == matrixSize
         || tile.Coords.y == 0
         || tile.Coords.y == matrixSize)
        & !tile.IsExit;

    /// <summary>
    /// Gets perimeter tiles from farthest side of perimeter which are on the Spy agents potential path
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles </param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns>List of tiles which are candidates for exit tiles</returns>
    private static List<Tile> PotentialExitTiles(List<List<Tile>> tileMatrix, int matrixSize) =>
        (from tileRow in tileMatrix
            from tile in tileRow
            where tile.Coords.y == matrixSize
            where tile.AdjacentTile[Direction.S].OnPath
            select tile).ToList();


    /// <summary>
    /// Places exits randomly along candidate exit tiles so long as they are not next to each other
    /// </summary>
    /// <param name="potentialExitTiles">Candidate exit tiles</param>
    /// <param name="exitCount">Number of exits to set</param>
    private static void SetExits(List<Tile> potentialExitTiles, int exitCount)
    {
        Random r = new Random();
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

    /// <summary>
    /// Gets a list of tiles which are both on the Spy agents path and in the guard spawn area
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="mapScale">Size of map corresponding to plane scale</param>
    /// <param name="matrixSize">Size of Matrix</param>
    /// <returns>List of appropriate tiles</returns>
    private static List<Tile> PotentialGuardSpawnTiles(List<List<Tile>> tileMatrix, int mapScale, int matrixSize) => 
        (from tileRow in tileMatrix
            from tile in tileRow
            where tile.OnPath
            where tile.Coords.x % 2 == 0
            where InGuardSpawnAreaY(tile, mapScale, matrixSize)
            select tile).ToList();

    /// <summary>
    /// Checks that there are enough spawn tiles available and at least one guard agent being spawned
    /// </summary>
    /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
    /// <param name="guardCount">Number of guards agents to spawn</param>
    /// <returns>true if there are enough spawn tiles available and at least one guard agent being spawned</returns>
    private static bool GuardPlacesAreAvailableIn(List<Tile> guardSpawnTiles, int guardCount) => 
        guardCount <= guardSpawnTiles.Count && guardCount >= 1;

    /// <summary>
    /// Randomly sets guard spawn tiles out of the candidate tiles 
    /// </summary>
    /// <param name="guardSpawnTiles">Candidate tiles for guard spawning</param>
    /// <param name="guardCount">Number of guards agents to spawn</param>
    private static void SetGuardTiles(List<Tile> guardSpawnTiles, int guardCount)
    {
        var randomList = GetUniqueRandomList(guardCount, guardSpawnTiles.Count);
        for (int i = 0; i < guardCount; i++) guardSpawnTiles[randomList[i]].HasGuard = true;
    }

    /// <summary>
    /// Checks if tile is in the guard spawn area (1 row if mapScale less than 3, else 3 rows)
    /// </summary>
    /// <param name="tile">Tile to Check</param>
    /// <param name="mapScale">Size of the map corresponding to scale of plane</param>
    /// <param name="matrixSize">Size of tile matrix</param>
    /// <returns>True if tile is in the guard spawn area </returns>
    private static bool InGuardSpawnAreaY(Tile tile, int mapScale, int matrixSize) =>
        mapScale >= 1 & mapScale <= 3 & tile.Coords.y == matrixSize - 1 
        || mapScale > 3 & (tile.Coords.y >= matrixSize - 1 || tile.Coords.y <= matrixSize - 3);

    /// <summary>
    /// Creates box with given scale, GameObject parent, and 3D position
    /// </summary>
    /// <param name="scale">Size of box</param>
    /// <param name="parent">Parent GameObject of box</param>
    /// <param name="position">3D position of box</param>
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
    /// <param name="scale">Size of the plane</param>
    /// <param name="parent">Parent GameObject of the plane</param>
    private static void CreatePlane(Vector3 scale, Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = parent.localPosition;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
    }
    
    /// <summary>
    /// Creates 3D objects based on tile position and logic
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    private void PopulateEnv(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, int mapScale)
    {
        CreatePlane(
            scale: new Vector3(mapScale, 1, mapScale),
            parent: _parentDictionary[ParentObject.TopParent].transform
        );

        tileMatrix
            .ForEach(tileRow => tileRow
                .Where(tile => tile.HasEnv)
                .ToList()
                .ForEach(tile =>
                    CreateBox(new Vector3(2, 2, 2), parentDictionary[ParentObject.EnvParent].transform, tile.Position)));
    }

    /// <summary>
    /// Returns dictionary of Tile type references and corresponding tiles
    /// </summary>
    /// <returns>Tile type references and corresponding tiles</returns>
    public Dictionary<TileType, List<Tile>> GetTileTypes()
    {
        foreach (var tileRow in _tileMatrix) foreach (var tile in tileRow)
        {
            if (tile.IsExit) _tileTypes[TileType.ExitTiles].Add(tile);
            else if (tile.HasSpy) _tileTypes[TileType.SpyTile].Add(tile);
            else if (tile.HasGuard) _tileTypes[TileType.GuardTiles].Add(tile);
            else if (tile.HasEnv) _tileTypes[TileType.EnvTiles].Add(tile);
            else _tileTypes[TileType.FreeTiles].Add(tile);
        }
        return _tileTypes;
    }

    /// <summary>
    /// Creates 3D object on first tile matching predicate - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    private static void DebugFirstInstance(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
      CreateBox(new Vector3 (1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, (from tileRow in tileMatrix
          from tile in tileRow
          select tile).Where(tilePredicate).ToList()[0].Position);
   
    /// <summary>
    /// Creates 3D objects on all tiles matching predicates - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    private static void DebugAll(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
       (from tileRow in tileMatrix
           from tile in tileRow
           select tile).Where(tilePredicate).ToList().ForEach(tile =>
           CreateBox(new Vector3(1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, tile.Position));



}
