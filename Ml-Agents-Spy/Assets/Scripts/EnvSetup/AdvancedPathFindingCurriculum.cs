using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace EnvSetup
{
    public class AdvancedPathFindingCurriculum : ITileLogicFacade
    {
        public Dictionary<GameParam, int> EnvParamDict { get; } = new Dictionary<GameParam, int>();
        public ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects)
        {
            int mapScale;
            int mapDiff;
            int exitCount;
            int guardAgentCount;
            int curriculumParamInt = (int)curriculumParam;
            switch (curriculumParamInt)
            {
                case 1:
                    mapScale = 2;
                    mapDiff = 20;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );

                case 2:
                    mapScale = 3;
                    mapDiff = 30;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 3:
                    mapScale = 3;
                    mapDiff = 50;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 4:
                    mapScale = 3;
                    mapDiff = 60;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 5:
                    mapScale = 4;
                    mapDiff = 60;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 6:
                    mapScale = 4;
                    mapDiff = 80;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 7:
                    mapScale = 4;
                    mapDiff = 100;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 8:
                    mapScale = 5;
                    mapDiff = 100;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 9:
                    mapScale = 5;
                    mapDiff = 150;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
                case 10:
                    mapScale = 6;
                    mapDiff = 200;
                    exitCount = 2;
                    guardAgentCount = 1;
                    EnvParamDict[GameParam.ExitCount] = exitCount;
                    EnvParamDict[GameParam.MapDifficulty] = mapDiff;
                    EnvParamDict[GameParam.MapScale] = mapScale;
                    EnvParamDict[GameParam.GuardAgentCount] = guardAgentCount;
                    return new TileLogicBuilder(
                        mapScale: mapScale,
                        mapDifficulty: mapDiff,
                        exitCount: exitCount,
                        guardAgentCount: guardAgentCount,
                        parentDictionary: parentObjects,
                        hasMiddleTiles: true
                    );
            }

            throw new InvalidOperationException("Wrong value given to EnvSetupFacade from curriculum.");
        }
    }
}
