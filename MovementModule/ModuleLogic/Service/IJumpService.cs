using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public interface IJumpService
    {
        public bool IsJumping { get; }
        public void StartJump();
        public Vector3 Jump();
        public void EndJump();
    }
}
