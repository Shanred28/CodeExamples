using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MoveService2DTopDown : IMoveService
    {
        private readonly float _speed;

        public MoveService2DTopDown(float speed)
        {
            _speed = speed;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 Move(Vector3 currentPosition, Vector3 inputDirection)
        {
            if (inputDirection.sqrMagnitude > 1f)
            {
                inputDirection.Normalize();
            }
            
            var targetPosition = inputDirection * (_speed * Time.deltaTime);
            Vector3 newPosition = new Vector3(currentPosition.x + targetPosition.x, 0, currentPosition.z + targetPosition.y);
            return newPosition;
        }
    }
}
