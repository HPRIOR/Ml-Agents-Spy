using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup
{
    private readonly int _mapSize;
    private readonly int _mapComplexity;
    private readonly int _gridMapSize;

    private readonly Dictionary<ParentObject, GameObject> _parents;
    private readonly List<List<Tile>> _tiles;
    private List<Tile> _freeTiles = new List<Tile>();


    public EnvSetup(int mapSize, int mapComplexity, Dictionary<ParentObject, GameObject> parents)
    {
        _mapSize = mapSize;
        _mapComplexity = mapComplexity;
        _gridMapSize = GridMapSize(_mapSize);
        _parents = parents;
        _tiles = new TileManager(_mapSize, _parents[ParentObject.TopParent].transform.localPosition, _gridMapSize).Tiles;

    }

    /// <summary>
    /// This is called by the Academy in the SceneController to produce a new env
    /// </summary>
    public void CreateEnv()
    {
        GameObject plane = CreatePlane(
            scale: new Vector3(_mapSize, 1, _mapSize),
            parent: _parents[ParentObject.TopParent].transform
            );
        CreatePerimeter(_tiles, _parents, _gridMapSize);
        AddEnvBoxComplexity(_freeTiles, _mapComplexity, _parents);
        PathFinder p = new PathFinder();
        p.GetExitCount(_freeTiles[0]);
        Debug.Log(p.exitCount);
    }

    /// <summary>
    /// Defines the height/width of the centre of the outermost tile
    /// </summary>
    /// <param name="mapSize">the scale of the plane on which the tiles will be set</param>
    /// <returns>max height/width from centre of outermost tile</returns>
    private int GridMapSize(int mapSize) =>
        mapSize % 2 == 0 ? (mapSize * 10) / 2 : ((mapSize * 10) / 2) + 1;


    private GameObject CreateBox(Vector3 scale, Transform parent, Vector3 position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = scale;
        box.transform.parent = parent;
        return box;
    }

   
    /// <summary>
    /// Produces a plane with a specified size and position (relative to the parent)
    /// </summary>
    /// <param name="scale">the size of the plane</param>
    /// <param name="parent">the parent in the hierarchy window</param>
    /// <returns></returns>
    private GameObject CreatePlane(Vector3 scale, Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // sets the plane relative to the parent
        plane.transform.localPosition = parent.localPosition ;
        plane.transform.localScale = scale;
        // sets the parent of the plane object
        plane.transform.parent = parent;
        return plane;
    }

    private void CreatePerimeter(List<List<Tile>> tiles, Dictionary<ParentObject, GameObject> parents, int maxLen)
    {
        foreach (var list in tiles)
        {
            foreach (var tile in list)
            {
                if (CanPlacePerimeter(tile, maxLen)) {
                    CreateBox(
                    scale: new Vector3(2, 2, 2),
                    parent: parents[ParentObject.PerimeterParent].transform, 
                    position: tile.Position
                    );
                    tile.HasEnv = true;
                }
                else if (CanPlaceMiddle(tile, maxLen)){
                    CreateBox(
                    scale: new Vector3(2, 2, 2), 
                    parent: parents[ParentObject.MiddleParent].transform, 
                    position: tile.Position
                    );
                    tile.HasEnv = true;
                }
                else 
                {
                    if (!tile.IsExit & !tile.HasGuard & !tile.HasSpy) _freeTiles.Add(tile);
                } 
            }
        }
    }

    private bool CanPlacePerimeter(Tile tile, int maxLen) => 
        (tile.Coords.x == 0 
         || tile.Coords.x == maxLen
         || tile.Coords.y == 0
         || tile.Coords.y == maxLen) 
        & !tile.IsExit;

    private bool CanPlaceMiddle(Tile tile, int maxLen) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !CanPlacePerimeter(tile, maxLen);

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
        List<int> randSequence = RandomHelper.GetUniqueRandomList(freeTiles.Count, freeTiles.Count);
        

        for (int i = 0; i < checkComplexityCount; i++)
        {
            var tile = freeTiles[randSequence[i]];
            if (!tile.HasGuard || !tile.HasSpy)
            {
                tile.HasEnv = true;
                freeTiles.Remove(tile);
                CreateBox(new Vector3(2, 2, 2), parents[ParentObject.ComplexitiesParent].transform, tile.Position);
            }
            
        }

    }

}
