using System.ComponentModel;
using Curriculum;
using Curriculum.EnvSetup;
using Enums;
using Interfaces;

namespace EnvSetup
{
    public class TileLogicFacadeInjector
    {
        public ITileLogicFacade GetTileLogicFacade(CurriculumEnum facadeName, float curriculumParam)
        {
            switch (facadeName)
            {
                case CurriculumEnum.SimplePathFinding:
                    return new SimplePathFindingCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedPathFinding:
                    return new AdvancedPathFindingCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedTestCurriculum:
                    return new AdvancedTestCurriculum(curriculumParam);
                case CurriculumEnum.SimpleTestCurriculum:
                    return new SimpleTestCurriculum(curriculumParam);
            }

            throw new InvalidEnumArgumentException("Invalid Enum passed to Facade");
        }
    }
}