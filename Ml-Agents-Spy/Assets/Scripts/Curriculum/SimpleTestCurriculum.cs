using System.ComponentModel;

namespace Curriculum
{
    public class SimpleTestCurriculum : AbstractCurriculum
    {
        public SimpleTestCurriculum(float curriculum)
        {
            InterpretCurriculum(curriculum);
        }
        
        protected sealed override void InterpretCurriculum(float curriculum)
        {
            switch ((int)curriculum)
            {
                case 1:
                    _mapScale = 1;
                    _mapDiff = 0;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                default:
                    _mapScale = 1;
                    _mapDiff = 0;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                    
            }
            
        }
    }
}