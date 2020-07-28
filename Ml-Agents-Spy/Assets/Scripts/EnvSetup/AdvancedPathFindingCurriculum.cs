using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnvSetup
{
    public class AdvancedPathFindingCurriculum : ITileLogicFacade
    {
        public ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects)
        {
            int curriculumParamInt = (int)curriculumParam;
            switch (curriculumParamInt)
            {
                case 1:
                    return new TileLogicBuilder(
                        mapScale: 2,
                        mapDifficulty: 20,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );

                case 2:
                    return new TileLogicBuilder(
                        mapScale: 3,
                        mapDifficulty: 30,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 3:
                    return new TileLogicBuilder(
                        mapScale: 3,
                        mapDifficulty: 50,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 4:
                    return new TileLogicBuilder(
                        mapScale: 3,
                        mapDifficulty: 60,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 5:
                    return new TileLogicBuilder(
                        mapScale: 4,
                        mapDifficulty: 60,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 6:
                    return new TileLogicBuilder(
                        mapScale: 4,
                        mapDifficulty: 80,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 7:
                    return new TileLogicBuilder(
                        mapScale: 4,
                        mapDifficulty: 100,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 8:
                    return new TileLogicBuilder(
                        mapScale: 5,
                        mapDifficulty: 100,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 9:
                    return new TileLogicBuilder(
                        mapScale: 5,
                        mapDifficulty: 150,
                        exitCount: 2,
                        guardAgentCount: 1,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 10:
                    return new TileLogicBuilder(
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
}
