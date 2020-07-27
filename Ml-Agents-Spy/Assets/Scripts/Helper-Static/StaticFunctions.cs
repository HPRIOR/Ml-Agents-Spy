using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class StaticFunctions
{
    public static int MapScaleToMatrixSize(int mapScale) =>
        mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;

    public static int MatrixLengthToMapScale(int matrixLength)
    {
        int squareRootOfMatrixSize = (int)Math.Sqrt(matrixLength);
        var matrixSize = squareRootOfMatrixSize - 1;
        return matrixSize % 10 == 0 ? (matrixSize * 2) / 10 : ((matrixSize - 1) * 2) / 10;
    }



    public static float MaxLocalDistance(int mapScale) => mapScale % 2 == 0 ?
        (mapScale * 5) - 1.3f :
        (mapScale * 5) - 0.3f;

    public static float MaxVectorDistanceToExit(int mapScale) => mapScale % 2 == 0 ?
        (mapScale * 10) - 1.4f :
        (mapScale * 10) + 0.6f;

    public static float NormalisedFloat(float min, float max, float current) =>
        (current - min) / (max - min);

    /// <summary>
    /// Returning 0 on current == 0 needed for trail memory to indicate a lack of observation to agent
    /// </summary>
    public static float NormalisedMemoryFloat(float min, float max, float current) => current == 0 ? 0f :
        (current - min) / (max - min);
}
