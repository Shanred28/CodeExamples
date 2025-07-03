using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class DragData
    {
        public ItemAtInventory DraggedItem { get; set; }
        public InventoryService SourceInventory { get; set; }
        public UIInventory SourceUI { get; set; }
        public Sprite IconSprite { get; set; }
        public Vector2 InitialPosition { get; set; }
    }
}