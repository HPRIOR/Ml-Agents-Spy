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
            GameObject surrogate = Object.Instantiate(CoroutinePrefab);
            
            List<IEnvTile> tiles = new List<IEnvTile>()
            {
                new EnvTile(Vector3.zero, (0,0)),
                new EnvTile(new Vector3(2,0,0), (1, 0)),
                new EnvTile(new Vector3(4,0,0), (2, 0))
            };
            
            // spawn agent on first tile
            GameObject agent = new GameObject();
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
            Assert.That(nearestTile.Coords, Is.EqualTo((1, 0)));
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            yield return null;
            var agentTransform = agent.transform;
            
            // move one tile along
            agentTransform.position = patrolGuardTileManager.GuardTiles[1].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            yield return null;
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            Assert.AreEqual((2, 0), nearestTile.Coords);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // move to final tile
            agentTransform.position = patrolGuardTileManager.GuardTiles[2].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            yield return null;
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            Assert.AreEqual(null, nearestTile);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // wait for 5 seconds to wait for timout period - check nearest tile is still adjacent
            yield return new WaitForSeconds(5.0f);
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agentTransform);
            Assert.AreEqual((1, 0), nearestTile.Coords);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);


            agentTransform.position = patrolGuardTileManager.GuardTiles[1].Position;
            patrolGuardTileManager.CanRewardAgent(agentTransform);
            nearestTile = patrolGuardTileManager.GetNearestPatrolTile(agent.transform);
            
            Assert.That(nearestTile.Coords, Is.EqualTo((0, 0)));
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            yield return null;




        }

        

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator Test_Tiles_Change_When_Visited_And_Reset_After_Timeout()
        {
            var (patrolGuardTileManager, agent) = Setup();
            yield return true;
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            yield return null;
            
            // move agent to next tile
            agent.transform.position = patrolGuardTileManager.GuardTiles[1].Position;
            patrolGuardTileManager.CanRewardAgent(agent.transform);

            var agentTransform = agent.transform;
            yield return null;
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // agent hasn't moved but neither tile has timed 
            yield return new WaitForSeconds(2f);
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // agent hasn't moved but both visited tiles have timed out 
            yield return new WaitForSeconds(3f);
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // move back to original tile
            yield return null;
            agentTransform.position = patrolGuardTileManager.GuardTiles[0].Position;
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            yield return new WaitForSeconds(1f);
            agentTransform.position = patrolGuardTileManager.GuardTiles[1].Position;
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // move through all tiles
            yield return new WaitForSeconds(1f);
            agentTransform.position = patrolGuardTileManager.GuardTiles[2].Position;
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // wait for timeout of first tile
            yield return new WaitForSeconds(3f);
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // wait for timeout of second tile
            yield return new WaitForSeconds(1f);
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // wait for timeout of second tile
            yield return new WaitForSeconds(1f);
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // move threshold, should observe no change 
            yield return null;
            agentTransform.position -=  new Vector3(0.9f, 0, 0 );
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
            
            // move to threshold should observe change
            yield return null;
            agentTransform.position -= new Vector3(0.1f, 0, 0 );
            patrolGuardTileManager.CanRewardAgent(agent.transform);
            
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[0].RecentlyVisitedByGuard);
            Assert.AreEqual(true, patrolGuardTileManager.GuardTiles[1].RecentlyVisitedByGuard);
            Assert.AreEqual(false, patrolGuardTileManager.GuardTiles[2].RecentlyVisitedByGuard);
        }
    }
}