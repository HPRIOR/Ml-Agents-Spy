using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileLogicFacade
{
    ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects);
}
