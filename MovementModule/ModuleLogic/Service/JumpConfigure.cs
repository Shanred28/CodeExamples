using System;
using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public class JumpConfigure 
    {
        public IJumpService Configure(ScriptableObject scriptableObject)
        {
            if (scriptableObject is MovementConfigureJumped jump)
                return jump.movementType switch
                {
                    TypeMovement.Movement2DPlatform => new JumpService(jump.jumpForce, jump.gravity),
                    TypeMovement.Movement2DTopDown => new JumpService(jump.jumpForce, jump.gravity),
                    TypeMovement.Movement3Dfps => new JumpService(jump.jumpForce, jump.gravity),
                    _ => throw new ArgumentException("Неизвестный тип движения", nameof(scriptableObject))
                };
            throw new ArgumentException("Неверный тип объекта ScriptableObject. Ожидался MovementConfigureJumped.", nameof(scriptableObject));
        }
    }
}
