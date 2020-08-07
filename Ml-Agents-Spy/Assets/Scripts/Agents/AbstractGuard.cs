using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    // this may be incorped into abstract agent because the agent will need to use this 
    public abstract class AbstractGuard : AbstractAgent
    {
        public bool CanMove { get; set; } = true;

    }
}