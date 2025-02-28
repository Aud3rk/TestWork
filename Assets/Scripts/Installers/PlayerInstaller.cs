using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject characterPlayer;
        public override void InstallBindings()
        { 
            Container.Bind<SceneCameras>().FromInstance(characterPlayer.GetComponent<SceneCameras>()).AsSingle();
            Container.Bind<PlayerInteraction>().FromNew().AsSingle();

        }
    }
}