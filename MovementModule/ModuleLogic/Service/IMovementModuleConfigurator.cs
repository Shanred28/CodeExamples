using UnityEngine;

namespace MovementModule.ModuleLogic.Service
{
    public interface IMovementModuleConfigurator<out T>
    {
        public T Configure(ScriptableObject scriptableObject);
    }
}
