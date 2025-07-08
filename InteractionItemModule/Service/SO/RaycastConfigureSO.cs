using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Service.SO
{
    [CreateAssetMenu(fileName = "RaycastConfigure", menuName = "Configurations/Raycast Configure")]
    public class RaycastConfigureSO : ScriptableObject
    {
        public Camera mainCamera;
        public LayerMask interactableLayerMask;
        public float interactDistance = 3f; 
    }
}
