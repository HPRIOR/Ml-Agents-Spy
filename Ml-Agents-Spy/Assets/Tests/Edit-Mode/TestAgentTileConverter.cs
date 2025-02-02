﻿using System;
using System.Collections.Generic;
using System.Linq;
using Agents;
using Enums;
using EnvSetup;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestAgentTileConverter
    {
        private readonly Func<(ISpyTile, IEnvTile), bool> _guardPredicate = tileTuple => tileTuple.Item2.HasGuard;
        private readonly Func<(ISpyTile, IEnvTile), bool> _spyPredicate = tileTuple => tileTuple.Item2.HasSpy;

        private readonly ITileLogicBuilder tileLogicBuilder = new TileLogicBuilder(
            1,
            0,
            2,
            1,
            _dictionary,
            hasMiddleTiles: true
        );


        private static Dictionary<ParentObject, GameObject> _dictionary =>
            new Dictionary<ParentObject, GameObject>
            {
                {ParentObject.TopParent, new GameObject()},
                {ParentObject.EnvParent, new GameObject()},
                {ParentObject.SpyParent, new GameObject()},
                {ParentObject.GuardParent, new GameObject()},
                {ParentObject.DebugParent, new GameObject()}
            };


        private ITileLogicSetup TileLogic => tileLogicBuilder.GetTileLogicSetup();


        private IAgentTileConverter GetAgentTiles()
        {
            var env = TileLogic;
            env.GetTileLogic();
            var envDict = env.GetTileTypes();
            var envTileList = new List<IEnvTile>();
            envDict[TileType.FreeTiles].ForEach(tile => envTileList.Add(tile));
            envDict[TileType.SpyTile].ForEach(tile => envTileList.Add(tile));
            envDict[TileType.GuardTiles].ForEach(tile => envTileList.Add(tile));

            return new SpyAgentTileConverter(envTileList, _spyPredicate);
        }

        [Test]
        public void TestAgentTileCount()
        {
            var agentTileConv = GetAgentTiles();
            Assert.AreEqual(21, agentTileConv.GetAgentTiles().Count);
        }

        [Test]
        public void TestAdjacentTiles()
        {
            var agentTileConv = GetAgentTiles();

            var middleTile = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 3));
            var northFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 4));
            var southFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (3, 2));
            var eastFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (4, 3));
            var westFromMiddle = agentTileConv.GetAgentTiles().First(tile => tile.Coords == (2, 3));

            Assert.AreEqual(northFromMiddle.Coords, middleTile.AdjacentTile[Direction.N].Coords);
            Assert.AreEqual(eastFromMiddle.Coords, middleTile.AdjacentTile[Direction.E].Coords);
            Assert.AreEqual(southFromMiddle.Coords, middleTile.AdjacentTile[Direction.S].Coords);
            Assert.AreEqual(westFromMiddle.Coords, middleTile.AdjacentTile[Direction.W].Coords);
        }

        [Test]
        public void TestInitialSpyPlacement()
        {
            var env = TileLogic;
            env.GetTileLogic();
            var envDict = env.GetTileTypes();
            var envTiles = new List<IEnvTile>();
            envDict[TileType.FreeTiles].ForEach(tile => envTiles.Add(tile));
            envDict[TileType.SpyTile].ForEach(tile => envTiles.Add(tile));
            envDict[TileType.GuardTiles].ForEach(tile => envTiles.Add(tile));

            IAgentTileConverter converter = new SpyAgentTileConverter(envTiles, _spyPredicate);

            var agentTiles = converter.GetAgentTiles();

            Assert.AreEqual(
                agentTiles.First(tile => tile.OccupiedByAgent).Coords,
                envDict[TileType.SpyTile][0].Coords
            );
        }

        [Test]
        public void TestInitialGuardPlacement()
        {
            var env = TileLogic;
            env.GetTileLogic();
            var envDict = env.GetTileTypes();
            var envTiles = new List<IEnvTile>();
            envDict[TileType.FreeTiles].ForEach(tile => envTiles.Add(tile));
            envDict[TileType.SpyTile].ForEach(tile => envTiles.Add(tile));
            envDict[TileType.GuardTiles].ForEach(tile => envTiles.Add(tile));

            IAgentTileConverter converter = new SpyAgentTileConverter(envTiles, _guardPredicate);

            var agentTiles = converter.GetAgentTiles();

            var guardAgentTiles = agentTiles
                .Where(tile => tile.OccupiedByAgent)
                .OrderBy(tile => tile.Coords.y).ToList();

            var orderedGuardTiles = envDict[TileType.GuardTiles].OrderBy(tile => tile.Coords.y).ToList();

            Assert.AreEqual(guardAgentTiles[0].Coords, orderedGuardTiles[0].Coords);
        }

        [Test]
        public void TestInitialVisitCount()
        {
            var agentTileConv = GetAgentTiles();

            agentTileConv.GetAgentTiles().ForEach(tile => Assert.AreEqual(1, tile.VisitCount));
        }

        [Test]
        public void TestInitialVisitedByAlgo()
        {
            var agentTileConv = GetAgentTiles();

            agentTileConv.GetAgentTiles().ForEach(tile => Assert.AreEqual(false, tile.VisitedByAlgo));
        }
    }
}