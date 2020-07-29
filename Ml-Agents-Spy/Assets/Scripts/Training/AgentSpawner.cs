using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Training
{
    public class AgentSpawner : IAgentSpawner
    {
        private GameObject _spyPrefab;
        private GameObject _spyPrefabClone;
        private Dictionary<TileType, List<IEnvTile>> _tileDict;
        private Dictionary<GameParam, int> _gameParams;
        private TrainingScenario _trainingScenario;
        private Transform _trainingInstanceTransform;
        
        public AgentSpawner(Dictionary<TileType, List<IEnvTile>> tileDict,Dictionary<GameParam, int> gameParams, TrainingScenario trainingScenario, GameObject[] prefabs, Transform trainingInstanceTransform)
        {
            _tileDict = tileDict;
            _gameParams = gameParams;
            _trainingScenario = trainingScenario;
            _spyPrefab = prefabs[0];
            _trainingInstanceTransform = trainingInstanceTransform;
        }
        
        public void SpawnAgents()
        {
            SpawnSpy();
            SpawnAlertGuard();
            SpawnPatrolGuard();
        }

        private void SpawnSpy()
        {
            if (_spyPrefabClone is null)
            {
                _spyPrefabClone = GameObject.Instantiate(_spyPrefab, _tileDict[TileType.SpyTile][0].Position, Quaternion.identity);
                _spyPrefabClone.transform.parent = _trainingInstanceTransform;
            }
            else
            {
                _spyPrefabClone.transform.position = _tileDict[TileType.SpyTile][0].Position;
            }
        }

        private void SpawnPatrolGuard()
        {
            
        }

        private void SpawnAlertGuard()
        {
            
        }
    }
}