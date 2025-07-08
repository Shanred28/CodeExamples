using System.Collections.Generic;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.InventorySystem
{
    public class InventoryGrid
    {
        private readonly string[,] _grid;

        private readonly int _width;
        private readonly int _height;

        public InventoryGrid(int width, int height)
        {
            _width = width;
            _height = height;
            _grid = new string[width, height];
        }

        public bool IsCellFree(Vector2Int pos, string currentItemId = null)
        {
            if (!IsInside(pos))
                return false;
            string occupant = _grid[pos.x, pos.y];
            return string.IsNullOrEmpty(occupant) || occupant == currentItemId;
        }

        public bool CanPlaceShape(List<Vector2Int> shape, Vector2Int start)
        {
            foreach (var cell in shape)
            {
                var pos = new Vector2Int(start.x + cell.x, start.y + cell.y);
                if (!IsInside(pos) || !string.IsNullOrEmpty(_grid[pos.x, pos.y]))
                    return false;
            }

            return true;
        }

        public List<Vector2Int> PlaceShape(string itemId, List<Vector2Int> shape, Vector2Int start)
        {
            var occupied = new List<Vector2Int>();
            foreach (var cell in shape)
            {
                var pos = new Vector2Int(start.x + cell.x, start.y + cell.y);
                _grid[pos.x, pos.y] = itemId;
                occupied.Add(pos);
            }

            return occupied;
        }

        public void ClearCell(Vector2Int pos, string itemId)
        {
            if (IsInside(pos) && _grid[pos.x, pos.y] == itemId)
            {
                _grid[pos.x, pos.y] = null;
            }
        }

        private bool IsInside(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
        }

        public void SetCell(Vector2Int pos, string itemId)
        {
            if (IsInside(pos))
                _grid[pos.x, pos.y] = itemId;
        }

        public string GetCell(Vector2Int pos)
        {
            if (IsInside(pos))
                return _grid[pos.x, pos.y];
            return null;
        }
    }
}