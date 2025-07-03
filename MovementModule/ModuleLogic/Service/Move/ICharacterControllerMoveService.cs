using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public interface ICharacterControllerMoveService
    {
        public void SetCharacterController(CharacterController controller);
        void Move(Vector3 inputDirection, ref Vector3 currentVelocity);
    }
}