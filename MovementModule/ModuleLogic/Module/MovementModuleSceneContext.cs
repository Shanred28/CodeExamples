using Base.ProjectScope;
using MovementModule.BusinessLogic;
using MovementModule.ModuleLogic.ServiceModule;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace MovementModule.ModuleLogic.Module
{
    public class MovementModuleSceneContext : SceneContext
    {
        [FormerlySerializedAs("movementModulePresenter")] [FormerlySerializedAs("movementPlayer")] [FormerlySerializedAs("player")] [SerializeField] private MovementModuleView movementModuleView;
        protected override void Configure(IContainerBuilder builder)
        {
            var movementConfigurator = new MovementConfigurator(builder);
            builder.RegisterInstance(movementConfigurator).AsSelf();

            builder.Register<MovementModulePresenter>(Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(movementModuleView, Lifetime.Singleton);
            
            builder.RegisterEntryPoint<StartMovementModule>();
            Debug.Log("Module loaded");
        }
    }
}
