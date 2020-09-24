using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public int time = 1;
    void Update()
    {
        Time.timeScale = time;
    }

    public void ChangeSpeed(float speed)
    {
        time = (int) speed;
    }
}
