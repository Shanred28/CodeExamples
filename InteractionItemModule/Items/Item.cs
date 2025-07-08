using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Items
{
    public class Item : MonoBehaviour
    {
         [SerializeField] private ItemSO itemSO;

         public ItemSO ItemSO => itemSO;

         public void Initialize(ItemSO itemSo)
         {
             itemSO = itemSo;
         }
    }
}
