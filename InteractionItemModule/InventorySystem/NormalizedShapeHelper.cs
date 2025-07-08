using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    public static class NormalizedShapeHelper
    {
        public static List<Vector2Int> GetRotatedNormalizedShape(List<Vector2Int> shape, int rotationSteps)
        {
            if (shape == null || shape.Count == 0)
                return new List<Vector2Int>();

            List<Vector2Int> rotated = new List<Vector2Int>(shape);
            for (int i = 0; i < rotationSteps; i++)
            {
                rotated = rotated.Select(p => new Vector2Int(p.y, -p.x)).ToList();
            }

            return NormalizeShape(rotated);
        }

        private static List<Vector2Int> NormalizeShape(List<Vector2Int> shape)
        {
            int minX = shape.Min(p => p.x);
            int minY = shape.Min(p => p.y);
            return shape.Select(p => new Vector2Int(p.x - minX, p.y - minY)).ToList();
        }
    }
}