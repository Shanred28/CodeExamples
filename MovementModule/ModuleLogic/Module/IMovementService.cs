using UnityEngine;

namespace MovementModule.ModuleLogic
{
    public interface IMovementService
    {
        Vector3 ComputeMovement(MovementRequestInput movementRequestInput);
        void SwitchToRunning();
        void SwitchToWalking();
        void SwitchToCrouching();
        void SwitchToSliding();
    }
    
    public interface IJumpable
    {
        void Jump();
    }
}
