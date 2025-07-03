using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class Grounded3DTriggerService : IIsGroundedService
    {
        private bool _isGroundTrigger;

        public void SetGroundTrigger(bool isGroundTrigger)
        {
            _isGroundTrigger = isGroundTrigger;
        }

        public void SetGroundCheckPoint(Transform groundCheckPoint)
        {
            Debug.Log("Service is not configured in SO.");
        }

        public bool IsGrounded()
        {
            return _isGroundTrigger;
        }

        public void Configure(ScriptableObject scriptableObject)
        {
            throw new System.NotImplementedException();
        }
    }
}
