using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public interface IMoveService
    {
        public void SetPlayerTransform(Transform playerTransform);
        public Vector3 Move(Vector3 currentPosition, Vector3 inputDirection);
    }
}
