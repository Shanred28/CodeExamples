using UnityEngine;

namespace MovementModule.ModuleLogic.ServiceModule
{
    [CreateAssetMenu(fileName = "GroundedConfigSO", menuName = "MovementModule/GroundedConfig")]
    public class MovementConfigureGrounded : ScriptableObject
    {
        [Header("IsGrounded")] public GroundedCheckType groundedCheckType;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;
    }
}