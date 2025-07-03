using System;
using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class RotateConfigure
    {
        public IRotateService Configure(ScriptableObject scriptableObject)
        {
            if (scriptableObject is MovementConfigureRotate rotate)
                return rotate.typeMovement switch
            {
                TypeMovement.Movement3Dfps => new RotateService3Dfps(rotate.rotationSpeed),
                _ => throw new ArgumentException("Неизвестный тип движения", nameof(scriptableObject))
            };
            
            throw new ArgumentException("Неверный тип объекта ScriptableObject. Ожидался MovementConfigureRotate.", nameof(scriptableObject));
        }
    }
}
