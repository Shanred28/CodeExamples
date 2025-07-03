using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public class MockIJumpService : IJumpService
    {
        public bool IsJumping { get; }

        public void StartJump() { }

        public Vector3 Jump() => Vector3.zero;

        public void EndJump() { }
    }
}
