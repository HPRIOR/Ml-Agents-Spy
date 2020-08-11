using Training;
using UnityEngine;

public static class VectorConversions
{
    
    public static Vector3 RayCastHitLocation(Quaternion rotation, Vector3 position, float distance)
    {
        Vector3 direction = rotation * Vector3.forward;
        return position + (direction  * distance);
    }
    public static Vector2 ConvertToVector2(Transform t)
    {
        var position = t.position;
        return new Vector2(position.x, position.z);
    }

    public static Vector2 ConvertToVector2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 GetLocalPosition(Vector3 position, TrainingInstanceController trainingInstance)
    {
        return position - trainingInstance.transform.localPosition;
    }
}