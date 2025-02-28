using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Grabable : MonoBehaviour, IGrabable
    {
        private readonly WaitForSeconds FIXED_UPDATE_STEP = new WaitForSeconds(0.002f);
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Collider collider;
        [SerializeField] private float speedMoving;
        [SerializeField] private float forcePower;
        private Coroutine _smoothChangePositionRoutine;

        public void Grab(Transform grabTransform)
        {
            rigidbody.useGravity = false;
            collider.enabled = false;
            rigidbody.isKinematic = true;
            gameObject.layer = 6;
            for(int i=0;i<transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = 6;
            }
            StartSmoothChangePositionRoutine(grabTransform);
        }

        public void UnGrab()
        {
            StopSmoothChangePositionRoutine();
        }
        private void StartSmoothChangePositionRoutine(Transform transform)
        {
            if (_smoothChangePositionRoutine == null)
                _smoothChangePositionRoutine = StartCoroutine(SmoothChangePosition(transform));
        }

        private void StopSmoothChangePositionRoutine()
        {
            StopCoroutine(_smoothChangePositionRoutine);
            _smoothChangePositionRoutine = null;
            rigidbody.useGravity = true;
            collider.enabled = true;
            rigidbody.isKinematic = false;
            gameObject.layer = 0;
            for(int i=0;i<transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = 0;
            }
            rigidbody.AddForce( transform.forward*forcePower, ForceMode.Impulse);
        }


        private IEnumerator SmoothChangePosition(Transform transform)
        {
            while (true)
            {
                rigidbody.MovePosition(Vector3.MoveTowards(rigidbody.position,transform.position, Time.deltaTime*speedMoving));
                rigidbody.MoveRotation(transform.rotation);
                yield return FIXED_UPDATE_STEP;
            }
        }
    }
}