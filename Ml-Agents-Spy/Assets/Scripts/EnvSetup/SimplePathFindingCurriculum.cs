using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SimplePathFindingCurriculum : IEnvSetupFacade
{
    public IEnvSetup GetEnvSetup(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects)
    {
        int curriculumParamInt = (int) curriculumParam;
        switch (curriculumParamInt)
        {
            case 1:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 0,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: false
                    );
                
            case 2:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 0,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                ); 
            case 3:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 1,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 4:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 2,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 5:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 3,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                ); 
            case 6:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 4,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 7:
                return new EnvSetup(
                    mapScale: 1,
                    mapDifficulty: 5,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 8:
                return new EnvSetup(
                    mapScale: 2,
                    mapDifficulty: 7,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 9:
                return new EnvSetup(
                    mapScale: 2,
                    mapDifficulty: 13,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                ); 
            case 10:
                return new EnvSetup(
                    mapScale: 2,
                    mapDifficulty: 20,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 11:
                return new EnvSetup(
                    mapScale: 3,
                    mapDifficulty: 50,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
        }

        throw new InvalidOperationException("Wrong value given to EnvSetupFacade from curriculum.");
    }
}
