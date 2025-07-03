using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public interface IIsGroundedService
    {
        public void SetGroundCheckPoint(Transform groundCheckPoint);
        bool IsGrounded();
    }
}
