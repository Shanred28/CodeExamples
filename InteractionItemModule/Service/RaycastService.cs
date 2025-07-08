using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    public class RaycastService : IRaycastService
    {
        private readonly Camera _mainCamera;
        private readonly LayerMask _interactableLayerMask;
        private readonly float _interactDistance;
        private Vector2 _screenPoint;

        public RaycastService(Camera mainCamera, LayerMask interactableLayerMask, float interactDistance)
        {
            _mainCamera = mainCamera;
            _interactableLayerMask = interactableLayerMask;
            _interactDistance = interactDistance;
        }

        public void SetPointRaycast(Vector3 point)
        {
            _screenPoint = new Vector2(point.x, point.y);
        }

        public bool TryGetInteractable(out IInteractable interactable)
        {
            interactable = null;
            Ray ray = _mainCamera.ScreenPointToRay(_screenPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance, _interactableLayerMask))
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                return interactable != null;
            }
            return false;
        }
    }
}
