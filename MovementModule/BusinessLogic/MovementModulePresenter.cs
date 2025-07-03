using System;
using JetBrains.Annotations;
using MovementModule.ModuleLogic.Service;
using MovementModule.ModuleLogic.Service.GroundedService;
using MovementModule.ModuleLogic.Service.Move;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementModule.BusinessLogic
{
    [UsedImplicitly]
    public class MovementModulePresenter : IDisposable
    {
        private Transform _transformView;
        
        private readonly IMoveService _moveService;
        private readonly IIsGroundedService _isGroundedService;
        private readonly IRotateService _rotateService;
        private readonly IJumpService _jumpService;
        
        private InputSystem_Actions _inputSystemActions;

        public MovementModulePresenter(IMoveService moveService, IIsGroundedService isGroundedService,IRotateService rotateService,IJumpService jumpService)
        {
            _moveService = moveService;
            _isGroundedService = isGroundedService;
            _rotateService = rotateService;
            _jumpService = jumpService;
        }
        
        public  void Init(Transform transformView, Transform groundCheckPoint )
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Player2D.Enable();
            
            _inputSystemActions.Player2D.Jump.started += JumpOnStarted;
            _transformView  = transformView;
            _moveService.SetPlayerTransform(_transformView);
            _isGroundedService.SetGroundCheckPoint(groundCheckPoint);
        }
        
        private void JumpOnStarted(InputAction.CallbackContext obj)
        {
            if(_isGroundedService.IsGrounded() && !_jumpService.IsJumping)  _jumpService.StartJump();
        }
        
        private void UpdateJump()
        {
            Vector3 jumpDelta = _jumpService.Jump();
            _transformView.position += jumpDelta;
            
            if (_isGroundedService.IsGrounded())
            {
                _jumpService.EndJump();
            }
        }
        
        public void Move()
        {
            _transformView.position  = _moveService.Move(_transformView.position,_inputSystemActions.Player2D.Move.ReadValue<Vector2>());
            _rotateService.Rotate(_transformView,_inputSystemActions.Player2D.Look.ReadValue<Vector2>());

            if(_jumpService.IsJumping) UpdateJump();
        }

        public void Dispose()
        {
            _inputSystemActions?.Dispose();
        }
    }
}
