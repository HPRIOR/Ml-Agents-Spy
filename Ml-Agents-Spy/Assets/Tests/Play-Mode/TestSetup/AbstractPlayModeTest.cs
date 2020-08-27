using NUnit.Framework;
using Unity.MLAgents;
using UnityEngine.SceneManagement;

namespace Tests.TestSetup
{
    public abstract class AbstractPlayModeTest
    {
        [SetUp]
        protected void Init()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
            //
        }

        [TearDown]
        protected void TearDown()
        {
            Academy.Instance.Dispose();
        }
    }
}