using System.Collections;
using UnityEngine.TestTools;

namespace Tests.AgentTests
{
    public class AlertGuardTests
    {
        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator AlertGuardTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
        
        
        // Spy local position
    }
}