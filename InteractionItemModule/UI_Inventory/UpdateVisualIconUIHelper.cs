using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public static class UpdateVisualIconUIHelper
    {
        public static void UpdatePositionAndScaleIcon(List<Vector2Int> positions, RectTransform rectTransformItem, RectTransform rectTransformIcon, float cellWidth, float cellHeight,int rotation)
        {
            int minX = positions.Min(pos => pos.x);
            int maxX = positions.Max(pos => pos.x);
            int minY = positions.Min(pos => pos.y);
            int maxY = positions.Max(pos => pos.y);
            
            float centerX = (minX + maxX + 1) * cellWidth/ 2f;
            float centerY = -((minY + maxY + 1) * cellHeight / 2f);
            
            rectTransformItem.anchoredPosition = new Vector2(centerX, centerY);
            
            int cellsWidth = maxX - minX + 1;
            int cellsHeight = maxY - minY + 1;
            rectTransformItem.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellsWidth * cellWidth);
            rectTransformItem.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellsHeight * cellHeight);
            

            float widthCompensation = 1f + 0.1f * (cellsWidth - 1);
            float heightCompensation = 1f + 0.1f * (cellsHeight - 1);
            rectTransformIcon.localScale = new Vector3(widthCompensation, heightCompensation, 1);
            
            rectTransformIcon.localEulerAngles = new Vector3(0, 0, -rotation * 90f);           
        }
    }
}
