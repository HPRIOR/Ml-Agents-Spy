namespace Curriculum
{
    public class AdvancedRandomCurriculum : AbstractCurriculum
    {
        public AdvancedRandomCurriculum(float curriculum)
        {
            InterpretCurriculum(curriculum);
        }

        protected sealed override void InterpretCurriculum(float curriculum)
        {
            switch ((int) curriculum)
            {
                case 1:
                    _mapScale = 2;
                    _mapDiff = 15;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 2:
                    _mapScale = 2;
                    _mapDiff = 20;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 3:
                    _mapScale = 3;
                    _mapDiff = 30;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 4:
                    _mapScale = 3;
                    _mapDiff = 35;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 5:
                    _mapScale = 4;
                    _mapDiff = 35;
                    _exitCount = 3;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 6:
                    _mapScale = 4;
                    _mapDiff = 40;
                    _exitCount = 3;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
            }
        }
    }
}