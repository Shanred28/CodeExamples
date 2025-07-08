using System;
using BaseInfrastructure.BaseService.Input;
using ImportedTools.StarterPack.CoreLogic.Tools.Ticker;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public class DragManager : IDragManager, IDisposable
    {
        private readonly IInputService _input;
        private DragData _currentDrag;
        private bool _isDragging;
        private Vector2 _lastPointer;

        public event Action<DragData> OnDragStarted;
        public event Action<Vector2> OnDragging;
        public event Action<Vector2> OnDropped;
        public event Action OnRotate;

        public DragManager(IInputService inputService)
        {
            _input = inputService;
        }

        public void BeginDrag(DragData data)
        {
            if (_isDragging)
                CancelDrag();

            _currentDrag = data;
            _isDragging  = true;
            _lastPointer = data.InitialPosition;
            
            _input.OnPointMoved += HandlePointMoved;
            _input.OnPrimaryClick += HandlePrimaryClick;
            _input.OnSecondaryClick += HandleSecondaryClick;
            
            Ticker.RegisterUpdateable(this);
            OnDragStarted?.Invoke(data);
        }

        public void CancelDrag()
        {
            if (!_isDragging) return;
            Cleanup();
        }

        private void HandlePointMoved(Vector2 pos)
        {
            _lastPointer = pos;
        }

        private void HandlePrimaryClick()
        {
            FinishDrag(_lastPointer);
        }

        private void HandleSecondaryClick()
        {
            if (_isDragging)
                OnRotate?.Invoke();
        }

        public void OnUpdate()
        {
            if (!_isDragging) 
                return;
            OnDragging?.Invoke(_lastPointer);
        }

        private void FinishDrag(Vector2 screenPos)
        {
            if (!_isDragging) 
                return;

            OnDropped?.Invoke(screenPos);
            Cleanup();
        }

        private void Cleanup()
        {
            _input.OnPointMoved -= HandlePointMoved;
            _input.OnPrimaryClick -= HandlePrimaryClick;
            _input.OnSecondaryClick -= HandleSecondaryClick;

            Ticker.UnregisterUpdateable(this);
            _isDragging = false;
            _currentDrag = null;
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}
