using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace EnvSetup
{
    public class SimplePathFindingCurriculum : ITileLogicFacade
    {
        public Dictionary<GameParam, int> EnvParamDict { get; } = new Dictionary<GameParam, int>();
        public ITileLogicBuilder GetTileLogicBuilder(float curriculumParam, Dictionary<ParentObject, GameObject> parentObjects)
        {
            int mapScale;
            int mapDiff;
            int exitCount;
            int guardAgentCount;
            int curriculumParamInt = (int) curriculumParam;
            switch (curriculumParamInt)
            {
                case 1:
                    mapScale = 1;
                    mapDiff = 0;
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
                        hasMiddleTiles: false
                    );
                case 2:
                    mapScale = 1;
                    mapDiff = 0;
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
                    mapScale = 1;
                    mapDiff = 1;
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
                    mapScale = 1;
                    mapDiff = 2;
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
                    mapScale = 1;
                    mapDiff = 3;
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
                    mapScale = 1;
                    mapDiff = 4;
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
                    mapScale = 1;
                    mapDiff = 5;
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
                    mapScale = 2;
                    mapDiff = 7;
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
                    mapScale = 2;
                    mapDiff = 13;
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
            }
            throw new InvalidOperationException("Wrong value given to EnvSetupFacade from curriculum.");
        }

        
    }
}
