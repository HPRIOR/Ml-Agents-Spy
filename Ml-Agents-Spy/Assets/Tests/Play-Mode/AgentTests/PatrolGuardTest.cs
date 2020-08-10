using System.Collections;
using System.Collections.Generic;
using Tests.TestSetup;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AgentTests
{
    public class PatrolGuardTest : AbstractAgentTestSetup
    {
        // TODO
        [UnityTest]
        public IEnumerator Patrol_Guard_Test()
        {
           //var trainingInstanceController = GetDebugSetup(TrainingScenario.GuardPatrol);
           //SetDebugParameters(trainingInstanceController, 1, 0,
           //    2, 1, false);

           //yield return new WaitUntil(() => trainingInstanceController.TestSetUpComplete);

           //var positionsAroundCenter =
           //    GetPositionsAroundCenter(trainingInstanceController, 3, 1);

           //var freeTiles = trainingInstanceController
           //    .TileDict[TileType.FreeTiles]
           //    .Concat(trainingInstanceController.TileDict[TileType.GuardTiles])
           //    .Concat(trainingInstanceController.TileDict[TileType.SpyTile]);

           //var guardObject = trainingInstanceController.Guards[0];
           //var guardScript = guardObject.GetComponent<PatrolGuardAgent>();

           //guardScript.OnActionReceived(new float[]{1,1});
           yield return new WaitForSeconds(1);
           //
           //guardObject.transform.position
           //    = freeTiles.First(x => x.Coords == (3, 3)).Position;


           //var nearestPatrolTiles = CreateTupleList(guardScript.GetNearestPatrolTilePositions());
           //
           //nearestPatrolTiles.ForEach(tilePosition =>
           //{
           //    CollectionAssert.Contains(positionsAroundCenter, tilePosition);
           //});
           

        }

        List<(float, float)> CreateTupleList(List<float> floats)
        {
           
            
            List<(float,float)> tuples = new List<(float, float)>();

            for (int i = 0; i < floats.Count - 1; i++)
            {
                tuples.Add((floats[i],floats[i + 1]));
            }

            return tuples;
        }

    }
}