using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static RandomHelper;
using Vector3 = UnityEngine.Vector3;



/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup
{
    private readonly int _mapScale;
    private readonly int _mapDifficulty;
    private readonly int _matrixSize;
    private readonly int _exitCount;
    private readonly int _guardAgentCount;
    private readonly int _mapCreationTolerance;
    private readonly Dictionary<ParentObject, GameObject> _parentDictionary;
    private Dictionary<TileType, List<Tile>> _tileTypes = new Dictionary<TileType, List<Tile>>();
    private TileMatrix _tileMatrixProducer;
    private Tile[,] _tileMatrix;

    public EnvSetup(int mapScale, int mapDifficulty, int exitCount, int guardAgentCount,
        Dictionary<ParentObject, GameObject> parentDictionary, int mapCreationTolerance = 500)
    {
        _mapScale = mapScale;
        _mapDifficulty = mapDifficulty;
        _matrixSize = mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;
        _exitCount = exitCount;
        _guardAgentCount = guardAgentCount;
        _parentDictionary = parentDictionary;
        _tileMatrixProducer = new TileMatrix(_parentDictionary[ParentObject.TopParent].transform.localPosition, _matrixSize);
        _tileMatrix = _tileMatrixProducer.Tiles;
        _mapCreationTolerance = mapCreationTolerance;
        
        Enum.GetValues(typeof(TileType)).Cast<TileType>().ToList().ForEach(tileType => _tileTypes.Add(tileType, new List<Tile>()));
    }

    /// <summary>
    /// Called by Academy class in the SceneController to produce a new env for training instance
    /// </summary>
    public void SetUpEnv()
    {
        ModifyTileLogic();
        PopulateEnv(_tileMatrix, _parentDictionary, _mapScale);
        _tileTypes = GetTileTypes();

        // DebugFirstInstance(_tileMatrix, _parentDictionary, tile => tile.HasSpy);
        // DebugAll(_tileMatrix, _parentDictionary, tile => tile.HasGuard);
        // DebugAll(_tileMatrix, _parentDictionary, tile => tile.OnPath);

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
            Tile[,] tilesCopy = matrixClone.Tiles;
            Tile spyTile = SetSpyTile(tilesCopy, _matrixSize);
            
            IEnvTileLogic envTileLogic = new EnvTileLogic(tilesCopy, _matrixSize, _mapDifficulty);
            envTileLogic.SetEnvTiles();
            IPathFinder pathFinder = new PathFinder();
            pathFinder.GetPath(spyTile);
            IExitFinder exitFinder = new ExitFinder(tilesCopy, _matrixSize, _exitCount);
            int maxExits = exitFinder.ExitCount;
            // ensures there is at most -1 guards to exits
            int maxGuards = _guardAgentCount >= maxExits ? maxExits - 1 : _guardAgentCount;
            IGuardLogic guardLogic = new GuardLogic(tilesCopy, _mapScale, _matrixSize, maxGuards);

            // tests could be done inside classes instead of here, try catch could replaces
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
    /// Set the spawn tile of the Spy agent in the first row of the tile matrix
    /// </summary>
    /// <param name="tileMatrix">Matrix of Tiles</param>
    /// <param name="matrixSize">Size of matrix</param>
    /// <returns></returns>
    private Tile SetSpyTile(Tile[,] tileMatrix, int matrixSize)
    {
        int y = 1;
        int x = GetParityRandom(1, matrixSize - 1, ParityEnum.Even);
        tileMatrix[x,y].HasSpy = true;
        return tileMatrix[x,y];
    }
    
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
    /// <param name="mapScale">Size of map corresponding to scale of plane</param>
    private void PopulateEnv(Tile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, int mapScale)
    {
        var calcMapScale = mapScale % 2 == 0 ? mapScale + .2f : mapScale + .4f;
        CreatePlane(
            scale: new Vector3(calcMapScale, 1, calcMapScale),
            parent: _parentDictionary[ParentObject.TopParent].transform
        );
        foreach (var tile in tileMatrix)
        {
            if (tile.HasEnv) CreateBox(new Vector3(2, 2, 2), parentDictionary[ParentObject.EnvParent].transform, tile.Position);
        }
    }

    /// <summary>
    /// Returns dictionary of Tile type references and corresponding tiles
    /// </summary>
    /// <returns>Tile type references and corresponding tiles</returns>
    public Dictionary<TileType, List<Tile>> GetTileTypes()
    {
        foreach (var tile in _tileMatrix)
        {
            if (tile.IsExit) _tileTypes[TileType.ExitTiles].Add(tile);
            else if (tile.HasSpy) _tileTypes[TileType.SpyTile].Add(tile);
            else if (tile.HasGuard) _tileTypes[TileType.GuardTiles].Add(tile);
            else if (tile.HasEnv) _tileTypes[TileType.EnvTiles].Add(tile);
            else _tileTypes[TileType.FreeTiles].Add(tile);
        }
        return _tileTypes;
    }

    public List<Tile> GetSpyTile() => _tileTypes[TileType.SpyTile];

    public List<Tile> GetGuardTiles() => _tileTypes[TileType.GuardTiles];

    public List<Tile> GetExitTiles() => _tileTypes[TileType.ExitTiles];


    /// <summary>
    /// Creates 3D object on first tile matching predicate - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    private static void DebugFirstInstance(Tile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
      CreateBox(new Vector3 (1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, (
          from Tile tile in tileMatrix
          select tile).Where(tilePredicate).ToList()[0].Position);
   
    /// <summary>
    /// Creates 3D objects on all tiles matching predicates - used for visual debugging
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="tilePredicate">Injects predicate into where clause</param>
    private static void DebugAll(Tile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary, Func<Tile, bool> tilePredicate) =>
       (from Tile tile in tileMatrix
           select tile).Where(tilePredicate).ToList().ForEach(tile =>
           CreateBox(new Vector3(1, 1, 1), parentDictionary[ParentObject.DebugParent].transform, tile.Position));

}
