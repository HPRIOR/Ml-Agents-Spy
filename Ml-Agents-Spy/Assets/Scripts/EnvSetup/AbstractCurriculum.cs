using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace EnvSetup
{
    public abstract class AbstractCurriculum : ITileLogicFacade
    {
        protected int _exitCount;
        protected int _mapDiff;
        protected int _mapScale;
        protected int _guardAgentCount;
        protected bool _hasMiddleTiles;

        protected abstract void InterpretCurriculum(float curriculum);
        
        public ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects) => 
            new TileLogicBuilder(
                mapScale: _mapScale, 
                mapDifficulty: _mapDiff, 
                exitCount: _exitCount, 
                guardAgentCount: _guardAgentCount, 
                parentDictionary: parentObjects,
                hasMiddleTiles: _hasMiddleTiles
            );

        public Dictionary<GameParam, int> EnvParams =>
            new Dictionary<GameParam, int>()
            {
                {GameParam.ExitCount, _exitCount},
                {GameParam.MapDifficulty, _mapDiff},
                {GameParam.MapScale, _mapScale},
                {GameParam.GuardAgentCount, _guardAgentCount}
            };
    }
}