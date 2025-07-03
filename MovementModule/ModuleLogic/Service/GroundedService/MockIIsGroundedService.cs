using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class MockIIsGroundedService : IIsGroundedService
    {
        public void SetGroundCheckPoint(Transform groundCheckPoint)
        {
            Debug.Log("Service is not configured in SO.");
        }

        public bool IsGrounded()
        {
            Debug.Log("Service is not configured in SO.");
            return true;
        }
    }
}
