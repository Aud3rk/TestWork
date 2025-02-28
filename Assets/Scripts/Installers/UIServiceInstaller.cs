using DefaultNamespace;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIServiceInstaller : MonoInstaller
    {
        [SerializeField] private UIService uiService;

        public override void InstallBindings()
        {
            Container.Bind<IUIService>().FromInstance(uiService).AsSingle();
            
        }
    }
}