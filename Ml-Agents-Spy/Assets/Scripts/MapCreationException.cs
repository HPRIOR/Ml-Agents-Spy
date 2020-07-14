using System;
using UnityEngine;

public class MapCreationException : Exception
{
    public MapCreationException(string message, int matrixSize =0, int difficulty=0, 
        int exitCount=0, int guardAgentCount=0, int originalExit=0, int originalGuard=0) : base(message)
    {
        Debug.Log($"{message} \n" +
                  $"Matrix Size: {matrixSize}, \n" +
                  $"Difficulty: {difficulty}, \n" +
                  $"Exit count: {exitCount}, changed from {originalExit}, \n" +
                  $"Number of Guards: {guardAgentCount}, changed from {originalGuard}" );

    }
}
