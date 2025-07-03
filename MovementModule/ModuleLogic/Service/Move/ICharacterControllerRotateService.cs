using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public interface ICharacterControllerRotateService
    {
        void Initialize(Transform transformTarget, Transform cameraTransform);
        void Rotate(float horizontalInput, float verticalInput);
    }
}