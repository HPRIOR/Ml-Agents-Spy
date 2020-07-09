using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RandomHelper;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;



/// <summary>
/// This class generates the environment 
/// </summary>
public class EnvSetup : IEnvSetup, IGetTileTypes
{
    private readonly int _mapScale;
    private readonly int _mapDifficulty;
    private readonly int _matrixSize;
    private readonly int _mapCreationAttempts;
    private readonly int _exitCount;
    private readonly int _guardAgentCount;
    private readonly Dictionary<ParentObject, GameObject> _parents;
    private Dictionary<TileType, List<Tile>> _tileTypes = new Dictionary<TileType, List<Tile>>();
    private TileMatrix _tileMatrix;
    private List<List<Tile>> _tiles;

    public EnvSetup(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
        Dictionary<ParentObject, GameObject> parents, int mapCreationAttempts = 100)
    {
        _mapScale = mapScale;
        _mapDifficulty = mapDifficulty;
        _matrixSize = mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;
        _mapCreationAttempts = mapCreationAttempts;
        _exitCount = exitCount;
        _guardAgentCount = guardAgentCount;
        _parents = parents;
        _tileMatrix = new TileMatrix(_parents[ParentObject.TopParent].transform.localPosition, _matrixSize);
        _tiles = _tileMatrix.Tiles;
        Enum.GetValues(typeof(TileType)).Cast<TileType>().ToList().ForEach(tileType => _tileTypes.Add(tileType, new List<Tile>()));
    }

    /// <summary>
    /// This is called by the Academy in the SceneController to produce a new env
    /// </summary>
    public void SetUpEnv()
    {
        ModifyTileLogic();
        PopulateEnv(_tiles, _parents, _mapScale);

        // DebugFirstInstance(_tiles, _parents, tile => tile.HasSpy);
        // DebugAll(_tiles, _parents, tile => tile.HasGuard);
        // DebugAll(_tiles, _parents, tile => tile.OnSpyPath);

    }

    /// <summary>
    /// This changes logic within each tile, generating environment and agent tileMatrix
    /// It will loop for as many times as specified in _mapCreationAttempts (default = 100) eventually throwing an error,
    /// or the tile logic will be modified.
    /// Algorithm: Find spy tile -> create env -> find path to opposite end -> create exits along path -> place agents along path
    /// </summary>
    private void ModifyTileLogic()
    {
        var flag = true;
        int count = 0;

        while (flag)
        {
            TileMatrix matrixClone = (TileMatrix)_tileMatrix.Clone();
            List<List<Tile>> tilesCopy = matrixClone.Tiles;

            Tile spyTile = SetSpyTile(tilesCopy, _matrixSize);
            SetInitialEnv(tilesCopy, _matrixSize);
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
                List<Tile> potentialGuardSpawnTiles = PotentialGuardSpawnTiles(tilesCopy, _matrixSize, _mapScale);

                if (GuardPlacesAreAvailableIn(potentialGuardSpawnTiles, maxGuards))
                {
                    SetGuardTiles(potentialGuardSpawnTiles, maxGuards);
                    _tiles = tilesCopy;
                    flag = false;
                }
                else
                {
                    count += 1;
                    if (count > _mapCreationAttempts)
                    {
                        throw new EnvCreationException("Not enough free tileMatrix to place guards");
                    }
                }
            }
            else
            {
                count += 1;
                if (count > _mapCreationAttempts)
                {
                    throw new EnvCreationException("Exits cannot be created: \n either the map is too small for the number of exits, or the spy cannot reach enough exit tileMatrix");
                }
            }
        }
    }

    /// <summary>
    /// Sets a random tile in the first row as Spy Agent spawn point 
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="matrixSize">Size of the tile matrix</param>
    /// <returns>Reference to Spy spawn tile</returns>
    private static Tile SetSpyTile(List<List<Tile>> tileMatrix, int matrixSize)
    {
        int y = 1;
        int x = GetParityRandom(1, matrixSize - 1, ParityEnum.Even);
        tileMatrix[x][y].HasSpy = true;
        return tileMatrix[x][y];
    }
    
    /// <summary>
    /// Sets environment tiles at the perimeter and at equal points in the center
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles matrix</param>
    /// <param name="matrixSize">Size of the tile Matrix</param>
    private static void SetInitialEnv(List<List<Tile>> tileMatrix, int matrixSize)
    {
        foreach (var tileRow in tileMatrix)
        {
            foreach (var tile in tileRow)
            {
                if (CanSetPerimeter(tile, matrixSize)) tile.HasEnv = true;
                else if (CanSetMiddle(tile, matrixSize)) tile.HasEnv = true;
            }
        }
    }

    /// <summary>
    /// Sets environment tiles at random points on the map
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="matrixSize">Size of Tile Matrix</param>
    /// <param name="mapDifficulty">Number of environment tileMatrix to randomly set</param>
    private static void SetEnvDifficulty(List<List<Tile>> tileMatrix, int matrixSize, int mapDifficulty)
    {
        List<Tile> freeTiles =
            (from tileRow in tileMatrix
             from tile in tileRow
             where CanSetEnvDifficulty(tile, matrixSize)
             select tile)
            .ToList();

        // Defaults to max free potentialGuardSpawnTiles if difficulty is higher. Ensures max 1 env-block per tile
        var checkDifficultyCount = mapDifficulty > freeTiles.Count ? freeTiles.Count : mapDifficulty;

        List<int> randSequence = GetUniqueRandomList(mapDifficulty, freeTiles.Count);

        for (int i = 0; i < checkDifficultyCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            tile.HasEnv = true;
        }
    }

    /// <summary>
    /// Checks if tile can be set as a difficulty increasing environment tile
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of tile matrix</param>
    /// <returns>true if tile can be set</returns>
    private static bool CanSetEnvDifficulty(Tile tile, int matrixSize) =>
        !tile.IsExit
        & !tile.HasEnv 
        & !tile.HasGuard 
        & !tile.HasSpy 
        & !CanSetPerimeter(tile, matrixSize) 
        & !CanSetMiddle(tile, matrixSize);

    /// <summary>
    /// Checks if tile can be set to a default environment tile in the middle of the map
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize">Size of the tile matrix</param>
    /// <returns>true if tile can be set</returns>
    private static bool CanSetMiddle(Tile tile, int matrixSize) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !(tile.Coords.x == 0
            || tile.Coords.x == matrixSize
            || tile.Coords.y == 0
            || tile.Coords.y == matrixSize);

    /// <summary>
    /// Checks if a tile is on the outside perimeter of the map
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="matrixSize"></param>
    /// <returns>true if tile is on the perimeter</returns>
    private static bool CanSetPerimeter(Tile tile, int matrixSize) =>
        (tile.Coords.x == 0
         || tile.Coords.x == matrixSize
         || tile.Coords.y == 0
         || tile.Coords.y == matrixSize)
        & !tile.IsExit;

    /// <summary>
    /// Checks which perimeter tiles at the opposite end to the spy have a south adjacent which is on the spy's potential path 
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="matrixSize">Size of Matrix</param>
    /// <returns>List of tileMatrix which can be used as exit tileMatrix</returns>
    private static List<Tile> PotentialExitTiles(List<List<Tile>> tileMatrix, int matrixSize) =>
        (from tileRow in tileMatrix
            from tile in tileRow
            where tile.Coords.y == matrixSize
            where tile.AdjacentTile[Direction.S].OnSpyPath
            select tile).ToList();

    /// <summary>
    /// Sets which tiles will be exit points for spy agents, ensuring that no two exits are adjacent 
    /// </summary>
    /// <param name="potentialExitTiles">List of tiles which can be reached by spy</param>
    /// <param name="exitCount">Number of exits to be set</param>
    private static void SetExits(List<Tile> potentialExitTiles, int exitCount)
    {
        var r = new Random();
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
    /// Returns a list of tileMatrix which are both in the guard agent spawn area and along the potential spy path
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <param name="mapScale">Scale of the map area corresponding to scale of plane</param>
    /// <returns>List of tileMatrix which can be used to spawn guard agents</returns>
    private static List<Tile> PotentialGuardSpawnTiles(List<List<Tile>> tileMatrix, int matrixSize, int mapScale) => 
        (from tileRow in tileMatrix
            from tile in tileRow
            where tile.OnSpyPath
            where tile.Coords.x % 2 == 0
            where InGuardSpawnAreaY(tile, mapScale, matrixSize)
            select tile).ToList();

    /// <summary>
    /// Checks that the number of guard agents is smaller or equal to the number of places available to spawn them and that there is at least one guard agent
    /// </summary>
    /// <param name="guardSpawnTiles">List of tiles in which the guard agent can spawn</param>
    /// <param name="guardCount">Number of guards to spawn</param>
    /// <returns>true if there are enough guard agent spawn tiles and there is at least one guard</returns>
    private static bool GuardPlacesAreAvailableIn(List<Tile> guardSpawnTiles, int guardCount) => 
        guardCount <= guardSpawnTiles.Count & guardCount >= 1;

    /// <summary>
    /// Randomly sets spawn guard agent spawn points among acceptable tileMatrix
    /// </summary>
    /// <param name="potentialGuardSpawnTiles">List of tiles in which guard agents can be placed</param>
    /// <param name="guardCount">Number of guards to be spawned</param>
    private static void SetGuardTiles(List<Tile> potentialGuardSpawnTiles, int guardCount)
    {
        var randomList = GetUniqueRandomList(guardCount, potentialGuardSpawnTiles.Count);
        for (int i = 0; i < guardCount; i++) potentialGuardSpawnTiles[randomList[i]].HasGuard = true;
        
    }

    /// <summary>
    /// Checks if tile is in the guard spawn area
    /// 1 row if mapScale is less than 3, 3 rows if more than 3
    /// </summary>
    /// <param name="tile">Tile to check</param>
    /// <param name="mapScale">Map size corresponding to scale of plane</param>
    /// <param name="matrixSize"></param>
    /// <returns>true if tile is a appropriate spawn point for guard agetns</returns>
    private static bool InGuardSpawnAreaY(Tile tile, int mapScale, int matrixSize) =>
        mapScale >= 1 & mapScale <= 3 & tile.Coords.y == matrixSize - 1 
        || mapScale > 3 & (tile.Coords.y >= matrixSize - 1 || tile.Coords.y <= matrixSize - 3);

    /// <summary>
    /// Creates box with given parent, position and scale
    /// </summary>
    /// <param name="parent">Parent GameObject</param>
    /// <param name="scale"> Size of the box</param>
    /// <param name="position">Vector3 position</param>
    private static void CreateBox(Transform parent, Vector3 scale, Vector3 position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = scale;
        box.transform.parent = parent;
    }

    /// <summary>
    /// Produces a plane with a specified size and position (relative to the parent)
    /// </summary>
    /// <param name="parent">Parent GameObject</param>
    /// <param name="scale">The size of the plane</param>
    private static void CreatePlane(Transform parent, Vector3 scale)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = parent.localPosition;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
    }

    /// <summary>
    /// Creates the 3D objects based on the logic contained within the tile matrix
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary of GameObject parent references</param>
    /// <param name="mapScale">Size of map</param>
    private static void PopulateEnv(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary,
        int mapScale)
    {
        CreatePlane(parent: parentDictionary[ParentObject.TopParent].transform, scale: new Vector3(mapScale, 1, mapScale));

        tileMatrix
            .ForEach(tileRow => tileRow
                .Where(tile => tile.HasEnv)
                .ToList()
                .ForEach(tile =>
                    CreateBox(parentDictionary[ParentObject.EnvParent].transform, new Vector3(2, 2, 2), tile.Position)));
    }

    /// <summary>
    /// Exposed method which gives access to tile information created in the environment setup
    /// </summary>
    /// <returns>Dictionary mapping tile types to corresponding list of tileMatrix from matrix</returns>
    public Dictionary<TileType, List<Tile>> GetTileTypes()
    {
        foreach (var tileRow in _tiles) foreach (var tile in tileRow)
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
    /// Creates 3D GameObject on first tile which matches predicate - allows for visual debugging of spy placement
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing references to GameObjects</param>
    /// <param name="tilePredicate">Predicate which allows for logic to be injected into where clause, thus isolating a specific tile to debug</param>
    private static void DebugFirstInstance(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
      CreateBox(
          parentDictionary[ParentObject.DebugParent].transform, new Vector3 (1, 1, 1), (from tileRow in tileMatrix
              from tile in tileRow
              select tile).Where(tilePredicate).ToList()[0].Position);

    /// <summary>
    /// Creates 3D GameObjects on all tiles matching predicate
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing references to GameObjects</param>
    /// <param name="tilePredicate">Predicate which allows for logic to be injected into where clause, thus isolating specific tiles to debug</param>
    private static void DebugAll(List<List<Tile>> tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
       (from tileRow in tileMatrix
           from tile in tileRow
           select tile).Where(tilePredicate).ToList().ForEach(tile =>
           CreateBox(parentDictionary[ParentObject.DebugParent].transform, new Vector3(1, 1, 1), tile.Position));



}
