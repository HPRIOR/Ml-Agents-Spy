using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorConversions 
{
    public static Vector2 ConvertToVector2(Transform t) => new Vector2(t.position.x, t.position.z);

    public static Vector2 ConvertToVector2(Vector3 v) => new Vector2(v.x, v.z);
}
