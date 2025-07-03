using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class Grounded3DPointService : IIsGroundedService
    {
        private Transform _groundCheckPoint;
        private readonly float _checkRadius;
        private readonly LayerMask _groundLayer;

        public Grounded3DPointService(float checkRadius, LayerMask groundLayer)
        {
            _checkRadius = checkRadius;
            _groundLayer = groundLayer;
        }
    
        public void SetGroundCheckPoint(Transform groundCheckPoint)
        {
            _groundCheckPoint = groundCheckPoint;
        }
    
        public bool IsGrounded()
        {
            return _groundCheckPoint != null && 
                   Physics.OverlapSphere(_groundCheckPoint.position, _checkRadius, _groundLayer).Length > 0;
        }
    }
}
