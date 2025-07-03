using System;
using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class GroundedConfigure
    {
        public IIsGroundedService Configure(ScriptableObject scriptableObject)
        {
            if (scriptableObject is not MovementConfigureGrounded grounded)
            {
                throw new ArgumentException(
                    "Неверный тип объекта ScriptableObject. Ожидался MovementConfigureGrounded.",
                    nameof(scriptableObject));
            }

            IIsGroundedService Create2DPointService() =>
                new Grounded2DPointService(grounded.groundCheckRadius, grounded.groundLayer);

            IIsGroundedService Create2DColliderService() => new Grounded2DColliderService(grounded.groundLayer);

            IIsGroundedService Create3DPointService() =>
                new Grounded3DPointService(grounded.groundCheckRadius, grounded.groundLayer);

            IIsGroundedService Create3DTriggerService() => new Grounded3DTriggerService();

            return grounded.groundedCheckType switch
            {
                GroundedCheckType.CheckGround2dPoint => Create2DPointService(),
                GroundedCheckType.CheckGround2dCollider => Create2DColliderService(),
                GroundedCheckType.CheckGround2dAny => new CompositeGroundedService(new[]
                {
                    Create2DPointService(),
                    Create2DColliderService()
                }),
                GroundedCheckType.CheckGround2dAll => new CompositeAllGroundedService(new[]
                {
                    Create2DPointService(),
                    Create2DColliderService()
                }),
                GroundedCheckType.CheckGround3dPoint => Create3DPointService(),
                GroundedCheckType.CheckGround3dCollider => Create3DTriggerService(),
                GroundedCheckType.CheckGround3dAny => new CompositeGroundedService(new[]
                {
                    Create3DPointService(),
                    Create3DTriggerService()
                }),
                GroundedCheckType.CheckGround3dAll => new CompositeAllGroundedService(new[]
                {
                    Create3DPointService(),
                    Create3DTriggerService()
                }),
                _ => throw new ArgumentException("Неизвестный тип проверки", nameof(scriptableObject))
            };
        }
    }
}