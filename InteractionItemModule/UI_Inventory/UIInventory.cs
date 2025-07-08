using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class UIInventory : MonoBehaviour, IInventoryView
    {
        public float CellWidth => _cellWidth;
        public float CellHeight => _cellHeight;
        
        private float _cellWidth;
        private float _cellHeight;
        public RectTransform GridContainer => gridContainer;
        public string InventoryName => _inventoryName;
        
        private int _defaultColumns;
        private int _defaultRows;
        
        [SerializeField] private RectTransform itemsContainer;
        [SerializeField] private RectTransform gridContainer;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        [SerializeField] private GameObject panelInventory;
        [SerializeField] private UIItemViewer uiItemViewerPrefab;
        [SerializeField] private UICell gridCellPrefab;

        private string _inventoryName;
        private IInventoryInteraction _inventoryInteraction;
        private InventoryService _inventoryService;
        private readonly Dictionary<Vector2Int, UICell> _gridCells = new();
        private readonly Dictionary<string, UIItemViewer> _displayedItems = new();

        public void Initialize(IInventoryInteraction inventoryInteraction, string inventoryName)
        {
            _inventoryInteraction = inventoryInteraction;
            _inventoryService = _inventoryInteraction.GetInventoryService(inventoryName);
            
            var inventorySetting = _inventoryService.GetInventorySetting();
            _cellWidth = gridLayoutGroup.cellSize.x ;
            _cellHeight = gridLayoutGroup.cellSize.y;
            _defaultColumns = inventorySetting.columns;
            _defaultRows = inventorySetting.rows;
            
            _inventoryName = inventoryName;
            _inventoryInteraction.OnInventoryUpdated += UpdateDisplay;

            InitializeGrid();
            SynchronizeContainers();
            UpdateDisplay();
        }

        private void InitializeGrid()
        {
            ClearGrid();
            _gridCells.Clear();

            for (int row = 0; row < _defaultRows; row++)
            {
                for (int col = 0; col < _defaultColumns; col++)
                {
                    var cell = LeanPool.Spawn(gridCellPrefab, gridContainer);
                    cell.name = $"Cell_{col}_{row}";
                    
                    Vector2Int coord = new Vector2Int(col, row);
                    cell.Initialize(this, coord);
                    _gridCells.Add(coord, cell);
                }
            }
        }

        private void SynchronizeContainers()
        {
            RectTransform gridRect = gridContainer;
            RectTransform itemsRect = itemsContainer;

            itemsRect.anchorMin = gridRect.anchorMin;
            itemsRect.anchorMax = gridRect.anchorMax;
            itemsRect.pivot = gridRect.pivot;
            itemsRect.anchoredPosition = gridRect.anchoredPosition;
            itemsRect.sizeDelta = gridRect.sizeDelta;
        }

        public void BeginDrag(Vector2Int cellCoords, Vector2 initialPosition)
        {
            var item = _inventoryInteraction.GetItemAtCell(_inventoryName, cellCoords);
            if (item != null)
            {
                var sourceInventory = _inventoryInteraction.GetInventoryService(_inventoryName);
                var dragData = new DragData
                {
                    DraggedItem = item,
                    SourceInventory = sourceInventory,
                    SourceUI = this,
                    IconSprite = item.ItemSo.Icon,
                    InitialPosition = initialPosition
                };
                _inventoryInteraction.OnStartDragItem(dragData);
            }
        }

        public void UpdateDisplay()
        {
            ClearItemViewers();
            List<ItemAtInventory> allItems = _inventoryInteraction.GetAllItems(_inventoryName);
            foreach (var item in allItems)
            {
                UIItemViewer viewer = LeanPool.Spawn(uiItemViewerPrefab, itemsContainer);
                viewer.InitializeDisplay(item, this);
                _displayedItems[item.UniqueID] = viewer;
            }

            CancelHighlightCells();
        }

        private void ClearItemViewers()
        {
            foreach (var viewer in _displayedItems.Values)
            {
                LeanPool.Despawn(viewer.gameObject);
            }

            _displayedItems.Clear();
        }

        private void ClearGrid()
        {
            for (int i = gridContainer.childCount - 1; i >= 0; i--)
            {
                LeanPool.Despawn(gridContainer.GetChild(i).gameObject);
            }
        }

        public void HighlightCells(List<Vector2Int> targetCells, bool isValidPlacement)
        {
            foreach (var cell in _gridCells.Values)
            {
                cell.SetHighlight(false, true);
            }

            foreach (var coord in targetCells)
            {
                if (_gridCells.TryGetValue(coord, out UICell cell))
                {
                    cell.SetHighlight(true, isValidPlacement);
                }
            }
        }

        public void CancelHighlightCells()
        {
            foreach (var cell in _gridCells.Values)
            {
                cell.DefaultHighlight();
            }
        }

        public void ToggleInventory()
        {
            panelInventory.SetActive(!panelInventory.activeSelf);
        }

        public Vector2Int GetCellUnderScreenPoint(Vector2 screenPoint)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GridContainer, screenPoint, null,
                    out var localPoint))
            {
                int cellX = Mathf.FloorToInt(localPoint.x / _cellWidth);
                int cellY = Mathf.FloorToInt(-localPoint.y / _cellHeight);
                return new Vector2Int(cellX, cellY);
            }

            return Vector2Int.zero;
        }
        
        private void OnDestroy()
        {
            if (_inventoryInteraction != null)
                _inventoryInteraction.OnInventoryUpdated -= UpdateDisplay;
        }
    }
}