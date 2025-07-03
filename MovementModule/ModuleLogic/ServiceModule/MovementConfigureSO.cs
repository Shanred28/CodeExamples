using UnityEngine;

namespace MovementModule.ModuleLogic.ServiceModule
{
    public enum TypeMovement
    {
        Movement2DPlatform,
        Movement2DTopDown,
        Movement3Dfps
    }

    public enum GroundedCheckType
    {
        CheckGround2dPoint,
        CheckGround2dCollider,
        CheckGround2dAny,
        CheckGround2dAll,
        CheckGround3dPoint,
        CheckGround3dCollider,
        CheckGround3dAny,
        CheckGround3dAll,
        CheckGroundNon
    }


    [CreateAssetMenu(fileName = "MovementConfiguratorSO", menuName = "MovementModule/MovementConfiguratorSO")]
    public class MovementConfigureSO : ScriptableObject
    {
        public MovementConfigureMove movementConfigureMove;
        public MovementConfigureRotate movementConfigureRotate;      
        public MovementConfigureGrounded movementConfigureGrounded;
        public MovementConfigureJumped movementConfigureJumped;
    }
}
