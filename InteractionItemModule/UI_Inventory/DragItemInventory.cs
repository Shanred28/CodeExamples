using System.Collections.Generic;
using BaseInfrastructure.BaseService.Input;
using BaseInfrastructure.Ticker.Interfaces;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule;
using GamePlayLogic.GameService.Spawners.ItemSpawnWorldService;
using ImportedTools.StarterPack.CoreLogic.Tools.Ticker;
using Lean.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using IDropHandler = GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule.IDropHandler;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class DragItemInventory : IDragItemUIInventory
    {
        private Transform _dragLayer;
        private IGhostView _ghostView;
        private bool _isDragging;
        private readonly IDragManager _dragManager;
        private readonly ICellHighlighter _highlighter;
        private readonly IDropHandler _dropHandler;

        private ItemAtInventory _draggedItem;
        private InventoryService _sourceInventory;
        private UIInventory _sourceUI;

        private readonly IInventoryInteraction _inventoryInteraction;
        
        private RectTransform _activeGhostRect;
        
        private Vector2 _lastDragPosition;
        
        public DragItemInventory(IInputService inputService, IInventoryInteraction inventoryInteraction,IItemSpawnService itemSpawnService)
        {
            _dragManager = new DragManager(inputService);
            _inventoryInteraction = inventoryInteraction;
            _highlighter = new CellHighlighter();
            _dropHandler = new DropHandler(_inventoryInteraction,itemSpawnService);
            
            _dragManager.OnDragStarted += OnStarted;
            _dragManager.OnDragging += OnDragging;
            _dragManager.OnDropped += OnDropped;
            _dragManager.OnRotate += OnRotate;
        }

        public void Initialize(UIDragGhostItem dragGhostPrefab, Transform dragLayer)
        {
            _ghostView = new GhostView(dragGhostPrefab,dragLayer);
        }

        public void BeginDrag(DragData dragData)
        {
            _dragManager.BeginDrag(dragData);
        }

        private void OnStarted(DragData data)
        {
            _draggedItem = data.DraggedItem;
            _sourceUI = data.SourceUI;
            _sourceInventory = data.SourceInventory;
            
            _ghostView.Show(data.IconSprite, data.DraggedItem.Positions, _sourceUI, data.DraggedItem.Rotation, 
                data.InitialPosition);
            
            bool valid = _sourceInventory.CanPlaceItemAt(_draggedItem, data.DraggedItem.Positions);
            _highlighter.Highlight(_sourceUI, data.DraggedItem.Positions, valid);
        }

        /*private void HandleRotate()
        {
            if (!_isDragging) 
                return;
            
            _draggedItem.Rotation = (_draggedItem.Rotation + 1) % 4;
            Vector2Int baseCell = _sourceUI.GetCellUnderScreenPoint(_currentPointer);
            var cells = ItemGridCalculator.CalculateOccupiedCells(
                _draggedItem.ItemSo.Shape,
                _draggedItem.Rotation,
                baseCell);

            _activeGhost.VisualUpdate(cells, _sourceUI, _draggedItem.ItemSo.Icon,_draggedItem.Rotation);
        }*/
        
        private void OnDragging(Vector2 screenPos)
        {
            _lastDragPosition = screenPos;
            _ghostView.MoveTo(screenPos);
            _highlighter.Clear(_sourceUI);

            if (TryGetTargetUIInventory(screenPos, out var targetUI))
            {
                var service = _inventoryInteraction.GetInventoryService(targetUI.InventoryName);
                var cells   = CalculateTargetCells(targetUI, screenPos);
                bool valid  = service.CanPlaceItemAt(_draggedItem, cells);
                _highlighter.Highlight(targetUI, cells, valid);
            }
            else if (TryGetRelativeTargetCell(screenPos, _sourceUI, out var targetCell))
            {
                var cells  = ItemGridCalculator.CalculateOccupiedCells(
                    _draggedItem.ItemSo.Shape,
                    _draggedItem.Rotation,
                    targetCell);
                bool valid = _sourceInventory.CanPlaceItemAt(_draggedItem, cells);
                _highlighter.Highlight(_sourceUI, cells, valid);
            }
        }
        
        private void OnRotate()
        {
            if (_draggedItem == null) 
                return;
            
            _draggedItem.Rotation = (_draggedItem.Rotation + 1) % 4;
            
            Vector2Int baseCell = _sourceUI.GetCellUnderScreenPoint(_lastDragPosition);
            var cells = ItemGridCalculator.CalculateOccupiedCells(_draggedItem.ItemSo.Shape, _draggedItem.Rotation,
                baseCell);
            
            _ghostView.Show(_draggedItem.ItemSo.Icon, cells, _sourceUI, _draggedItem.Rotation, _lastDragPosition);
            
            _highlighter.Clear(_sourceUI);
            _highlighter.Highlight(_sourceUI, cells, _sourceInventory.CanPlaceItemAt(_draggedItem, cells));
        }
        
        private void OnDropped(Vector2 screenPos)
        {
            _highlighter.Clear(_sourceUI);
            _ghostView.Hide();

            bool overAnyUI = TryGetTargetUIInventory(screenPos, out var targetUI);
            
            if (overAnyUI)
            {
                bool isCross = targetUI != _sourceUI;
                var cells = CalculateTargetCells(
                    isCross ? targetUI : _sourceUI,
                    screenPos);

                if (isCross)
                    _dropHandler.HandleCrossInventoryDrop(
                        _draggedItem, _sourceUI, targetUI, cells);
                else
                    _dropHandler.HandleLocalDrop(
                        _draggedItem, _sourceInventory, _sourceUI, cells);
            }
            else
            {
                _dropHandler.HandleThrowAway(_draggedItem, _sourceInventory);
            }
            
            _draggedItem = null;
            _sourceUI = null;
            _sourceInventory = null;
        }
        
        private bool TryGetRelativeTargetCell(Vector2 screenPosition, UIInventory ui, out Vector2Int targetCell)
        {
            targetCell = Vector2Int.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ui.GridContainer, screenPosition, null,
                    out var localPoint))
            {
                int cellX = Mathf.FloorToInt(localPoint.x / ui.CellWidth);
                int cellY = Mathf.FloorToInt(-localPoint.y / ui.CellHeight);
                targetCell = new Vector2Int(cellX, cellY);
                return true;
            }

            return false;
        }

        private bool TryGetTargetUIInventory(Vector2 screenPosition, out UIInventory targetUIInventory)
        {
            targetUIInventory = null;
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = screenPosition };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);
            foreach (var result in raycastResults)
            {
                var ui = result.gameObject.GetComponentInParent<UIInventory>();
                if (ui)
                {
                    targetUIInventory = ui;
                    return true;
                }
            }

            return false;
        }
        
        private List<Vector2Int> CalculateTargetCells(UIInventory ui, Vector2 screenPoint)
        {
            Vector2Int targetCell = ui.GetCellUnderScreenPoint(screenPoint);
            return ItemGridCalculator.CalculateOccupiedCells(_draggedItem.ItemSo.Shape, _draggedItem.Rotation,
                targetCell);
        }
        
        private bool IsFullyOutsideAllInventories()
        {
            if (_activeGhostRect == null)
                return false;
            
            Vector3[] worldCorners = new Vector3[4];
            _activeGhostRect.GetWorldCorners(worldCorners);
            
            foreach (var worldCorner in worldCorners)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldCorner);
                if (TryGetTargetUIInventory(screenPoint, out _))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}