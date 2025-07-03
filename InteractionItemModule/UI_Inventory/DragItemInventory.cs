using System.Collections.Generic;
using BaseInfrastructure.Ticker.Interfaces;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using GamePlayLogic.GameService.Spawners.ItemSpawnWorldService;
using ImportedTools.StarterPack.CoreLogic.Tools.Ticker;
using Lean.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class DragItemInventory : IDragItemUIInventory, IUpdateable
    {
        private UIDragGhostItem _dragGhostPrefab;
        private Transform _dragLayer;
        private UIDragGhostItem _activeGhost;
        private bool _isDragging;
        private readonly InputSystem_Actions _inputSystemActions;
        private readonly IItemSpawnService _dropService;

        private ItemAtInventory _draggedItem;
        private InventoryService _sourceInventory;
        private UIInventory _sourceUI;

        private readonly IInventoryInteraction _inventoryInteraction;
        
        private RectTransform _activeGhostRect;
        
        public DragItemInventory(InputSystem_Actions inputService, IInventoryInteraction inventoryInteraction,IItemSpawnService itemSpawnService)
        {
            _inputSystemActions = inputService;
            _dropService = itemSpawnService;
            _inventoryInteraction = inventoryInteraction;
        }

        public void Initialize(UIDragGhostItem dragGhostPrefab, Transform dragLayer)
        {
            _dragGhostPrefab = dragGhostPrefab;
            _dragLayer = dragLayer;
        }

        public void BeginDrag(DragData dragData)
        {
            if (_isDragging)
                EndDrag(_inputSystemActions.UI.Point.ReadValue<Vector2>());
            
            _sourceUI = dragData.SourceUI;
            _draggedItem = dragData.DraggedItem;
            _sourceInventory = dragData.SourceInventory;

            _activeGhost = LeanPool.Spawn(_dragGhostPrefab, _dragLayer);
            _activeGhostRect = _activeGhost.GetComponent<RectTransform>();
            
            _activeGhost.VisualUpdate(_draggedItem.Positions,_sourceUI,dragData.IconSprite,dragData.DraggedItem.Rotation);
            _activeGhost.transform.position = dragData.InitialPosition;
            
            _isDragging = true;
            _inputSystemActions.UI.Enable();
            _inputSystemActions.UI.Click.performed += OnDropping;
            _inputSystemActions.PlayerWalking.SecondaryClick.performed  += OnRotate;
            Ticker.RegisterUpdateable(this);
        }
        
        private void OnRotate(InputAction.CallbackContext ctx)
        {
            if (!_isDragging) 
                return;
            
            _draggedItem.Rotation = (_draggedItem.Rotation + 1) % 4;
            
            Vector2 mousePos = _inputSystemActions.UI.Point.ReadValue<Vector2>();
            Vector2Int baseCell = _sourceUI.GetCellUnderScreenPoint(mousePos);
            var cells = ItemGridCalculator.CalculateOccupiedCells(
                _draggedItem.ItemSo.Shape,
                _draggedItem.Rotation,
                baseCell);

            _activeGhost.VisualUpdate(cells, _sourceUI, _draggedItem.ItemSo.Icon,_draggedItem.Rotation);
        }

        private void OnDropping(InputAction.CallbackContext context)
        {
            _inputSystemActions.UI.Click.performed -= OnDropping;
            EndDrag(_inputSystemActions.UI.Point.ReadValue<Vector2>());
        }

        public void OnUpdate()
        {
            UpdateDrag();
        }

        private void UpdateDrag()
        {
            if (!_isDragging || _activeGhost == null) return;

            Vector2 currentPos = _inputSystemActions.UI.Point.ReadValue<Vector2>();
            _activeGhost.transform.position = currentPos;

            if (TryGetTargetUIInventory(currentPos, out var targetUI))
            {
                UpdateTargetHighlight(targetUI, currentPos);
            }
            else
            {
                if (TryGetRelativeTargetCell(currentPos, _sourceUI, out Vector2Int targetCell))
                {
                    List<Vector2Int> cells = ItemGridCalculator.CalculateOccupiedCells(_draggedItem.ItemSo.Shape,
                        _draggedItem.Rotation, targetCell);
                    bool isValid = _sourceInventory.CanPlaceItemAt(_draggedItem, cells);
                    _sourceUI.HighlightCells(cells, isValid);
                }
            }
        }

        private void EndDrag(Vector2 screenPosition)
        {
            _inputSystemActions.UI.Click.performed -= OnDropping;
            _inputSystemActions.PlayerWalking.SecondaryClick.performed  -= OnRotate;
            
            Ticker.UnregisterUpdateable(this);

            bool overAnyUI = TryGetTargetUIInventory(screenPosition, out var targetUI);
            bool isCross = overAnyUI && targetUI != _sourceUI;
            
            if (overAnyUI)
            {
                if (isCross)
                    HandleCrossInventoryDrop(targetUI, screenPosition);
                else
                    HandleLocalDrop(screenPosition);
            }
            else if (IsFullyOutsideAllInventories())
            {
                ThrowAwayItem();
            }
            else
            {
                _sourceUI.CancelHighlightCells();
                Debug.Log("UI не определён. Возврат в исходное положение.");
            }

            if (_activeGhost != null)
                LeanPool.Despawn(_activeGhost);

            _activeGhost = null;
            _activeGhostRect = null;
            _isDragging = false;
            _draggedItem = null;
            _sourceInventory = null;
        }

        private void HandleCrossInventoryDrop(UIInventory targetUI, Vector2 screenPosition)
        {
            Vector2Int targetCell = targetUI.GetCellUnderScreenPoint(screenPosition);
            List<Vector2Int> cells =
                ItemGridCalculator.CalculateOccupiedCells(_draggedItem.ItemSo.Shape, _draggedItem.Rotation, targetCell);
            bool moved = _inventoryInteraction.MoveItemBetweenInventories(_sourceUI.InventoryName,
                targetUI.InventoryName,
                _draggedItem.UniqueID,
                cells);
            targetUI.UpdateDisplay();
            
            if (!moved)Debug.Log("Перемещение предмета между инвентарями не удалось.");
            _sourceUI.CancelHighlightCells();
            targetUI.CancelHighlightCells();
        }

        private void HandleLocalDrop(Vector2 screenPosition)
        {
            if (TryGetRelativeTargetCell(screenPosition, _sourceUI, out Vector2Int targetCell))
            {
                List<Vector2Int> cells = ItemGridCalculator.CalculateOccupiedCells(_draggedItem.ItemSo.Shape,
                    _draggedItem.Rotation, targetCell);
                if (_sourceInventory.CanPlaceItemAt(_draggedItem, cells))
                {
                    _sourceInventory.UpdateItemPlace(_draggedItem.UniqueID, cells, _draggedItem.Rotation);
                    _sourceUI.UpdateDisplay();
                }
                else
                {
                    _sourceUI.CancelHighlightCells();
                    Debug.Log("Невозможно разместить предмет в выбранной позиции. Возврат в исходное положение.");
                }
            }
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

        private void UpdateTargetHighlight(UIInventory targetUI, Vector2 screenPos)
        {
            List<Vector2Int> cells = CalculateTargetCells(targetUI, screenPos);
            InventoryService targetService = _inventoryInteraction.GetInventoryService(targetUI.InventoryName);
            bool isValid = targetService?.CanPlaceItemAt(_draggedItem, cells) ?? _sourceInventory.CanPlaceItemAt(_draggedItem, cells);
            targetUI.HighlightCells(cells, isValid);
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
        
        private void ThrowAwayItem()
        {
            _sourceInventory.RemoveItem(_draggedItem.UniqueID);
            _dropService.Drop(DropRequest.NearPlayer(_draggedItem.ItemSo));
            Debug.Log($"Предмет {_draggedItem.ItemSo.name} выброшен в мир.");
        }
    }
}