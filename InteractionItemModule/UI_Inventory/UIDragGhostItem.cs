using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class UIDragGhostItem : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image image;

        public void VisualUpdate( List<Vector2Int> positions, UIInventory inventory, Sprite sprite, int rotation)
        {
            image.sprite = sprite;
            UpdateVisualIconUIHelper.UpdatePositionAndScaleIcon(positions,rectTransform,rectTransform,inventory.CellWidth ,inventory.CellHeight,rotation);
        }
    }
}
