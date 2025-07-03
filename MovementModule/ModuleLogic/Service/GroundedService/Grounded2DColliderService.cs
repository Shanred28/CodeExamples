using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class Grounded2DColliderService : IIsGroundedService
    {
        private Collider2D _groundColliderCheck;
        private readonly LayerMask _groundLayer;

        public Grounded2DColliderService(LayerMask groundLayer)
        {
            _groundLayer = groundLayer;
        }

        public void SetGroundColliderCheck(Collider2D groundCollider)
        {
            _groundColliderCheck = groundCollider;
        }

        public void SetGroundCheckPoint(Transform groundCheckPoint)
        {
            Debug.Log("Service is not configured in SO.");
        }

        public bool IsGrounded()
        {
            return _groundColliderCheck != null && 
                   _groundColliderCheck.IsTouchingLayers(_groundLayer);
        }

        public void Configure(ScriptableObject scriptableObject)
        {
            
        }
    }
}
