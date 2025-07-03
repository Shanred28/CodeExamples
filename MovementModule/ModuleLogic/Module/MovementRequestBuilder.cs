using UnityEngine;

namespace MovementModule.ModuleLogic
{
    public class MovementRequestBuilder
    {
        private readonly InputSystem_Actions _inputSystemActions;

        public MovementRequestBuilder(InputSystem_Actions inputSystemActions)
        {
            _inputSystemActions = inputSystemActions;
        }

        public MovementRequestInput BuildMovementRequest(Vector3 currentPosition, float deltaTime)
        {
            Vector2 input = _inputSystemActions.Player2D.Move.ReadValue<Vector2>();
            
            return new MovementRequestInput
            {
                CurrentPosition = currentPosition,
                MovementInput = input,
                DeltaTime = deltaTime
            };
        }
    }
}
