using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class CharacterControllerJumpService : ICharacterControllerJumpService
    {
        private readonly float _jumpForce;

        public CharacterControllerJumpService(float jumpForce)
        {
            _jumpForce = jumpForce;
        }

        public void Jump(bool isGrounded,ref Vector3 velocity)
        {
            if(isGrounded)
                velocity.y = _jumpForce;
        }
    }
}