using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public static class CreateEnv
{
    /// <summary>
    ///     Creates box with given scale, GameObject parent, and 3D position
    /// </summary>
    /// <param name="scale">Size of box</param>
    /// <param name="parent">Parent GameObject of box</param>
    /// <param name="position">3D position of box</param>
    /// <param name="planeMaterial"></param>
    public static void CreateBox(Vector3 scale, Transform parent, Vector3 position, Material planeMaterial = null)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = scale;
        box.transform.parent = parent;
        box.tag = "env";
        var meshRend = box.GetComponent<MeshRenderer>();
        meshRend.material = planeMaterial;
    }

    /// <summary>
    ///     Produces a plane with a specified size and position (relative to the parent)
    /// </summary>
    /// <param name="scale">Size of the plane</param>
    /// <param name="parent">Parent GameObject of the plane</param>
    /// <param name="planeMaterial"></param>
    public static void CreatePlane(Vector3 scale, Transform parent, Material planeMaterial = null)
    {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = parent.position;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
        plane.tag = "plane";
        var meshRend = plane.GetComponent<MeshRenderer>();
        meshRend.material = planeMaterial;
    }


    /// <summary>
    ///     Creates 3D objects based on tile position and logic
    /// </summary>
    /// <param name="tileMatrix">Matrix of tiles</param>
    /// <param name="parentDictionary">Dictionary containing ParentObject references and corresponding GameObjects</param>
    /// <param name="mapScale">Size of map corresponding to scale of plane</param>
    /// <param name="materials"></param>
    public static void PopulateEnv(IEnvTile[,] tileMatrix, Dictionary<ParentObject, GameObject> parentDictionary,
        int mapScale, Material[] materials)
    {
        var adjustedMapScale = mapScale % 2 == 0 ? mapScale + .2f : mapScale + .4f;
        CreatePlane(
            new Vector3(adjustedMapScale, 1, adjustedMapScale),
            parentDictionary[ParentObject.EnvParent].transform, materials[0]);
        foreach (var tile in tileMatrix)
        {
            if (tile.HasEnv)
                CreateBox(
                    new Vector3(2, 2, 2),
                    parentDictionary[ParentObject.EnvParent].transform,
                    tile.Position, materials[1]);
            // close off exits
            if (tile.IsExit)
                CreateBox(
                    new Vector3(2, 2, 2),
                    parentDictionary[ParentObject.EnvParent].transform,
                    tile.Position + new Vector3(0, 0, 2f), materials[1]);
        }
    }
}