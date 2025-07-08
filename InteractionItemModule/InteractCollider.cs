using System;
using GamePlayLogic.GameService.InteractionItemModule.Items;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule
{
    public class InteractCollider : MonoBehaviour
    {
        public event Action<ItemSO> OnInteract;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Item interactItem))
            {
                OnInteract?.Invoke(interactItem.ItemSO);
                Destroy(interactItem.gameObject);
            }
        }
    }
}
