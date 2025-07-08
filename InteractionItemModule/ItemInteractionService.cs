using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule
{
    public class ItemInteractionService 
    {
        private InventoryService _inventoryService;
        private HandService _handService;

        public ItemInteractionService(InventoryService inventoryService, HandService handService)
        {
            _inventoryService = inventoryService;
            _handService = handService;
        }
        
        /*public bool PickUpItem(Item item)
        {
            if (item.IsStorable)
            {
                return _inventoryService.AddItem(item);
            }
            else
            {
                return _handService.Equip(item);
            }
        }*/
    }
}
