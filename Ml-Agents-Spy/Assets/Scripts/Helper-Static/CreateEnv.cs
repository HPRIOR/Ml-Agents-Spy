using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreateEnv 
{
    /// <summary>
    /// Creates box with given scale, GameObject parent, and 3D position
    /// </summary>
    /// <param name="scale">Size of box</param>
    /// <param name="parent">Parent GameObject of box</param>
    /// <param name="position">3D position of box</param>
    public static void CreateBox(Vector3 scale, Transform parent, Vector3 position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localPosition = position + new Vector3(0, 0.5f, 0);
        box.transform.localScale = scale;
        box.transform.parent = parent;
        box.tag = "env";
    }

    /// <summary>
    /// Produces a plane with a specified size and position (relative to the parent)
    /// </summary>
    /// <param name="scale">Size of the plane</param>
    /// <param name="parent">Parent GameObject of the plane</param>
    public static void CreatePlane(Vector3 scale, Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = parent.position;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
    }
}
