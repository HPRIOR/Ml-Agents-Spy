using System.Collections.Generic;
using Enums;

public static class RandomHelper 
{
    private static System.Random _random = new System.Random();

    /// <summary>
    /// Generates list of random unique sequence of numbers.
    /// </summary>
    /// <remarks>
    /// The count will default to the max value if the count is larger than the max value.
    /// Otherwise there will be no values left to populate the list and it will loop forever
    /// </remarks>
    /// <param name="listLength">the length of the returned List</param>
    /// <param name="maximumValue">highest value which can be generated - exclusive</param>
    /// <returns>List of random unique sequence of number</returns>
    public static List<int> GetUniqueRandomList(int listLength, int maximumValue)
    {
        int maximumNumberOfElements = listLength > maximumValue ? maximumValue : listLength;

        List<int> uniqueInts = new List<int>();
        
        int numberOfAddedElements = 0;
        while (numberOfAddedElements < maximumNumberOfElements)
        {
            int num = _random.Next(0, maximumValue);
            if  (!uniqueInts.Contains(num))
            {
                uniqueInts.Add(num);
                numberOfAddedElements += 1;
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