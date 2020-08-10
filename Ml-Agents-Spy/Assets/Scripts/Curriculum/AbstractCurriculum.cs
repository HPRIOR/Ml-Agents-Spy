using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using UnityEngine;

namespace Curriculum
{
    public abstract class AbstractCurriculum : ITileLogicFacade
    {
        protected int _exitCount;
        protected int _guardAgentCount;
        protected bool _hasMiddleTiles;
        protected int _mapDiff;
        protected int _mapScale;

        public ITileLogicBuilder GetTileLogicBuilder(float curriculumParam,
            Dictionary<ParentObject, GameObject> parentObjects)
        {
            return new TileLogicBuilder(
                _mapScale,
                _mapDiff,
                _exitCount,
                _guardAgentCount,
                parentObjects,
                hasMiddleTiles: _hasMiddleTiles
            );
        }

        public Dictionary<GameParam, int> EnvParams =>
            new Dictionary<GameParam, int>
            {
                {GameParam.ExitCount, _exitCount},
                {GameParam.MapDifficulty, _mapDiff},
                {GameParam.MapScale, _mapScale},
                {GameParam.GuardAgentCount, _guardAgentCount}
            };

        protected abstract void InterpretCurriculum(float curriculum);
    }
}