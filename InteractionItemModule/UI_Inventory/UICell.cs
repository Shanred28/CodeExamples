using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class UICell : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color freeHighlightColor = Color.green;
        [SerializeField] private Color occupiedHighlightColor = Color.red;
    
        private Vector2Int _cellCoords;
        private UIInventory _inventoryUI;

        public void Initialize(UIInventory inventoryUI, Vector2Int cellCoords)
        {
            _inventoryUI = inventoryUI;
            _cellCoords = cellCoords;
            backgroundImage.color = defaultColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        { 
            _inventoryUI.BeginDrag(_cellCoords, eventData.position);
        }
    
        public void SetHighlight(bool highlight, bool isFree)
        {
            if (highlight)
                backgroundImage.color = isFree ? freeHighlightColor : occupiedHighlightColor;
            else
                backgroundImage.color = defaultColor;
        }

        public void DefaultHighlight()
        {
            backgroundImage.color = defaultColor;
        }
    }
}