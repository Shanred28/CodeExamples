using UnityEngine;

namespace MovementModule.ModuleLogic.MovementStrategy
{
    public interface IMovementStrategy
    {
        public void SetMovementParameters(MovementParameters parameters);
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition, float deltaTime);
        public void SetCurrentState(MovementState state);

        public bool CheckGrounded();
    }
    
    public  class MovementParameters
    {
        public MovementParameters(MovementSettingsSo settings)
        {
            WalkingSpeed = settings.walkingSpeed;
            Acceleration = settings.acceleration;
            RunningSpeed = settings.runningSpeed;
            CrouchingSpeed = settings.crouchingSpeed;
            SlidingSpeed  = settings.slidingSpeed;
            JumpForce = settings.jumpForce;
            Gravity = settings.gravity;
        }

        public readonly float WalkingSpeed;
        public float Acceleration;
        public readonly float RunningSpeed;
        public readonly float CrouchingSpeed;
        public readonly float SlidingSpeed;
        public readonly float JumpForce;
        public readonly float Gravity;
    }
}
