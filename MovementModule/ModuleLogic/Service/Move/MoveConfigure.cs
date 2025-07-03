using System;
using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.Move
{
    public class MoveConfigure
    {
        public IMoveService Configure(ScriptableObject scriptableObject)
        {
            if (scriptableObject is MovementConfigureMove move)
                return move.movementType switch
                {
                    TypeMovement.Movement2DPlatform => new MoveService2DPlatform(move.speed),
                    TypeMovement.Movement2DTopDown => new MoveService2DTopDown(move.speed),
                    TypeMovement.Movement3Dfps => new MoveService3Dfps(move.speed),
                    _ => throw new ArgumentException("Неизвестный тип движения", nameof(scriptableObject))
                };
            throw new ArgumentException("Неверный тип объекта ScriptableObject. Ожидался MovementConfigureMove.", nameof(scriptableObject));
        }
    }
}
