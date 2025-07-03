using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MockIRotateService : IRotateService
    {
        public void Rotate(Transform transform, Vector3 targetDirection)
        {
            Debug.Log("Service is not configured in SO.");
        }
    }
}
