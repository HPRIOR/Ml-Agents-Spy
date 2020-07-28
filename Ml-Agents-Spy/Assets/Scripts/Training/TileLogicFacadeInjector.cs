using System.ComponentModel;
using Enums;
using EnvSetup;
using Interfaces;

namespace Training
{
    public class TileLogicFacadeInjector 
    {
        public ITileLogicFacade GetTileLogicFacade(Curriculum facadeName)
        {
            switch (facadeName)
            {
                case Curriculum.SimplePathFinding:
                    return new SimplePathFindingCurriculum();
                case Curriculum.AdvancedPathFinding:
                    return new AdvancedPathFindingCurriculum();
            }
            throw new InvalidEnumArgumentException("Invalid Enum passed to Facade");
        }
    }
}
