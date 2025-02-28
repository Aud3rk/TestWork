using Player;
using UnityEngine;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace Services
{
    public class UIService: MonoBehaviour, IUIService
    {
        [SerializeField] private Button buttonThrow;
        private PlayerInteraction _playerInteraction;
        private GameObject _objectCamera;

        [Inject]
        public void Construct(PlayerInteraction playerInteraction, SceneCameras sceneCameras)
        {
            _playerInteraction = playerInteraction;
            _objectCamera = sceneCameras.ObjectCamera.gameObject;
        }

        private void Start()
        {
            buttonThrow.onClick.AddListener(_playerInteraction.ShootObject);
        }

        public void DisableObjectInterface()
        {
            DisableObjectCamera();
            DisableTrowButton();
        }

        public void EnableObjectInterface()
        {
            EnableTrowButton();
            EnableObjectCamera();
        }
        private void EnableObjectCamera() => _objectCamera.SetActive(true);

        private void DisableObjectCamera() => _objectCamera.SetActive(false);

        private void EnableTrowButton() => buttonThrow.gameObject.SetActive(true);
        private void DisableTrowButton() => buttonThrow.gameObject.SetActive(false);
    }
    
}