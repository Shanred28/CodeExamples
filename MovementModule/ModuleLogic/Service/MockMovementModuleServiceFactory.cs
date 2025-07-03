using System;
using System.Linq;
using System.Reflection;

namespace MovementModule.ModuleLogic.Service
{
    public class MockMovementModuleServiceFactory
    {
        public object CreateMockService(Type serviceType)
        {
            var mockType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.Name == $"Mock{serviceType.Name}");
            
            return Activator.CreateInstance(mockType ?? throw new InvalidOperationException($"Mock type for {serviceType.Name} not found"));
        }
    }
}