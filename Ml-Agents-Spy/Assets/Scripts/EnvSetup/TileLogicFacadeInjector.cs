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
                case CurriculumEnum.SimplePatrolGuardPathFinding:
                    return new SimplePatrolGuardPathFindingCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedPatrolGuardPathFinding:
                    return new AdvancedPatrolGuardPathFindingCurriculum(curriculumParam);
                case CurriculumEnum.SimplePatrolGuardPathFindingWithSpy:
                    return new SimplePatrolGuardPathFindingWithSpyCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedPatrolGuardPathFindingWithSpy:
                    return new AdvancedPatrolGuardPathFindingWithSpyCurriculum(curriculumParam);
                case CurriculumEnum.SimpleGuardAlert:
                    return new SimpleGuardAlertCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedGuardAlert:
                    return new AdvancedGuardAlertCurriculum(curriculumParam);
                case CurriculumEnum.SimpleSpyEvade:
                    return new SimpleSpyEvadeCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedSpyEvade:
                    return new AdvancedSpyEvadeCurriculum(curriculumParam);
                case CurriculumEnum.AdvancedRandom:
                    return new AdvancedRandomCurriculum(curriculumParam);
            }

            throw new InvalidEnumArgumentException("Invalid Enum passed to Facade");
        }
    }
}