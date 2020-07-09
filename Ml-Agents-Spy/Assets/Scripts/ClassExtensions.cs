using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ClassExtensions 
{
    public static IList<T> CloneList<T>(this IList<T> cloningList) where T : ICloneable
    {
        return cloningList.Select(x => (T) x.Clone()).ToList();
    }

    public static List<List<T>> CopyMatrix<T>(this List<List<T>> matrixT) where T : ICloneable
    {
        List<List<T>> copy = new List<List<T>>();
        foreach (var colT in matrixT)
        {
            var rowList = new List<T>();
            foreach (var rowT in colT)
            {
                rowList.Add((T)rowT.Clone());
            }
            copy.Add(rowList);
        }
        return copy;
    }

}
