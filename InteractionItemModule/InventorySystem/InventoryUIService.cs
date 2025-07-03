using System;
using System.Collections.Generic;
using BaseInfrastructure.BaseService.Input;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using GamePlayLogic.GameService.InteractionItemModule.Service;
using GamePlayLogic.GameService.InteractionItemModule.UI_Inventory;
using GamePlayLogic.GameService.Spawners.ItemSpawnWorldService;
using JetBrains.Annotations;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    [UsedImplicitly]
    public class InventoryUIService : IInventoryInteraction
    {
        public event Action OnInventoryUpdated;

        private readonly InventoryPresenter _inventoryPresenter;

        private readonly InputService _inputService;
        private readonly InventoryUIView _viewInventoryUI;

        //TODO пусть балтается пока тут, пока нет общей системы.
        private const string InventoryPlayerId = "Backpack";

        private UIInventory _playerInventoryUI;
        private UIInventory _guestInventoryUI;
        private IDragItemUIInventory _dragItemHelper;
        private readonly IItemSpawnService _itemSpawnService;
        private readonly List<string> _openInventories = new List<string>();

        public InventoryUIService(InventoryPresenter inventoryPresenter, InputService inputService,InventoryUIView viewInventoryUI, IItemSpawnService itemSpawnService)
        {
            _inventoryPresenter = inventoryPresenter;
            _inputService = inputService;
            _viewInventoryUI = viewInventoryUI;
            _itemSpawnService = itemSpawnService;
            Initialize();
        }

        private void Initialize()
        {
            _inventoryPresenter.SubscribeInventoryUpdate(InventoryPlayerId, InventoryChangedHandler);
            
            _dragItemHelper = new DragItemInventory(_inputService.GetInputSystemActions(), this,_itemSpawnService);
            _dragItemHelper.Initialize(_viewInventoryUI.DragPrefab, _viewInventoryUI.TransformCanvas);
            
        }
        
        public InventoryService GetInventoryService(string inventoryId) => _inventoryPresenter.GetInventory(inventoryId);
        
        public void OpenInventory(string[] inventoryId)
        {
            foreach (var id in inventoryId)
            {
                if (!_openInventories.Contains(id))
                {
                    _inventoryPresenter.SubscribeInventoryUpdate(id, InventoryChangedHandler);
                    _openInventories.Add(id);
                }
            }
            _viewInventoryUI.OpenInventory(this, inventoryId);
        }

        public void CloseInventory()
        {
            foreach (var id in _openInventories)
                _inventoryPresenter.UnsubscribeInventoryUpdate(id, InventoryChangedHandler);
    
            _openInventories.Clear();
            _viewInventoryUI.CloseAll();
        }
        
        private void InventoryChangedHandler()
        {
            OnInventoryUpdated?.Invoke();
        }

        public List<ItemAtInventory> GetAllItems(string inventoryId) => _inventoryPresenter.GetAllItemsInInventory(inventoryId);

        public bool MoveItemBetweenInventories(string sourceInventoryId, string targetInventoryId, string itemUniqueId,
            List<Vector2Int> targetCells)
        {
            bool success = _inventoryPresenter.MoveItemBetweenInventories(sourceInventoryId, targetInventoryId, itemUniqueId, targetCells);
            Debug.Log(success 
                ? "Предмет успешно перемещён между инвентарями." 
                : "Перемещение предмета не удалось.");
            return success;
        }
        
        public void OnStartDragItem(DragData dragData)
        {
            _dragItemHelper.BeginDrag(dragData);
        }

        public ItemAtInventory GetItemAtCell(string nameInventory, Vector2Int cell) => _inventoryPresenter.GetItemAtCell(nameInventory, cell);
        
        public void AddItem(ItemSO itemSo)
        {
            _inventoryPresenter.AddItemToInventory(InventoryPlayerId, itemSo);
        }
    }
}