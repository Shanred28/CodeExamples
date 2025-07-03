using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public interface ICharacterControllerJumpService
    {
        void Jump(bool isGrounded,ref Vector3 velocity);
    }
}