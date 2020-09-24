using System.Collections.Generic;
using Enums;
using EnvSetup;
using Interfaces;
using UnityEngine;

namespace Curriculum
{
    /// <summary>
    /// Protected fields are defined in subclasses. These are then used to retrieve the Builder class, and game param
    /// dictionary
    /// </summary>
    public abstract class AbstractCurriculum : ITileLogicFacade
    {
        protected int _exitCount;
        protected int _guardAgentCount;
        protected bool _hasMiddleTiles;
        protected int _mapDiff;
        protected int _mapScale;

        /// <summary>
        /// Returns a builder class which instantiates the various environment producing classes
        /// with the various arguments 
        /// </summary>
        /// <param name="curriculumParam"></param>
        /// <param name="parentObjects"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a dictionary of game parameters, these are defined as public fields after InterpretCurriculum has
        /// been called in the subclasses
        /// </summary>
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