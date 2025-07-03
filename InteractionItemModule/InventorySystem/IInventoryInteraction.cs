using System;
using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using GamePlayLogic.GameService.InteractionItemModule.UI_Inventory;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    public interface IInventoryInteraction
    {
        public event Action OnInventoryUpdated;
        List<ItemAtInventory> GetAllItems(string inventoryId);
        public void AddItem(ItemSO itemSo);
        void OpenInventory(string[] inventoryId);

        void CloseInventory();
        public InventoryService GetInventoryService(string inventoryId);

        public void OnStartDragItem(DragData dragData);

        public ItemAtInventory GetItemAtCell(string nameInventory ,Vector2Int cell);
        
        public bool MoveItemBetweenInventories(string sourceInventoryId, string targetInventoryId, string itemUniqueId,
            List<Vector2Int> targetCells);
    }
}
