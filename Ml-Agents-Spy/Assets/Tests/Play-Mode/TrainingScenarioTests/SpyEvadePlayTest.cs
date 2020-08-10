using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.TrainingScenarioTests
{
    public class SpyEvadePlayTest
    {
        //TODO 
        [Test]
        public void SpyEvadePlayTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator SpyEvadePlayTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}