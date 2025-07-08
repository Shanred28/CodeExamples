using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory.DragItemModule
{
    public class GhostView : IGhostView
    {
        private readonly UIDragGhostItem _prefab;
        private readonly Transform _parentLayer;
        private UIDragGhostItem _instance;
        private RectTransform _instanceRect;
        
        public GhostView(UIDragGhostItem prefab, Transform parentLayer)
        {
            _prefab = prefab;
            _parentLayer = parentLayer;
        }
    
        public void Show(Sprite icon, List<Vector2Int> cells, UIInventory inv, int rot, Vector2 worldPos)
        {
            if (_instance == null)
            {
                _instance = LeanPool.Spawn(_prefab, _parentLayer);
                _instanceRect = _instance.GetComponent<RectTransform>();
            }
            _instance.VisualUpdate(cells, inv, icon, rot);
            _instance.transform.position = worldPos;
            _instance.gameObject.SetActive(true);
        }
    
        public void MoveTo(Vector2 screenPosition)
        {
            if (_instance != null)
                _instance.transform.position = screenPosition;
        }
    
        public void Hide()
        {
            if (_instance != null)
            {
                LeanPool.Despawn(_instance);
                _instance = null;
                _instanceRect = null;
            }
        }
    }
}
