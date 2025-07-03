using UnityEngine;

namespace MovementModule.ModuleLogic.ServiceModule
{
    [CreateAssetMenu(fileName = "MoveConfigSO", menuName = "MovementModule/MoveConfig")]
    public class MovementConfigureMove : ScriptableObject
    {
        public TypeMovement movementType;
        public float speed = 1.0f;
    }
}