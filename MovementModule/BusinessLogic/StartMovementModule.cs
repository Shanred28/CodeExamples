
using System;
using MovementModule.ModuleLogic.ServiceModule;
using VContainer;
using VContainer.Unity;

namespace MovementModule.BusinessLogic
{
    public class StartMovementModule: IStartable, IDisposable
    {
        private readonly IObjectResolver _container;
        private MovementModuleView _movementModuleView;
        
        public StartMovementModule(IObjectResolver container, MovementModuleView movementModuleView)
        {
            _container = container;
            _movementModuleView = movementModuleView;
        }
        
        public void Start()
        {
            
        }

        public void Dispose()
        {
            _movementModuleView = null;
            _container?.Dispose();
        }
    }
}
