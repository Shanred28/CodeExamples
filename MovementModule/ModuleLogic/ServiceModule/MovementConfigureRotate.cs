using UnityEngine;

namespace MovementModule.ModuleLogic.ServiceModule
{
    [CreateAssetMenu(fileName = "RotateConfigSO", menuName = "MovementModule/RotateConfig")]
    public class MovementConfigureRotate : ScriptableObject
    {
        [Header("Rotate")] 
        public TypeMovement typeMovement;
        public float rotationSpeed;
    }
}