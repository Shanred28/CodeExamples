using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public interface IRotateService
    {
       public void Rotate(Transform transform, Vector3 targetDirection);
    }
}
