using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPathFindingCurriculum : IEnvSetupFacade
{
    public IEnvSetup GetEnvSetup(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects)
    {
        int curriculumParamInt = (int)curriculumParam;
        switch (curriculumParamInt)
        {
            case 1:
                return new EnvSetup(
                    mapScale: 2,
                    mapDifficulty: 20,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: false
                    );

            case 2:
                return new EnvSetup(
                    mapScale: 3,
                    mapDifficulty: 30,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 3:
                return new EnvSetup(
                    mapScale: 3,
                    mapDifficulty: 50,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 4:
                return new EnvSetup(
                    mapScale: 3,
                    mapDifficulty: 60,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 5:
                return new EnvSetup(
                    mapScale: 4,
                    mapDifficulty: 60,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 6:
                return new EnvSetup(
                    mapScale: 4,
                    mapDifficulty: 80,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 7:
                return new EnvSetup(
                    mapScale: 4,
                    mapDifficulty: 100,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 8:
                return new EnvSetup(
                    mapScale: 5,
                    mapDifficulty: 100,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 9:
                return new EnvSetup(
                    mapScale: 5,
                    mapDifficulty: 150,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
            case 10:
                return new EnvSetup(
                    mapScale: 6,
                    mapDifficulty: 200,
                    exitCount: 2,
                    guardAgentCount: 1,
                    parentDictionary: parentObjects,
                    hasMiddleTiles: true
                );
        }

        throw new InvalidOperationException("Wrong value given to EnvSetupFacade from curriculum.");
    }
}
