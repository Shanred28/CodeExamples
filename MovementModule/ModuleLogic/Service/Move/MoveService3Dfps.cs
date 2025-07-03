using System;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MoveService3Dfps : IMoveService
    {
        private readonly float _speed;
        private Transform _playerTransform;

        public MoveService3Dfps(float speed)
        {
            _speed = speed;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public Vector3 Move(Vector3 currentPosition, Vector3 inputDirection)
        {
            if (_playerTransform == null)
                throw new NullReferenceException("Player transform is not assigned.");

            Vector3 localInput = new Vector3(inputDirection.x, 0f, inputDirection.y);
            if (localInput.sqrMagnitude > 1f)
            {
                localInput.Normalize();
            }

            Vector3 moveDirection = _playerTransform.right * localInput.x + _playerTransform.forward * localInput.z;
            Vector3 targetPosition = currentPosition + moveDirection;
            return Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);
        }
    }
}