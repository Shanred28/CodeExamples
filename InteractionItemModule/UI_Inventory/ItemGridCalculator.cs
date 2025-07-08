using System.Collections.Generic;
using System.Linq;
using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public static class ItemGridCalculator
    {
        public static List<Vector2Int> CalculateOccupiedCells(List<Vector2Int> baseShape, int rotationSteps,
            Vector2Int gridCell)
        {
            List<Vector2Int> rotatedShape = NormalizedShapeHelper.GetRotatedNormalizedShape(baseShape, rotationSteps);
            int minX = rotatedShape.Min(c => c.x);
            int minY = rotatedShape.Min(c => c.y);
            return rotatedShape.Select(rel => new Vector2Int(gridCell.x + (rel.x - minX),
                gridCell.y + (rel.y - minY))).ToList();
        }
    }
}