using System;
using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using JetBrains.Annotations;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    [UsedImplicitly]
    public class InventoryRepository: IInventoryRepository
    {
        private readonly Dictionary<string, InventoryService> _inventoryDict = new();

        public void RegisterInventory(string id, InventoryService inventoryService)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _inventoryDict[id] = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }
        
        public InventoryService GetInventory(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            _inventoryDict.TryGetValue(id, out var inventory);
            return inventory;
        }
        
        public bool UnregisterInventory(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return _inventoryDict.Remove(id);
        }
        
        public IEnumerable<InventoryService> GetAllInventories()
        {
            return _inventoryDict.Values;
        }
    }
}