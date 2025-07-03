using System;
using MovementModule.ModuleLogic.MovementStrategy;
using UnityEngine;

namespace MovementModule.ModuleLogic
{
    public class BaseMovementService : IMovementService
    {
        public IMovementStrategy MovementStrategy { get; private set; }

        public BaseMovementService(MovementSettingsSo config)
        {
            MovementStrategy = GetMovementStrategy(config.movementType);
            MovementStrategy.SetMovementParameters(new MovementParameters(config));
        }
        
        private IMovementStrategy GetMovementStrategy(MovementType movementType)
        {
            return movementType switch
            {
                MovementType.TwoD => new Movement2DStrategy(),
                MovementType.ThreeD => new Movement3DStrategy(),
                MovementType.Isometric => new MovementIsometricStrategy(),
                _ => throw new ArgumentException("Неизвестный тип перемещения")
            };
        }
        
        public Vector3 ComputeMovement(MovementRequestInput input) 
            => MovementStrategy.CalculateNextPosition(input.CurrentPosition, input.CurrentPosition + new Vector3(input.MovementInput.x, 0, input.MovementInput.y), 
                input.DeltaTime);

        public void SwitchToRunning() => MovementStrategy.SetCurrentState(MovementState.Running);
        public void SwitchToCrouching() => MovementStrategy.SetCurrentState(MovementState.Crouching);
        public void SwitchToSliding() => MovementStrategy.SetCurrentState(MovementState.Sliding);
        public void SwitchToWalking() => MovementStrategy.SetCurrentState(MovementState.Walking);
    }
}
