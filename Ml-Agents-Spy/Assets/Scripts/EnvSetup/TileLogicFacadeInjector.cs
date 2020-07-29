using System.ComponentModel;
using Enums;
using Interfaces;

namespace EnvSetup
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
