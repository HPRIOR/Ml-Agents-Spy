using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Training
{
    public class AgentSpawner : IAgentSpawner
    {
        private TrainingScenario _trainingScenario;
        private GameObject _spyPrefab;
        private GameObject _spyPrefabClone;
        private GameObject _guardPatrolPrefab;
        private GameObject _guardAlertPrefab;
        private List<GameObject> _guardPrefabClones;
        private Transform _instanceControllerTransform;
        private Dictionary<TileType, List<IEnvTile>> _tileDict;
        private Dictionary<GameParam, int> _gameParams;
        private bool _isInitialSetup;
        
        
        public AgentSpawner(TrainingScenario trainingScenario, GameObject spyPrefab,
            GameObject guardPatrolPrefab, GameObject guardAlertPrefab, Transform instanceControllerTransform,
            Dictionary<TileType, List<IEnvTile>> tileDict, Dictionary<GameParam, int> gameParams, bool isInitialSetup,
            List<GameObject> guardPrefabClones)
        {
            _trainingScenario = trainingScenario;
            _spyPrefab = spyPrefab;
            _guardPatrolPrefab = guardPatrolPrefab;
            _guardAlertPrefab = guardAlertPrefab;
            _instanceControllerTransform = instanceControllerTransform;
            _tileDict = tileDict;
            _gameParams = gameParams;
            _isInitialSetup = isInitialSetup;
            _guardPrefabClones = guardPrefabClones;
        }
        
        public void SpawnAgents()
        {
            if (_isInitialSetup)
            {
                if (SpyCanSpawn(_trainingScenario)) SpawnOrMoveSpy();
                SpawnGuards(_gameParams[GameParam.GuardAgentCount]);
                MoveGuards(_tileDict[TileType.GuardTiles], _gameParams);
                _isInitialSetup = false;
            }
            else
            {
                if (SpyCanSpawn(_trainingScenario)) SpawnOrMoveSpy();
                MoveGuards(_tileDict[TileType.GuardTiles], _gameParams);
            }



        }
        

        // will need to pass the _prefabClone and prefab into here
        private void SpawnOrMoveSpy()
        {
            if (_spyPrefabClone is null)
            {
                _spyPrefabClone = GameObject.Instantiate(_spyPrefab, _tileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                _spyPrefabClone.transform.parent = _instanceControllerTransform;
            }
            else
            {
                _spyPrefabClone.transform.position = _tileDict[TileType.SpyTile][0].Position;
            }
        }
        
        
        
        private void SpawnGuards(int numberOfGuards)
        {
            for (int i = 0; i < numberOfGuards; i++)
            {
                if (TrainingScenarioWantsPatrol(_trainingScenario))
                    _guardPrefabClones.Add(GameObject.Instantiate(_guardPatrolPrefab, _tileDict[TileType.GuardTiles][0].Position, Quaternion.identity));
                if (TrainingScenarioWantsAlert(_trainingScenario))
                    _guardPrefabClones.Add(GameObject.Instantiate(_guardAlertPrefab, _tileDict[TileType.GuardTiles][0].Position, Quaternion.identity));

                   
            }
            _guardPrefabClones.ForEach(guard => guard.transform.parent = _instanceControllerTransform);
        }
        
        private void MoveGuards(List<IEnvTile> potentialSpawnTiles, Dictionary<GameParam, int> gameParams)
        {
            if (_guardPrefabClones.Count <= 0) return;
            var indexes =
                RandomHelper.GetUniqueRandomList(gameParams[GameParam.GuardAgentCount], potentialSpawnTiles.Count);
            for (int i = 0; i < _guardPrefabClones.Count; i++)
            {
                _guardPrefabClones[i].transform.position = potentialSpawnTiles[indexes[i]].Position;
            }

        }

        private static bool SpyCanSpawn(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.SpyEvade
            || trainingScenario == TrainingScenario.GuardAlert
            || trainingScenario == TrainingScenario.SpyPathFinding
            || trainingScenario == TrainingScenario.GuardPatrolWithSpy;

        private static bool TrainingScenarioWantsPatrol(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.GuardPatrolWithSpy
            || trainingScenario == TrainingScenario.GuardPatrol
            || trainingScenario == TrainingScenario.SpyEvade;

        private static bool TrainingScenarioWantsAlert(TrainingScenario trainingScenario) =>
            trainingScenario == TrainingScenario.GuardAlert;

    }
}