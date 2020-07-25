using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnvSetupFacadeInjector 
{
    public IEnvSetupFacade GetEnvSetupFacade(string facadeName)
    {
        switch (facadeName)
        {
            case "SimplePathFinding":
                return new SimplePathFindingCurriculum();
            case "AdvancedPathFinding":
                return new AdvancedPathFindingCurriculum();
        }
        throw new InvalidEnumArgumentException("Wrong string entered to EnvSetupFacadeInjector.GetEnvSetupFacade(string facadeName)");
    }
}
