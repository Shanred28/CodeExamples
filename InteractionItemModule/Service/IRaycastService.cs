using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    public interface IRaycastService
    {
        void SetPointRaycast(Vector3 point);
        bool TryGetInteractable(out IInteractable interactable);
    }
}