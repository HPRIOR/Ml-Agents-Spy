using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface ITileLogicFacade
    {
        Dictionary<GameParam, int> EnvParams { get; }

        ITileLogicBuilder GetTileLogicBuilder(float curriculumParam,
            Dictionary<ParentObject, GameObject> parentObjects);
    }
}