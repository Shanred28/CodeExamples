using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MoveService3DfpsCharacterController : ICharacterControllerMoveService
    {
        private CharacterController _controller;
        private readonly float _speed;
        private readonly float _gravity;
        
        public MoveService3DfpsCharacterController(float speed, float gravity)
        {
            _speed = speed;
            _gravity = gravity;
        }

        public void SetCharacterController(CharacterController controller)
        {
            _controller = controller;
        }

        public void Move(Vector3 inputDirection,ref Vector3 currentVelocity)
        {
            Vector3 move = _controller.transform.TransformDirection(inputDirection) * _speed;
            
            if (_controller.isGrounded && currentVelocity.y < 0)
            {
                currentVelocity.y = 0f;
            }
            
            currentVelocity.y += _gravity * Time.deltaTime;
            
            Vector3 finalMove = (move + currentVelocity) * Time.deltaTime;
            _controller.Move(finalMove);
        }
    }
}
