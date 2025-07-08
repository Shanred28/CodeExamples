using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public interface IDropHandler
    {
        void HandleLocalDrop(ItemAtInventory item, InventoryService sourceInventory, UIInventory sourceUI,
            List<Vector2Int> targetCells);
        void HandleCrossInventoryDrop(ItemAtInventory item, UIInventory sourceUI, UIInventory targetUI, 
            List<Vector2Int> targetCells);
        void HandleThrowAway(ItemAtInventory item, InventoryService sourceInventory);
    }
}