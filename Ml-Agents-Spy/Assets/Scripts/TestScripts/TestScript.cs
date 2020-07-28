using System.Linq;
using UnityEngine;

namespace TestScripts
{
    public class TestScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MovementTracker movementTracker = new MovementTracker();
            float[] testFloats = new[] { 3f, 3f, 1f, 1f, 0f, 0f, 0f, 0f, 0f, 0f };

            movementTracker.GetAgentMemory(new Vector3(1, 1, 1));

            movementTracker.GetAgentMemory(new Vector3(2, 2, 2)).ToList().ForEach(f => Debug.Log(f));
        }

    
    }
}
