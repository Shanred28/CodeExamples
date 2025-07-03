
namespace MovementModule.ModuleLogic.MovementStrategy
{
    public enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Sliding,
        Jumping
    }

    public abstract class MovementStrategy
    {
        private MovementParameters _movementParameters;
        private MovementState _currentState = MovementState.Walking;
        private bool _isGrounded = true;
        private float _verticalVelocity;

        public void SetMovementParameters(MovementParameters parameters)
        {
            _movementParameters = parameters;
        }

        public void SetCurrentState(MovementState state)
        {
            _currentState = state;
        }

        public bool CheckGrounded() => _isGrounded;
        
        protected float GetSpeed()
        {
            switch (_currentState)
            {
                case MovementState.Running:
                    _currentState = MovementState.Running;
                    return _movementParameters.RunningSpeed;
                case MovementState.Crouching:
                    _currentState = MovementState.Crouching;
                    return _movementParameters.CrouchingSpeed;
                case MovementState.Sliding:
                    _currentState = MovementState.Sliding;
                    return _movementParameters.SlidingSpeed;
                case MovementState.Walking:
                    _currentState = MovementState.Walking;
                    return _movementParameters.WalkingSpeed;
                case MovementState.Jumping:
                    return _movementParameters.JumpForce;
                default:
                    return 1f;
            }
        }
        
        protected float JumpingY(float deltaTime, float newY)
        {
            if (_currentState == MovementState.Jumping)
            {
                if (_isGrounded)
                {
                    _verticalVelocity = _movementParameters.JumpForce;
                    _isGrounded = false;
                }
                newY += _verticalVelocity * deltaTime;
                _verticalVelocity -= _movementParameters.Gravity * deltaTime;

                if (newY <= 0f)
                {
                    newY = 0f;
                    _isGrounded = true;
                    _verticalVelocity = 0f;
                    _currentState = MovementState.Walking;
                }
            }
            else if (!_isGrounded)
            {
                newY += _verticalVelocity * deltaTime;
                _verticalVelocity -= _movementParameters.Gravity * deltaTime;

                if (newY <= 0f)
                {
                    newY = 0f;
                    _isGrounded = true;
                    _verticalVelocity = 0f;
                }
            }

            return newY;
        }
    }
}