using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class Grounded2DPointService : IIsGroundedService
    {
        private Transform _groundCheckPoint;
        private  float _checkRadius;
        private  LayerMask _groundLayer;

        public Grounded2DPointService(float checkRadius, LayerMask groundLayer)
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
                   Physics2D.OverlapCircle(_groundCheckPoint.position, _checkRadius, _groundLayer) != null;
        }

        
    }
}
