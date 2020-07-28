using System;
using System.Collections.Generic;
using System.Linq;
using EnvSetup;
using Interfaces;

namespace Agents
{
    public class AgentTileConverter : IAgentTileConverter
    {
        readonly List<IEnvTile> _envTiles;
        List<IAgentTile> GetInitialAgentTiles =>
            _envTiles
                .Select(tile => tile.Clone())
                .ToList()
                .ConvertAll(tile => (EnvTile)tile)
                .ConvertAll(tile => (AgentTile)tile)
                .ConvertAll(tile => (IAgentTile)tile);
        List<(IAgentTile, IEnvTile)> _correspondingTiles;
        private readonly Func<(IAgentTile, IEnvTile), bool> _envTileHasAttribute;

        public AgentTileConverter(List<IEnvTile> envTiles, Func<(IAgentTile, IEnvTile), bool> envTileHasAttribute)
        {
            _envTiles = envTiles;
            _envTileHasAttribute = envTileHasAttribute;
        }

    
        void GetAdjacentTiles(List<IAgentTile> agentTiles, List<IEnvTile> envTiles)
        {
            _correspondingTiles = agentTiles
                .OrderBy(tile => tile.Coords.x)
                .ThenBy(tile => tile.Coords.y)
                .Zip(
                    envTiles
                        .OrderBy(tile => tile.Coords.x)
                        .ThenBy(tile => tile.Coords.y),
                    (agentTile, envTile) => (agentTile, envTile)
                ).ToList();


            /*
         * Loops through the corresponding list of agent and env tiles.
         * For each of the directions in env tiles, the matching env tile is found in the corresponding list
         * when the match is found, the appropriate direction in the env tile adjacency dictionary is given the
         * corresponding agent tile
         */
            foreach (var (correspondingAgentTile, correspondingEnvTile) in _correspondingTiles)
            {
                //if (correspondingEnvTile.HasSpy) correspondingAgentTile.OccupiedByAgent = true;
                foreach (var keyValue in correspondingEnvTile.AdjacentTile)
                foreach (var (candidateAgentTile, candidateEnvTile) in _correspondingTiles)
                    if (keyValue.Value == candidateEnvTile)
                        correspondingAgentTile.AdjacentTile[keyValue.Key] = candidateAgentTile;
            }
        }

        void GetAgentLocations(Func<(IAgentTile, IEnvTile), bool> envTileHasAttribute)
        {
            if (_correspondingTiles is null) throw new NotImplementedException("Corresponding tuple list of Agent and Env tiles has not been created, run GetAdjecentTiles() first");
            _correspondingTiles.Where(envTileHasAttribute).ToList().ForEach(tileTuple => tileTuple.Item1.OccupiedByAgent = true);
        }

        public List<IAgentTile> GetAgentTiles()
        {
            var agentTiles = GetInitialAgentTiles;
            GetAdjacentTiles(agentTiles, _envTiles);
            GetAgentLocations(_envTileHasAttribute);
            return agentTiles;
        }




    }
}
