using System.ComponentModel;
using Enums;
using Interfaces;

namespace EnvSetup
{
    public class TileLogicFacadeInjector 
    {
        public ITileLogicFacade GetTileLogicFacade(Curriculum facadeName, float curriculumParam)
        {
            switch (facadeName)
            {
                case Curriculum.SimplePathFinding:
                    return new SimplePathFindingCurriculum(curriculumParam);
                case Curriculum.AdvancedPathFinding:
                    return new AdvancedPathFindingCurriculum(curriculumParam);
            }
            throw new InvalidEnumArgumentException("Invalid Enum passed to Facade");
        }
    }
}
