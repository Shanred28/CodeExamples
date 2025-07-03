using UnityEngine;

namespace MovementModule.ModuleLogic.MovementStrategy
{
    public class Movement3DStrategy : MovementStrategy, IMovementStrategy
    {
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition, float deltaTime)
        {
            Vector3 horizontalCurrent = new Vector3(currentPosition.x, 0f, currentPosition.z);
            Vector3 horizontalTarget = new Vector3(targetPosition.x, 0f, targetPosition.z);

            Vector3 horizontalNewPos = Vector3.MoveTowards(horizontalCurrent, horizontalTarget, GetSpeed() * deltaTime);
            
            float newY = currentPosition.y;
            newY = JumpingY(deltaTime, newY);
            
            return new Vector3(horizontalNewPos.x, newY, horizontalNewPos.z);
        }
    }
}
