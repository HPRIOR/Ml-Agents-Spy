using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public int time = 20;
    void Update()
    {
        Time.timeScale = time;

    }
}
