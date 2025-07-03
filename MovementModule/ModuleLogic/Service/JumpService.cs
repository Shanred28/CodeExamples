using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public class JumpService : IJumpService
    {
        private readonly float _jumpForce;
        private readonly float _gravity; 
        private float _verticalVelocity;   

        
        private bool _isJumping; 
        public bool IsJumping => _isJumping;
        
        public JumpService(float jumpForce, float gravity)
        {
            _jumpForce = jumpForce;
            _gravity = gravity;
        }
        
        public void StartJump()
        {
            if (!_isJumping)
            {
                _isJumping = true;
                _verticalVelocity = _jumpForce;
            }
        }
        
        public Vector3 Jump()
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
            Vector3 offset = Vector3.up * _verticalVelocity * Time.deltaTime;

            return offset;
        }
        
        public void EndJump()
        {
            _isJumping = false;
            _verticalVelocity = 0;
        }
    }
}
