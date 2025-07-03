using UnityEngine;
using VContainer;

namespace MovementModule.BusinessLogic
{
    public class MovementModuleView : MonoBehaviour
    {
        [SerializeField] protected Transform groundCheckPoint;
        private MovementModulePresenter _presenter;

        [Inject]
        public void Construct(MovementModulePresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _presenter.Init(transform,groundCheckPoint);
        }

        private void Update()
        {
            _presenter.Move();
        }
    }
}
