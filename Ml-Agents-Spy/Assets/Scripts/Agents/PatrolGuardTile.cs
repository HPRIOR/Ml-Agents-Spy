﻿using System.Collections;
using EnvSetup;
using Interfaces;
using Unity.MLAgents;
using UnityEngine;

namespace Agents
{
    public class PatrolGuardTile : IPatrolGuardTile
    {
        private GameObject _surrogateGameObject;

        private bool _recentlyVisitedByGuard;
        public bool RecentlyVisitedByGuard
        {
            get => _recentlyVisitedByGuard;
            set
            {
                _recentlyVisitedByGuard = value;
                var mono = _surrogateGameObject.GetComponent<SurrogateMono>();
                mono.StartCoroutine(WaitFor(5));
                _recentlyVisitedByGuard = false;
            }
        }

        public PatrolGuardTile(GameObject surrogateGameObject, Vector3 tilePosition, (int x, int y) coords)
        {
            _surrogateGameObject = surrogateGameObject;
            Position = tilePosition;
            Coords = coords;
        }
        
        private static IEnumerator WaitFor(int seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public Vector3 Position { get; }
        public (int x, int y) Coords { get; }
    }
}