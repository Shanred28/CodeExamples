using UnityEngine;

namespace MovementModule.ModuleLogic
{
    public class MovementRequestInput 
    {
        public Vector3 CurrentPosition { get; set; }
        public Vector2 MovementInput { get; set; }
        public float DeltaTime { get; set; }
    }
}
