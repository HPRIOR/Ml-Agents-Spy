using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

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
