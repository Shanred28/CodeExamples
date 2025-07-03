using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class RotateService3DfpsCharacterController : ICharacterControllerRotateService 
    {
        private Transform _transformTarget;
        private Transform _cameraTransform;
        private readonly float _sensitivity;
        private readonly float _maxAngleVertical;
        private readonly float _minAngleVertical;
   
        private float _verticalRotation = 0f;

        public RotateService3DfpsCharacterController(float sensitivity, float maxAngleVertical, float minAngleVertical)
        {
            _sensitivity = sensitivity;
            _maxAngleVertical = maxAngleVertical;
            _minAngleVertical = minAngleVertical;
        }

        public void Initialize(Transform transformTarget, Transform cameraTransform)
        {
            _transformTarget = transformTarget;
            _cameraTransform = cameraTransform;
        }

        public void Rotate(float horizontalInput, float verticalInput)
        {
            _transformTarget.Rotate(0f, horizontalInput * _sensitivity, 0f);
       
            _verticalRotation -= verticalInput * _sensitivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, _minAngleVertical, _maxAngleVertical);
            _cameraTransform.localEulerAngles = new Vector3(_verticalRotation, 0f, 0f);
        }
    }
}