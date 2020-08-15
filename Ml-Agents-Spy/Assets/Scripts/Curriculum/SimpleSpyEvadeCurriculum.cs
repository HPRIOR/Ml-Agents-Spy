namespace Curriculum
{
    public class SimpleSpyEvadeCurriculum : AbstractCurriculum
    {
        
        
        public SimpleSpyEvadeCurriculum(float curriculum)
        {
            InterpretCurriculum(curriculum);
        }

        protected sealed override void InterpretCurriculum(float curriculum)
        {
            switch ((int) curriculum)
            {
                case 1:
                    _mapScale = 1;
                    _mapDiff = 0;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = false;
                    break;
                case 2:
                    _mapScale = 1;
                    _mapDiff = 0;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 3:
                    _mapScale = 1;
                    _mapDiff = 1;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 4:
                    _mapScale = 1;
                    _mapDiff = 2;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 5:
                    _mapScale = 1;
                    _mapDiff = 3;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 6:
                    _mapScale = 1;
                    _mapDiff = 4;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 7:
                    _mapScale = 1;
                    _mapDiff = 5;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 8:
                    _mapScale = 2;
                    _mapDiff = 7;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 9:
                    _mapScale = 2;
                    _mapDiff = 13;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 10:
                    _mapScale = 2;
                    _mapDiff = 20;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
            }
        }
    }
}