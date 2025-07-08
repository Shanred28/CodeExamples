using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.GameService.InteractionItemModule.UI_Inventory
{
    public class UIItemViewer : MonoBehaviour
    {
        [SerializeField] private RectTransform  rectTransformItem;
        [SerializeField] private RectTransform  rectTransformIcon;
        [SerializeField] private Image icon;

        [SerializeField] private TMP_Text amountText;

        private ItemAtInventory _item;

        private UIInventory _uiInventory;
        
        private CanvasGroup _canvasGroup;
        
        public void InitializeDisplay(ItemAtInventory item, UIInventory uiInventory)
        {
            _item = item;
            _uiInventory = uiInventory;
            
            icon.sprite = _item.ItemSo.Icon;
            icon.preserveAspect = true;

            if (_item.ItemSo.IsStackable)
            {
                amountText.text = _item.AmountValue.ToString();
                amountText.gameObject.SetActive(true);
            }
            else
                amountText.gameObject.SetActive(false);
            
            UpdateUIPosition();
        }

        private void UpdateUIPosition()
        {
            UpdateVisualIconUIHelper.UpdatePositionAndScaleIcon(_item.Positions,rectTransformItem,rectTransformIcon,_uiInventory.CellWidth,_uiInventory.CellHeight,_item.Rotation);
        }
    }
}