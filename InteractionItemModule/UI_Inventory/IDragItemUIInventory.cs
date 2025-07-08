using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public interface IDragItemUIInventory
    {
        public void Initialize(UIDragGhostItem dragGhostPrefab, Transform dragLayer);

        public void BeginDrag(DragData dragData);
    }
}
