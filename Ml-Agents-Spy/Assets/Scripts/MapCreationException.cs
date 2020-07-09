using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreationException : Exception
{
    public MapCreationException(string message, int matrixSize, int difficulty, 
        int exitCount, int guardAgentCount, int originalExit, int originalGuard) : base(message)
    {
        Debug.Log($"{message} \n" +
                  $"Matrix Size: {matrixSize}, \n" +
                  $"Difficulty: {difficulty}, \n" +
                  $"Exit count: {exitCount}, changed from {originalExit}, \n" +
                  $"Number of Guards: {guardAgentCount}, changed from {originalGuard}" );

    }
}
