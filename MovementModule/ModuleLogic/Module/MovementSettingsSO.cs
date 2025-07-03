using UnityEngine;
using UnityEngine.Serialization;

namespace MovementModule.ModuleLogic
{
    public enum MovementType
    {
        TwoD,
        ThreeD,
        Isometric
    }
    
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Movement/Movement Settings", order = 1)]
    public class MovementSettingsSo : ScriptableObject
    {
        public bool canJump;
        
        public MovementType movementType = MovementType.ThreeD;
       
        public float acceleration = 10f;
        public float deceleration = 5f;
        
        public float walkingSpeed = 5f;
        public float runningSpeed = 10f;
        public float crouchingSpeed = 1f;
        public float slidingSpeed = 7f;
        
        public float jumpForce = 7f;
        public float gravity = 9.81f;
    }
}
