using System;
using MovementModule.ModuleLogic.Service;
using MovementModule.ModuleLogic.Service.GroundedService;
using MovementModule.ModuleLogic.Service.Move;
using UnityEngine;
using VContainer;

namespace MovementModule.ModuleLogic.ServiceModule
{
    public class MovementConfigurator
    {
        private const string CONFIG_MOVEMENT_SETTING = "Configs/MovementModuleConfig/MovementConfiguratorSO";
        
        private readonly MovementConfigureSO _movementConfigureSo;
        private readonly IContainerBuilder _builder;

        public MovementConfigurator(IContainerBuilder builder)
        {
            try
            {
                _movementConfigureSo = Resources.Load<MovementConfigureSO>(CONFIG_MOVEMENT_SETTING);
        
                if (_movementConfigureSo == null)
                {
                    Debug.LogError("MovementConfigurateSO could not be loaded");
                }
                else
                {
                    Debug.Log("MovementConfigurateSO loaded");
                }

                _builder = builder;
                CreateAndRegisterService();

            }
            catch (Exception ex)
            {
                Debug.LogError("Error initializing MovementConfigurator: " + ex.Message);
            }
        }

        private void CreateAndRegisterService()
        {
            RegisterService(
                _movementConfigureSo.movementConfigureMove,
                "Movement service",
                config => new MoveConfigure().Configure(config),
                typeof(IMoveService)
            );

            RegisterService(
                _movementConfigureSo.movementConfigureGrounded,
                "Grounded service",
                config => new GroundedConfigure().Configure(config),
                typeof(IIsGroundedService)
            );

            RegisterService(
                _movementConfigureSo.movementConfigureRotate,
                "Rotate service",
                config => new RotateConfigure().Configure(config), 
                typeof(IRotateService)
                );
            
            RegisterService(_movementConfigureSo.movementConfigureJumped,
                "Jump service", 
                config => new JumpConfigure().Configure(config), 
                typeof(IJumpService));
        }
        
        private void RegisterService<TConfig, TService>(TConfig config, string serviceName, Func<TConfig, TService> configureFunc, Type serviceInterface)
        {
            if (config != null)
            {
                var serviceInstance = configureFunc(config);
                _builder.RegisterInstance(serviceInstance).As(serviceInterface);
            }
            else
            {
                var mockService = new MockMovementModuleServiceFactory().CreateMockService(serviceInterface);
                _builder.RegisterInstance(mockService).As(serviceInterface);
                Debug.Log($"{serviceName} is not configured in So. Mock service is used");
            }
        }
    }
}
