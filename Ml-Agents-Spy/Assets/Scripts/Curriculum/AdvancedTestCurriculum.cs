using System.ComponentModel;

namespace Curriculum
{
    namespace EnvSetup
    {
        public class AdvancedTestCurriculum : AbstractCurriculum
        {
            
            public AdvancedTestCurriculum(float curriculum)
            {
                InterpretCurriculum(curriculum);
            }
            protected sealed override void InterpretCurriculum(float curriculum)
            {
                switch ((int)curriculum)
                {
                    case 1:
                        _mapScale = 3;
                        _mapDiff = 10;
                        _exitCount = 3;
                        _guardAgentCount = 2;
                        _hasMiddleTiles = true;
                        break;
                    default:
                        _mapScale = 3;
                        _mapDiff = 10;
                        _exitCount = 3;
                        _guardAgentCount = 2;
                        _hasMiddleTiles = true;
                        break;
                }
            }
        }
    }
}