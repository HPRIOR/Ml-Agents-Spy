using Agents;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestMovementTracker
    {
        // create default movement tracker (size 10, distance of 1)


        [Test]
        public void TestInitialFloatArray()
        {
            var movementTracker = new MovementTracker();
            var testFloats = new float[10];
            for (var i = 0; i < 2; i++) testFloats[i] = 1;
            Assert.AreEqual(testFloats, movementTracker.GetAgentMemory(new Vector3(1, 1, 1)));
        }

        [Test]
        public void TestTooSmallToChange()
        {
            var movementTracker = new MovementTracker();
            var testFloats = new float[10];
            for (var i = 0; i < 2; i++) testFloats[i] = 1;
            movementTracker.GetAgentMemory(new Vector3(1, 1, 1));
            Assert.AreEqual(testFloats, movementTracker.GetAgentMemory(new Vector3(1, 1, 0.5f)));
        }

        [Test]
        public void TestLargeEnoughToChange()
        {
            var movementTracker = new MovementTracker();
            float[] testFloats = {1f, 1f, 2f, 2f, 0f, 0f, 0f, 0f, 0f, 0f};

            movementTracker.GetAgentMemory(new Vector3(1, 1, 1));
            Assert.AreEqual(testFloats, movementTracker.GetAgentMemory(new Vector3(2, 1, 2)));
        }

        [Test]
        public void TestFullList()
        {
            var movementTracker = new MovementTracker();
            float[] testFloats = {1f, 1f, 2f, 2f, 3f, 3f, 4f, 4f, 5f, 5f};

            movementTracker.GetAgentMemory(new Vector3(1, 1, 1));
            movementTracker.GetAgentMemory(new Vector3(2, 1, 2));
            movementTracker.GetAgentMemory(new Vector3(3, 1, 3));
            movementTracker.GetAgentMemory(new Vector3(4, 1, 4));
            Assert.AreEqual(testFloats, movementTracker.GetAgentMemory(new Vector3(5, 1, 5)));
        }

        [Test]
        public void TestDequeue()
        {
            var movementTracker = new MovementTracker();
            float[] testFloats = {2f, 2f, 3f, 3f, 4f, 4f, 5f, 5f, 6f, 6f};

            movementTracker.GetAgentMemory(new Vector3(1, 1, 1));
            movementTracker.GetAgentMemory(new Vector3(2, 1, 2));
            movementTracker.GetAgentMemory(new Vector3(3, 1, 3));
            movementTracker.GetAgentMemory(new Vector3(4, 1, 4));
            movementTracker.GetAgentMemory(new Vector3(5, 1, 5));
            Assert.AreEqual(testFloats, movementTracker.GetAgentMemory(new Vector3(6, 1, 6)));
        }
    }
}