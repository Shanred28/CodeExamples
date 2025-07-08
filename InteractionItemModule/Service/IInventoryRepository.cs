using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    public interface  IInventoryRepository
    {
        void RegisterInventory(string id, InventoryService inventoryService);
        InventoryService GetInventory(string id);
        bool UnregisterInventory(string id);
        IEnumerable<InventoryService> GetAllInventories();
    }
}
