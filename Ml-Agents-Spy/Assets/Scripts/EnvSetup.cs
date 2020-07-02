using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

/// <summary>
/// This class generates the environment environment 
/// </summary>
public class EnvSetup : IEnvSetup
{
    private readonly int _mapSize;
    private readonly int _mapComplexity;
    private readonly Dictionary<ParentObject, GameObject> _parentObjects;

  
    private readonly List<List<Tile>> _tiles;
    private readonly int _gridMapSize;

    public EnvSetup(int mapSize, int mapComplexity, Dictionary<ParentObject, GameObject> parentObjects)
    {
        _mapSize = mapSize;
        _mapComplexity = mapComplexity;
        _gridMapSize = GridMapSize(_mapSize);
        _parentObjects = parentObjects;
        _tiles = new TileManager(_mapSize, _parentObjects[ParentObject.TopParent].transform.localPosition, _gridMapSize).Tiles;

    }

    /// <summary>
    /// This is called by the Academy in the SceneController to produce a new env
    /// </summary>
    public void CreateEnv()
    {
        GameObject plane = CreatePlane(
            scale: new Vector3(_mapSize, 1, _mapSize),
            parent: _parentObjects[ParentObject.TopParent].transform
            );
        CreatePerimeter(_tiles, _parentObjects, _gridMapSize);
    }

    /// <summary>
    /// Defines the height/width of the centre of the outermost tile
    /// </summary>
    /// <param name="mapSize">the scale of the plane on which the tiles will be set</param>
    /// <returns>max height/width from centre of outermost tile</returns>
    private int GridMapSize(int mapSize) =>
        mapSize % 2 == 0 ? (mapSize * 10) / 2 : ((mapSize * 10) / 2) + 1;


    private GameObject CreateBox(float scale, Transform parent, Vector3 position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = new Vector3(scale, scale, scale);
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
                if (CanPlacePerimeter(tile)) CreateBox(2, parents[ParentObject.PerimeterParent].transform, tile.Position);
                if (CanPlaceMiddle(tile)) CreateBox(2, parents[ParentObject.MiddleParent].transform, tile.Position);
            }
        }
    }

    private bool CanPlacePerimeter(Tile tile) => 
        (tile.Coords.x == 0 
         || tile.Coords.x == _gridMapSize
         || tile.Coords.y == 0
         || tile.Coords.y == _gridMapSize) 
        & !tile.IsExit;

    private bool CanPlaceMiddle(Tile tile) =>
        (tile.Coords.y % 2 == 0 & tile.Coords.x % 2 == 0)
        & !CanPlacePerimeter(tile);

}
