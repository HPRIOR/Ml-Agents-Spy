using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvSetup : IEnvSetup
{
    private readonly int _mapSize;
    private readonly int _mapComplexity;
    private readonly GameObject _topLevelParent;

    public EnvSetup(int mapSize, int mapComplexity, GameObject topLevelParent)
    {
        _mapSize = mapSize;
        _mapComplexity = mapComplexity;
        _topLevelParent = topLevelParent;
    }

    public void CreateEnv()
    {
        GameObject plane = CreatePlane(
            position: _topLevelParent.transform.localPosition,
            scale: new Vector3(_mapSize, 1, _mapSize),
            parent: _topLevelParent.transform
            );
        
    }

    private int GridMapSize => 
        _mapSize % 2 == 0 ? (_mapSize * 10) / 2 : ((_mapSize * 10) / 2) + 1;



    private GameObject CreatePlane(Vector3 position, Vector3 scale, Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = position;
        plane.transform.localScale = scale;
        plane.transform.parent = parent;
        return plane;
    }

}
