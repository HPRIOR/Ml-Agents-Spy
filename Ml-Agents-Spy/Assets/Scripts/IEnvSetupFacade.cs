using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvSetupFacade
{
    IEnvSetup GetEnvSetup(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects);
}
