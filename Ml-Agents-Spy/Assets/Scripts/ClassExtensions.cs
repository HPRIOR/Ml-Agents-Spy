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
    
}
