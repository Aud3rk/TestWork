using System;
using DefaultNamespace;
using Services;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInteraction: IDisposable
    {
        public bool IsInteactedWithObject;
        public event Action<IGrabable> ObjectTook;
        public event Action<IGrabable> ObjectThrow;

        public Transform _grabPoint;
        private IUIService _uiService;
        private IGrabable _interectedObject;
        private InventorySystem _inventorySystem;

        
        public PlayerInteraction()
        {
            _inventorySystem = new InventorySystem();
        }

        [Inject]
        public void Construct(IUIService uiService)
        {
            SubscribeInventorySystem();
            _uiService = uiService;
        }

        private void SubscribeInventorySystem()
        {
            ObjectTook += _inventorySystem.Collect;
            ObjectThrow += _inventorySystem.UnCollect;
        }
        
        private void UnsubscribeInventorySystem()
        {
            ObjectTook -= _inventorySystem.Collect;
            ObjectThrow -= _inventorySystem.UnCollect;
        }

        public void TakeObject(IGrabable takenObject)
        {
            IsInteactedWithObject = true;
            takenObject.Grab(_grabPoint);
            _interectedObject = takenObject;
            _uiService.EnableObjectInterface();
            ObjectTook.Invoke(takenObject);
            
        }

        public void ShootObject()
        {
            IsInteactedWithObject = false;
            _interectedObject.UnGrab();
            _uiService.DisableObjectInterface();
            ObjectThrow.Invoke(_interectedObject);
            _interectedObject = null;
        }

        public void Dispose()
        {
            UnsubscribeInventorySystem();
        }
    }
}