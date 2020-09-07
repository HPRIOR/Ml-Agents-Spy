namespace Curriculum
{
    public class SimplePatrolGuardPathFindingCurriculum : AbstractCurriculum
    {
        public SimplePatrolGuardPathFindingCurriculum(float curriculum)
        {
            InterpretCurriculum(curriculum);
        }

        protected sealed override void InterpretCurriculum(float curriculum)
        {
            switch ((int) curriculum)
            {
                case 1:
                    _mapScale = 5;
                    _mapDiff = 0;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = false;
                    break;
                case 2:
                    _mapScale = 5;
                    _mapDiff = 10;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 3:
                    _mapScale = 5;
                    _mapDiff = 15;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 4:
                    _mapScale = 5;
                    _mapDiff = 20;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 5:
                    _mapScale = 5;
                    _mapDiff = 25;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 6:
                    _mapScale = 5;
                    _mapDiff = 30;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 7:
                    _mapScale = 5;
                    _mapDiff = 35;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 8:
                    _mapScale = 5;
                    _mapDiff = 40;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 9:
                    _mapScale = 5;
                    _mapDiff = 45;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
                case 10:
                    _mapScale = 2;
                    _mapDiff = 50;
                    _exitCount = 2;
                    _guardAgentCount = 1;
                    _hasMiddleTiles = true;
                    break;
            }
        }
    }
}