using GamePlayLogic.GameService.InteractionItemModule.Items;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule
{
    public class HandService
    {
        public Item EquippedItem { get; private set; }
        
        public bool Equip(Item item)
        {
            if (EquippedItem == null)
            {
                EquippedItem = item;
                return true;
            }
            return false;
        }
        
        public void ClearHands()
        {
            EquippedItem = null;
        }
    }
}
