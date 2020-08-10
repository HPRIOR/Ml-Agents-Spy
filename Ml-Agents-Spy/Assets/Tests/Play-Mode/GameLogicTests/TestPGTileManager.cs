using System.Collections;
using System.Collections.Generic;
using Agents;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using Tests.TestSetup;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.GameLogicTests
{
    public class TestPgTileManager : AbstractPlayModeTest
    {
        private static readonly GameObject CoroutinePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coroutine Surrogate.prefab");

        private static (IPatrolGuardTileManager, GameObject) Setup()
        {
            var surrogate = Object.Instantiate(CoroutinePrefab);

            var tiles = new List<IEnvTile>
            {
                new EnvTile(Vector3.zero, (0, 0)),
                new EnvTile(new Vector3(2, 0, 0), (1, 0)),
                new EnvTile(new Vector3(4, 0, 0), (2, 0)),
                new EnvTile(new Vector3(0, 0, 2), (0, 1)),
                new EnvTile(new Vector3(2, 0, 2), (1, 1)),
                new EnvTile(new Vector3(4, 0, 2), (2, 1))
            };

            // spawn agent on first tile
            var agent = new GameObject();
            agent.transform.position = tiles[0].Position;


            IPatrolGuardTileManager patrolGuardTileManager =
                new PatrolGuardTileManager(surrogate, tiles, agent.transform);
            return (patrolGuardTileManager, agent);
        }

        [UnityTest]
        public IEnumerator Test_Nearest_Tile()
        {
            var (patrolGuardTileManager, agent) = Setup();

            var nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agent.transform);
            nearestTile.ForEach(t =>
            {
                Assert.That(t.Coords, Is.EqualTo((1, 0)).Or.EqualTo((0, 1)).Or.EqualTo((1, 1)));
            });
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);
            yield return null;
            
            // move  to (1, 0)
            // move  to (1, 0)
            var agentTransform = agent.transform;
            agentTransform.position = patrolGuardTileManager.GuardTiles[1].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            yield return null;
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            nearestTile.ForEach(t =>
            {
                Assert.That(t.Coords, Is.EqualTo((2, 0)).Or.EqualTo((1, 1)).Or.EqualTo((0, 1)).Or.EqualTo((2,1)));
            });
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);

            // move to (2,0)
            agentTransform.position = patrolGuardTileManager.GuardTiles[2].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            yield return null;
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            
            nearestTile.ForEach(t =>
            {
                Assert.That(t.Coords, Is.EqualTo((2, 1)).Or.EqualTo((1, 1)).Or.EqualTo((0, 1)));
            });
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);

            // move to 1,1
            agentTransform.position = patrolGuardTileManager.GuardTiles[4].Position;
            
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            
            nearestTile.ForEach(t =>
            {
                if (!(t is null))
                {
                    Assert.That(t.Coords, Is.EqualTo((0, 1)).Or.EqualTo((2, 1)));
                }
                else Assert.Null(t);
            });
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);

            
            // move to (2,1)

            agentTransform.position = patrolGuardTileManager.GuardTiles[5].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agent.transform);

            nearestTile.ForEach(t =>
            {
                if (!(t is null))
                {
                    Assert.That(t.Coords, Is.EqualTo((0, 1)));
                }
                else Assert.Null(t);
            });
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);
            yield return null;
            
            // move to (0,1) - all tiles reset but the current one 

            agentTransform.position = patrolGuardTileManager.GuardTiles[3].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agent.transform);

            nearestTile.ForEach(t =>
            {
                Assert.That(t.Coords, Is.EqualTo((0, 0)).Or.EqualTo((1,0)).Or.EqualTo((1, 1)));
            });
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[3].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[4].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[5].RecentlyVisitedByGuard);
            yield return null;
            
            
            
        }


        
        
    }
}