
using MovementModule.ModuleLogic.MovementStrategy;
using UnityEngine;

namespace MovementModule.ModuleLogic.MovementComponent
{
    public class MovementJumpComponent : IJumpable
    {
        private readonly BaseMovementService _baseService;

        public MovementJumpComponent(BaseMovementService baseService)
        {
            _baseService = baseService;
        }
        
        public void Jump()
        {
            if (_baseService.MovementStrategy.CheckGrounded())
            {
                _baseService.MovementStrategy.SetCurrentState(MovementState.Jumping);
            }
        }
    }
    
    public class NullJumpable : IJumpable
    {
        public void Jump()
        {
            Debug.Log("Прыжок не поддерживается данной конфигурацией.");
        }
    }
}
