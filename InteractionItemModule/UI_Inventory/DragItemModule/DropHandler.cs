using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using GamePlayLogic.GameService.Spawners.ItemSpawnWorldService;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public class DropHandler : IDropHandler
    {
        private readonly IInventoryInteraction _inventoryInteraction;
        private readonly IItemSpawnService _itemSpawnService;
        
        public DropHandler(IInventoryInteraction inventoryInteraction, IItemSpawnService itemSpawnService)
        {
            _inventoryInteraction = inventoryInteraction;
            _itemSpawnService     = itemSpawnService;
        }
        
        public void HandleLocalDrop(ItemAtInventory item, InventoryService sourceInventory, UIInventory sourceUI,
            List<Vector2Int> targetCells)
        {
            if (sourceInventory.CanPlaceItemAt(item, targetCells))
            {
                sourceInventory.UpdateItemPlace(item.UniqueID, targetCells, item.Rotation);
                sourceUI.UpdateDisplay();
            }
            else
            {
                sourceUI.CancelHighlightCells();
                Debug.Log("Невозможно разместить предмет в выбранной позиции.");
            }
        }
        
        public void HandleCrossInventoryDrop(ItemAtInventory item, UIInventory sourceUI, UIInventory targetUI,
            List<Vector2Int> targetCells)
        {
            bool moved = _inventoryInteraction.MoveItemBetweenInventories(sourceUI.InventoryName, targetUI.InventoryName,
                item.UniqueID, targetCells);

            targetUI.UpdateDisplay();
            if (!moved)
                Debug.Log("Перемещение между инвентарями не удалось.");

            sourceUI.CancelHighlightCells();
            targetUI.CancelHighlightCells();
        }
        
        public void HandleThrowAway(ItemAtInventory item, InventoryService sourceInventory)
        {
            sourceInventory.RemoveItem(item.UniqueID);
            _itemSpawnService.Drop(DropRequest.NearPlayer(item.ItemSo));
            Debug.Log($"Предмет {item.ItemSo.name} выброшен в мир.");
        }
    }
}
