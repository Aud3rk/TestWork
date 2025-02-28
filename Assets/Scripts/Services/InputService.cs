using System;
using UnityEngine;

namespace Services
{
    public class InputService : MonoBehaviour
    {
        public event Action<Vector2> ChangedDirection;
        public event Action<Vector2> ChangedRotation;
        public event Action<Vector2> TapScreen;
        [SerializeField] private FixedJoystick joystick;
        
        private Vector2 lastMousePos;
        private float rightFingerId=-1;

        public void OnChangedMoveDirection()
            => ChangedDirection.Invoke(joystick.Direction);


        public void OnChangedRotateDirection()
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    switch (touch.phase)
                    {
                            
                        case TouchPhase.Began:
                            if (touch.position.x > Screen.width / 2 && rightFingerId == -1)
                            {
                                rightFingerId = touch.fingerId;
                            }
                            TapScreen?.Invoke(touch.position);
                            
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            if (rightFingerId == touch.fingerId)
                                rightFingerId = -1;
                            break;
                        case TouchPhase.Moved:
                            if (rightFingerId == touch.fingerId)
                                ChangedRotation.Invoke(new Vector2(touch.deltaPosition.x, touch.deltaPosition.y));
                            break;

                    }
                    

                }
            }

            else if (Input.GetMouseButton(0)) 
            {
                Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePos;
                
                ChangedRotation.Invoke(mouseDelta);
            }
            lastMousePos = Input.mousePosition; 
            
        }
        
        
    }
}