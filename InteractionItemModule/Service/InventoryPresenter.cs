using System;
using System.Collections.Generic;
using System.Linq;
using BaseInfrastructure.BaseService;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using JetBrains.Annotations;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    [UsedImplicitly]
    public class InventoryPresenter
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IConfigsProvider _configsProvider;
        
        public InventoryPresenter(IInventoryRepository repository, IConfigsProvider configsProvider)
        {
            _inventoryRepository = repository;
            _configsProvider = configsProvider;
            InitializeFirstInventory();
        }

        //TODO создаем все иневнтари и игрока и предметов, пока нет того кто будет отвечать за инвентари на местности. 
        private void InitializeFirstInventory()
        {
            Debug.Log("InitializeFirstInventory");
            var inventoryConfig = _configsProvider.GetInventoryConfigureSo();

            for (int i = 0; i < inventoryConfig.Length; i++)
            {
               _inventoryRepository.RegisterInventory(inventoryConfig[i].nameInventory, new InventoryService( inventoryConfig[i]));
                if (inventoryConfig[i].ItemsSo.Length > 0)
                {
                    foreach (var t in inventoryConfig[i].ItemsSo)
                        AddItemToInventory(inventoryConfig[i].nameInventory,t);
                }
            }
        }
        
        public bool MoveItemBetweenInventories(string sourceId, string targetId, string itemUniqueId, List<Vector2Int> targetCells)
        {
            InventoryService source = _inventoryRepository.GetInventory(sourceId);
            InventoryService target = _inventoryRepository.GetInventory(targetId);
    
            if (source == null || target == null)
            {
                Debug.LogError("Один из инвентарей не найден");
                return false;
            }
            ItemAtInventory item = source.GetItemByUniqueId(itemUniqueId);
            if (item != null && target.CanPlaceItemAt(item, targetCells))
            {
                bool removed = source.RemoveItem(itemUniqueId);
                if (removed)
                {
                    if(target.AddItemAt(item, targetCells))
                    {
                        //UpdateAllInventoriesUI();
                        return true;
                    }
                    else
                    {
                        Debug.LogError("Не удалось разместить предмет в целевом инвентаре");
                    }
                }
            }
            return false;
        }
        
        public void SubscribeInventoryUpdate(string inventoryId, Action handler)
        {
            var inventory = GetInventory(inventoryId);
            if (inventory != null)
            {
                inventory.OnInventoryUpdated += handler;
            }
        }

        public void UnsubscribeInventoryUpdate(string inventoryId, Action handler)
        {
            var inventory = GetInventory(inventoryId);
            if (inventory != null)
            {
                inventory.OnInventoryUpdated -= handler;
            }
        }

        public List<ItemAtInventory> GetAllItemsInInventory(string inventoryId)
        {
            InventoryService inventory = _inventoryRepository.GetInventory(inventoryId);
            if (inventory == null)
            {
                Debug.LogError("Инвентарь не найден при получении всех предметов");
                return null;
            }
            
            return inventory.GetAllItems().ToList();
        }

        public ItemAtInventory GetItemAtCell(string inventoryId, Vector2Int cell)
        {
            var inventory = GetInventory(inventoryId);
            return inventory.GetItemAtCell(cell);
        }

        public InventoryService GetInventory(string inventoryId)
        {
           return _inventoryRepository.GetInventory(inventoryId);
        }

        public bool AddItemToInventory(string inventoryId, ItemSO item)
        {
            InventoryService inventory = _inventoryRepository.GetInventory(inventoryId);
            if(inventory.AddItem(item)) return true;
            return false;
        }
    }
}