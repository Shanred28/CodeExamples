using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    public class InventoryService
    {
        private class PlacementCandidate
        {
            public bool Found { get; set; }
            public int Rotation { get; set; }
            public Vector2Int StartPosition { get; set; }
            public List<Vector2Int> RotatedShape { get; set; }
        }
        
        public event Action OnInventoryUpdated;
        
        private readonly int _width;
        private readonly int _height;
        private readonly InventorySetting _inventorySetting;
        
        private readonly InventoryGrid _inventoryGrid;
        private readonly Dictionary<string, List<ItemAtInventory>> _itemStacks;

        public InventoryService(InventorySetting inventorySetting)
        {
            _inventorySetting = inventorySetting;
            _width = _inventorySetting.columns;
            _height = _inventorySetting.rows;
            _inventoryGrid = new InventoryGrid(_width, _height);
            _itemStacks = new Dictionary<string, List<ItemAtInventory>>();
        }
        public bool AddItem(ItemSO item)
        {
            if (item == null)
                return false;

            bool result = !item.IsStackable
                ? TryPlaceItem(item)
                : AddStackableItem(item);
            
            if (result)
                OnInventoryUpdated?.Invoke();
            return result;
        }
        
        public bool AddItemAt(ItemAtInventory item, List<Vector2Int> targetCells)
        {
            if (!targetCells.All(cell => _inventoryGrid.IsCellFree(cell, item.ItemSo.ID)))
                return false;
            
            foreach (Vector2Int pos in targetCells)
                _inventoryGrid.SetCell(pos, item.ItemSo.ID);
            
            item.Positions = targetCells;
            
            List<ItemAtInventory> stackList = GetOrCreateStackList(item.ItemSo.ID);
            stackList.Add(item);

            OnInventoryUpdated?.Invoke();
            return true;
        }

        public bool RemoveItem(string uniqueId)
        {
            foreach (var key in _itemStacks.Keys.ToList())
            {
                var stack = _itemStacks[key].FirstOrDefault(x => x.UniqueID == uniqueId);
                if (stack != null)
                {
                    foreach (var pos in stack.Positions)
                    {
                        _inventoryGrid.ClearCell(pos, stack.ItemSo.ID);
                    }

                    _itemStacks[key].Remove(stack);
                    if (_itemStacks[key].Count == 0)
                        _itemStacks.Remove(key);
                    OnInventoryUpdated?.Invoke();
                    return true;
                }
            }

            return false;
        }
        
        public ItemAtInventory GetItemByUniqueId(string uniqueId)
        {
            return _itemStacks.Values.SelectMany(stack => stack)
                .FirstOrDefault(item => item.UniqueID == uniqueId);
        }

        public bool CanPlaceItemAt(ItemAtInventory itemInstance, List<Vector2Int> targetCells)
        {
            return targetCells.All(cell =>
            {
                if (itemInstance.Positions.Contains(cell))
                    return true;
                
                return _inventoryGrid.IsCellFree(cell, itemInstance.ItemSo.ID);
            });
        }

        public IEnumerable<ItemAtInventory> GetAllItems() =>
            _itemStacks.Values.SelectMany(s => s);
        
        public void UpdateItemPlace(string uniqueId, List<Vector2Int> newPositions, int newRotation)
        {
            ItemAtInventory item = FindItemByUniqueId(uniqueId);
            if (item == null)
                return;

            UpdateCellsForItem(item, newPositions);
            item.Rotation = newRotation;
            OnInventoryUpdated?.Invoke();
        }

        private bool TryPlaceItem(ItemSO item)
        {
            if (item == null || item.Shape == null || item.Shape.Count == 0)
                return false;

            var candidate = FindBestPlacement(item);
            if (!candidate.Found || candidate.RotatedShape == null)
                return false;

            List<Vector2Int> occupiedPositions = _inventoryGrid.PlaceShape(item.ID, candidate.RotatedShape, candidate.StartPosition);
            var newItem = new ItemAtInventory
            {
                UniqueID = Guid.NewGuid().ToString(),
                ItemSo = item,
                Positions = occupiedPositions,
                Rotation = candidate.Rotation,
                AmountStack = item.AmountStack,
                AmountValue = 0,
            };

            GetOrCreateStackList(item.ID).Add(newItem);
            return true;
        }

        private PlacementCandidate FindBestPlacement(ItemSO item)
        {
            var candidate = new PlacementCandidate { Found = false, StartPosition = new Vector2Int(0, int.MaxValue) };
            
            for (int rotation = 0; rotation < 4; rotation++)
            {
                var rotatedShape = NormalizedShapeHelper.GetRotatedNormalizedShape(item.Shape, rotation);
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        var pos = new Vector2Int(x, y);
                        if (_inventoryGrid.CanPlaceShape(rotatedShape, pos))
                        {
                            if (!candidate.Found ||
                                pos.y < candidate.StartPosition.y ||
                                (pos.y == candidate.StartPosition.y && pos.x < candidate.StartPosition.x))
                            {
                                candidate.Found = true;
                                candidate.Rotation = rotation;
                                candidate.StartPosition = pos;
                                candidate.RotatedShape = rotatedShape;
                            }
                        }
                    }
                }
            }
            return candidate;
        }

        private bool AddStackableItem(ItemSO item)
        {
            int remaining = item.AmountValue;
            List<ItemAtInventory> stacks = GetOrCreateStackList(item.ID);
            
            foreach (var stack in stacks)
            {
                int availableSpace = stack.AmountStack - stack.AmountValue;
                if (availableSpace > 0)
                {
                    int toAdd = Mathf.Min(availableSpace, remaining);
                    stack.AmountValue += toAdd;
                    remaining -= toAdd;
                    if (remaining <= 0)
                        return true;
                }
            }
            
            while (remaining > 0)
            {
                if (!CreateNewStackForItem(item, ref remaining))
                    return false;
            }
            return true;
        }

        private List<ItemAtInventory> GetOrCreateStackList(string itemId)
        {
            if (!_itemStacks.TryGetValue(itemId, out var stackList))
            {
                stackList = new List<ItemAtInventory>();
                _itemStacks[itemId] = stackList;
            }
            return stackList;
        }

        private bool CreateNewStackForItem(ItemSO item, ref int remaining)
        {
            if (!TryPlaceItem(item))
                return false;

            List<ItemAtInventory> stacks = GetOrCreateStackList(item.ID);
            ItemAtInventory newStack = stacks.Last();
            int toAdd = Mathf.Min(item.AmountStack, remaining);
            newStack.AmountValue = toAdd;
            remaining -= toAdd;
            return true;
        }
        
        private ItemAtInventory FindItemByUniqueId(string uniqueId) => _itemStacks.Values.SelectMany(s => s)
                .FirstOrDefault(item => item.UniqueID == uniqueId);
        
        private void UpdateCellsForItem(ItemAtInventory item, List<Vector2Int> newPositions)
        {
            foreach (Vector2Int pos in item.Positions)
            {
                if (_inventoryGrid.GetCell(pos) == item.ItemSo.ID)
                    _inventoryGrid.ClearCell(pos, item.ItemSo.ID);
            }
            foreach (Vector2Int pos in newPositions)
                _inventoryGrid.SetCell(pos, item.ItemSo.ID);
            item.Positions = newPositions;
        }
        
        public ItemAtInventory GetItemAtCell(Vector2Int cell)
        {
            return GetAllItems().FirstOrDefault(item => item.Positions.Contains(cell));
        }
        
        public InventorySetting GetInventorySetting() => _inventorySetting;
    }
}