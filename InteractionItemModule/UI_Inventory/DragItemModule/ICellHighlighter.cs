
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public interface ICellHighlighter
    {
        void Highlight(UIInventory inventory, List<Vector2Int> cells, bool valid);
        
        void Clear(UIInventory inventory);
    }
}
