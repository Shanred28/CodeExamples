using System.Collections.Generic;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    public class ItemAtInventory
    {
        public string UniqueID;
        public ItemSO ItemSo;
        public List<Vector2Int> Positions { get; set; }
        public int Rotation { get; set; }
        public int AmountStack { get; set; }
        public int AmountValue { get; set; }
    }
}