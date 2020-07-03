using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHelper 
{
    private static System.Random _random = new System.Random();

    /// <summary>
    /// Generates list of random unique sequence of numbers.
    /// </summary>
    /// <remarks>
    /// The exitCount will default to the max value if the exitCount is larger than the max value.
    /// Otherwise there will be no values left to populate the list and it will loop forever
    /// </remarks>
    /// <param name="count">the length of the returned List</param>
    /// <param name="maxVal">highest value which can be generated</param>
    /// <returns>List of random unique sequence of number</returns>
    public static List<int> GetUniqueRandomList(int count, int maxVal)
    {
        int checkCount = count > maxVal ? maxVal : count;
        List<int> uniqueInts = new List<int>();
        int counting = 0;
        while (counting < checkCount)
        {
            int num = _random.Next(0, maxVal);
            if (!uniqueInts.Contains(num))
            {
                uniqueInts.Add(num);
                counting += 1;
            }
        }

        return uniqueInts;
    }
}
