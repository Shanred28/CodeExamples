using UnityEngine;

namespace MovementModule.ModuleLogic.ServiceModule
{
    [CreateAssetMenu(fileName = "JumpConfigSO", menuName = "MovementModule/JumpConfig")]
    public class MovementConfigureJumped: ScriptableObject
    {
        [Header("Jump")]
        public TypeMovement movementType;
        public float jumpForce = 1.0f;
        public float gravity = 9.81f;
    }
}