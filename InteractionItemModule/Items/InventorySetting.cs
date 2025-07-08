using GamePlayLogic.GameService.InteractionItemModule.UI_Inventory;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Items
{
    [CreateAssetMenu (fileName = "InventoryListItemSO", menuName = "InventoryListItemSO")]
    public class InventorySetting : ScriptableObject
    {
        public int columns, rows;
        public string nameInventory;
        public UIInventory uiInventoryPrefab;
        public ItemSO[] ItemsSo;
    }
}
