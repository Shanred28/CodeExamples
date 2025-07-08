using System;
using BaseInfrastructure.Ticker.Interfaces;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public interface  IDragManager : IUpdateable
    {
        void BeginDrag(DragData data);
        void CancelDrag();
        event Action<DragData> OnDragStarted;
        event Action<Vector2> OnDragging;
        event Action<Vector2> OnDropped;
        event Action OnRotate;
    }
}
