using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MoveService2DPlatform : IMoveService
    {
        private readonly float _speed;

        public MoveService2DPlatform(float speed)
        {
            _speed = speed;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
        }

        public Vector3 Move(Vector3 currentPosition, Vector3 inputDirection)
        {
            inputDirection.y = 0;
            Vector3 targetPosition = currentPosition + inputDirection;
            return Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);
        }
    }
}