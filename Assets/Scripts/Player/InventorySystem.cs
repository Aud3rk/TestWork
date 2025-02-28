using System.Collections.Generic;
using DefaultNamespace;

namespace Player
{
    public class InventorySystem
    {
        private List<IGrabable> collectedObjects = new List<IGrabable>();

        public void Collect(IGrabable hitedObject)
        {
            collectedObjects.Add(hitedObject);
        }

        public void UnCollect(IGrabable collectedObject)
        {
            collectedObjects.Remove(collectedObject);
        }
    }
}