using System;
using System.ComponentModel;
using Enums;

public static class StaticFunctions
{
    public static int MapScaleToMatrixSize(int mapScale)
    {
        return mapScale % 2 == 0 ? mapScale * 10 / 2 : mapScale * 10 / 2 + 1;
    }

    public static int MatrixLengthToMapScale(int matrixLength)
    {
        var squareRootOfMatrixSize = (int) Math.Sqrt(matrixLength);
        var matrixSize = squareRootOfMatrixSize - 1;
        return matrixSize % 10 == 0 ? matrixSize * 2 / 10 : (matrixSize - 1) * 2 / 10;
    }

    //public static float GetMaxLocalDistance(int mapScale) => 
    //    mapScale % 2 == 0 ?
    //    (mapScale * 5) - 1.3f :
    //    (mapScale * 5) - 0.3f;

    public static float GetMaxLocalDistance(int mapScale)
    {
        return mapScale % 2 == 0 ? mapScale * 5 - 0f : mapScale * 5 + 1f;
    }
    
    
    public static float MaxVectorDistanceToExit(int mapScale)
    {
        return mapScale % 2 == 0 ? mapScale * 10 - 1f : mapScale * 10 + 1f;
    }

    public static float NormalisedFloat(float min, float max, float current)
    {
        return (current - min) / (max - min);
    }

    /// <summary>
    ///     Returning 0 on current == 0 needed for trail memory to indicate a lack of observation to agent
    /// </summary>
    public static float NormalisedMemoryFloat(float min, float max, float current)
    {
        return current == 0 ? 0f : (current - min) / (max - min);
    }

    public static int MinusAddAngle(int input, AddSub addSub, int quantity)
    {
        if (addSub == AddSub.Add)
        {
            for (var i = 0; i < quantity; i++)
            {
                input++;
                if (input > 359) input = 0;
            }

            return input;
        }

        if (addSub == AddSub.Sub)
        {
            for (var i = 0; i < quantity; i++)
            {
                input--;
                if (input < 0) input = 359;
            }

            return input;
        }

        throw new InvalidEnumArgumentException("Wrong enum given to MinusAddAngle");
    }
    
    
    
    
    
}