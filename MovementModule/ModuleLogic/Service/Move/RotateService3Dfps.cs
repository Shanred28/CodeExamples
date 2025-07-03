using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class RotateService3Dfps : IRotateService
    {
        private readonly float _rotationSpeed;

        public RotateService3Dfps(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }
    
        public void Rotate(Transform transform, Vector3 targetDirection)
        {
            float horizontalInput = targetDirection.x;
            float yAngle = horizontalInput * _rotationSpeed * Time.deltaTime;
        
            transform.Rotate(0f, yAngle, 0f);
        }
    }
}
