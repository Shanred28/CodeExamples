using System.Collections.Generic;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public class CellHighlighter : ICellHighlighter
    {
        public void Highlight(UIInventory inv, List<Vector2Int> cells, bool valid)
        {
            inv.CancelHighlightCells();
            inv.HighlightCells(cells, valid);
        }

        public void Clear(UIInventory inv)
        {
            inv.CancelHighlightCells();
        }
    }
}
