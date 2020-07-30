using System.Collections.Generic;
using Enums;

public static class RandomHelper 
{
    private static System.Random _random = new System.Random();

    /// <summary>
    /// Generates list of random unique sequence of numbers.
    /// </summary>
    /// <remarks>
    /// The ExitCount will default to the max value if the ExitCount is larger than the max value.
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
            if  (!uniqueInts.Contains(num))
            {
                uniqueInts.Add(num);
                counting += 1;
            }
        }

        return uniqueInts;
    }

    /// <summary>
    /// Generates random number within range with an even or odd parity
    /// </summary>
    /// <param name="min">inclusive minimum value</param>
    /// <param name="max">inclusive maximum value</param>
    /// <param name="parity">Even or Odd parity</param>
    /// <returns></returns>
    public static int GetParityRandom(int min, int max, ParityEnum parity)
    {
        if (parity == ParityEnum.Even)
        {
            int num = _random.Next(min, max);
            return num % 2 != 0 & num < max ? num + 1 : num % 2 != 0 & num > min ? num - 1 : num;
        }
        else
        {
            int num = _random.Next(min, max);
            return num % 2 == 0 & num < max ? num + 1 : num % 2 == 0 & num > min ? num - 1 : num;
        }
    }
}