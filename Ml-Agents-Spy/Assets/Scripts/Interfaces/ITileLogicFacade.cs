using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface ITileLogicFacade
    {
        ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects);
        Dictionary<GameParam, int> EnvParams { get; }
    }
}
