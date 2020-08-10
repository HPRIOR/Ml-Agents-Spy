using System.Collections.Generic;
using UnityEngine;

namespace Agents
{
    // this may be incorporated into abstract agent because the agent will need to use this 
    public abstract class AbstractGuard : AbstractAgent
    {
        public bool CanMove { get; set; } = true;

        /// <summary>
        ///     Gets the nearest guards to the current guard agent
        /// </summary>
        /// <remarks>
        ///     This override allows for guard specific logic to be used in getting guards. Namely, to prevent this guard
        ///     from appearing in the list.
        /// </remarks>
        /// <param name="amount">Number of agents to return</param>
        /// <returns>List of agents (GameObjects)</returns>
        protected override List<GameObject> GetNearestGuards(int amount)
        {
            return transform
                .gameObject
                .GetNearest(
                    amount,
                    InstanceController.Guards,
                    guard =>
                        transform.gameObject.GetInstanceID() != guard.gameObjectDistance.GetInstanceID());
        }
    }
}