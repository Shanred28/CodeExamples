using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public class GroundTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        private int _groundContacts = 0;
    
        public bool IsGrounded => _groundContacts > 0;

        private void OnTriggerEnter(Collider other)
        {
            if ((groundLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                _groundContacts++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((groundLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                _groundContacts = Mathf.Max(_groundContacts - 1, 0);
            }
        }
    }
}
