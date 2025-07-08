using System;
using GamePlayLogic.GameService.InteractionItemModule.Service.SO;
using UnityEngine;

namespace GamePlayLogic.GameService.InteractionItemModule.Service
{
    public class RaycastConfigure
    {
        private readonly RaycastConfigureSO _raycastConfigureSo;

        public RaycastConfigure(RaycastConfigureSO raycastConfigureSo)
        {
            _raycastConfigureSo = raycastConfigureSo;
        }

        public RaycastService GetRaycastService()
        {
            if (_raycastConfigureSo is RaycastConfigureSO config)
            {
                Camera cam = config.mainCamera != null ? config.mainCamera : Camera.main;
                return new RaycastService(cam, config.interactableLayerMask, config.interactDistance);
            }
            throw new ArgumentException("Неверный тип объекта ScriptableObject. Ожидался RaycastConfigureSO.", nameof(_raycastConfigureSo));
        }
    }
}
