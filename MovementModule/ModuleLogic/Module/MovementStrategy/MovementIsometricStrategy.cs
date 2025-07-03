using UnityEngine;

namespace MovementModule.ModuleLogic.MovementStrategy
{
    public class MovementIsometricStrategy : MovementStrategy, IMovementStrategy
    {
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition, float deltaTime)
        {
            Vector2 current2D = new Vector2(currentPosition.x, currentPosition.y);
            Vector2 target2D = new Vector2(targetPosition.x, targetPosition.y);
            Vector2 moved2D = Vector2.MoveTowards(current2D, target2D, GetSpeed() * deltaTime);
            
            float isoX = moved2D.x - moved2D.y;
            float isoY = (moved2D.x + moved2D.y) / 2;
            
            float newZ = currentPosition.z;
            newZ = JumpingY(deltaTime, newZ);
           
            return new Vector3(isoX, isoY, newZ);
        }
    }
}
