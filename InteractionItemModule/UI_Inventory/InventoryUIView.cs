using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using Lean.Pool;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class InventoryUIView : MonoBehaviour
    {
        public UIDragGhostItem DragPrefab => dragPrefab;
        public Transform TransformCanvas => canvas.transform;
        
        [Header("UI Prefabs & Layouts")] 
        [SerializeField] private InventoryPointHolder[] layoutPrefabs; 
        [SerializeField] private UIDragGhostItem dragPrefab;
        [SerializeField] private Canvas canvas;

        private InventoryPointHolder  _currentLayout;
        private Transform _canvasTransform;
        

        private void Start()
        {
            _canvasTransform = canvas.transform;
        }

        public void OpenInventory(IInventoryInteraction inventoryInteraction, string[] inventoryNames)
        {
            if (_currentLayout != null)
                LeanPool.Despawn(_currentLayout.gameObject);

            int count = inventoryNames.Length;
            var layoutGo = LeanPool.Spawn(layoutPrefabs[count - 1].gameObject, _canvasTransform, false);
            _currentLayout = layoutGo.GetComponent<InventoryPointHolder>();

            for (int i = 0; i < count; i++)
            {
                SpawnWindow(inventoryInteraction, inventoryNames[i], _currentLayout.slots[i]);
            }
        }
        
        public void CloseAll()
        {
            if (_currentLayout != null)
            {
                LeanPool.Despawn(_currentLayout.gameObject);
                _currentLayout = null;
            }
        }
        
        private void SpawnWindow(IInventoryInteraction interaction, string inventoryName, RectTransform slot)
        {
            var uiWindowGo =LeanPool.Spawn(interaction.GetInventoryService(inventoryName).GetInventorySetting().uiInventoryPrefab, slot, false);
            var rt = uiWindowGo.transform as RectTransform;
            rt.SetParent(slot, false);
            
            ResetDefault(rt);

            uiWindowGo.Initialize(interaction, inventoryName);
            uiWindowGo.ToggleInventory();
        }

        private void ResetDefault(RectTransform rt)
        {
            rt.anchorMin     = Vector2.zero;
            rt.anchorMax     = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta     = Vector2.zero;
            rt.localScale    = Vector3.one;
        }
    }
}