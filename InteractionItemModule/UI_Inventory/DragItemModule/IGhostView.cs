using System.Collections.Generic;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public interface IGhostView 
    {
        void Show(Sprite icon, List<Vector2Int> cells, UIInventory inventory, int rotation, Vector2 worldPosition);
        void MoveTo(Vector2 screenPosition);
        void Hide();
    }
}
