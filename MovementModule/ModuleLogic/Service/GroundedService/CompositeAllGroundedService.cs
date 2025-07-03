using System.Collections.Generic;
using UnityEngine;

namespace MovementModule.ModuleLogic.Service.GroundedService
{
    public class CompositeAllGroundedService : IIsGroundedService
    {
        private readonly IEnumerable<IIsGroundedService> _groundedServices;

        public CompositeAllGroundedService(IEnumerable<IIsGroundedService> groundedServices)
        {
            _groundedServices = groundedServices;
        }

        public void SetGroundCheckPoint(Transform groundCheckPoint)
        {
            Debug.Log("Service is not configured in SO.");
        }

        public bool IsGrounded()
        {
            foreach (var service in _groundedServices)
            {
                if (!service.IsGrounded())
                    return false;
            }
            return true;
        }
    }
}