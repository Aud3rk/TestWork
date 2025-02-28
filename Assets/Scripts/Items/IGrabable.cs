using UnityEngine;

namespace DefaultNamespace
{
    public interface IGrabable
    {
        public void Grab(Transform grabPoint);
        public void UnGrab();
        
    }
}