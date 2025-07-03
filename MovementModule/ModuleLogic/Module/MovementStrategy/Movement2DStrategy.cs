using UnityEngine;

namespace MovementModule.ModuleLogic.MovementStrategy
{
    public class Movement2DStrategy : MovementStrategy, IMovementStrategy
    {
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition, float deltaTime)
        {
            float newX = Mathf.MoveTowards(currentPosition.x, targetPosition.x, GetSpeed() * deltaTime);
            
            float newY = currentPosition.y;
        
            newY = JumpingY(deltaTime, newY);
            
            return new Vector3(newX, newY, currentPosition.z);
        }
    }
}
