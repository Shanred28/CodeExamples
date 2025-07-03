using MovementModule.ModuleLogic.MovementComponent;
using MovementModule.ModuleLogic.MovementStrategy;
using UnityEngine;

namespace MovementModule.ModuleLogic
{
    public class MovementService : IMovementService, IJumpable
    {
        private  const string CONFIG_MOVEMENT_SETTING = "Configs/MovementModuleConfig/MovementSettings";
        
        private readonly BaseMovementService _baseService;
        private readonly IJumpable _jumpComponent;
        private readonly IMovementStrategy _movementStrategy;

        public MovementService()
        {
            MovementSettingsSo config = (MovementSettingsSo) Resources.Load(CONFIG_MOVEMENT_SETTING);
            _movementStrategy = GetMovementStrategy(config.movementType);
            _baseService = new BaseMovementService(config);

            _jumpComponent = config.canJump ? new MovementJumpComponent(_baseService) : new NullJumpable();
        }
        
        private IMovementStrategy GetMovementStrategy(MovementType movementType)
        {
            return movementType switch
            {
                MovementType.TwoD => new Movement2DStrategy(),
                MovementType.ThreeD => new Movement3DStrategy(),
                _ => null
            };
        }
        
        public void Jump() => _jumpComponent.Jump();
        public Vector3 ComputeMovement(MovementRequestInput movementRequestInput) => _baseService.ComputeMovement(movementRequestInput);

        public void SwitchToRunning() => _baseService.SwitchToRunning();
        public void SwitchToWalking() => _baseService.SwitchToWalking();
        public void SwitchToCrouching() => _baseService.SwitchToCrouching();
        public void SwitchToSliding() => _baseService.SwitchToSliding();
    }
}