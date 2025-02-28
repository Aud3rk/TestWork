using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using DefaultNamespace;
using Player;
using Services;
using UnityEngine;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform characterTransform;

    [SerializeField] private float speedPosition;
    [SerializeField] private float speedrotationX;
    [SerializeField] private float speedrotationY;
    
    [SerializeField] private InputService inputService;
    [SerializeField] public Transform GrabPoint;
    
    private PlayerInteraction _playerInteraction;
    private float cameraRotatation;
    private float gravity=-9.8f;


    private void Start() 
        => SubscribeOnInputServiceEvents();

    private void OnDestroy() 
        => UnsubscribeFromInputServiceEvents();

    [Inject]
    public void Construct(PlayerInteraction playerInteraction)
    {
        _playerInteraction = playerInteraction;
        playerInteraction._grabPoint = GrabPoint;
    }

    private void FixedUpdate() 
        => UpdateCharacterController();

    private void UnsubscribeFromInputServiceEvents()
    {
        inputService.ChangedDirection -= InputService_OnChangedDirection;
        inputService.ChangedRotation -= InputService_OnChangedRotation;
        inputService.TapScreen -= InputService_OnTapScreen;
    }

    private void SubscribeOnInputServiceEvents()
    {
        inputService.ChangedDirection += InputService_OnChangedDirection;
        inputService.ChangedRotation += InputService_OnChangedRotation;
        inputService.TapScreen += InputService_OnTapScreen;
    }

    private void UpdateCharacterController()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdatePosition() 
        => inputService.OnChangedMoveDirection();

    private void UpdateRotation() 
        => inputService.OnChangedRotateDirection();

    private void InputService_OnChangedDirection(Vector2 direction)
        => characterController.Move(
            (characterTransform.forward * direction.y + characterTransform.right * direction.x+characterTransform.up*gravity).normalized
            * speedPosition * Time.deltaTime);

    
    private void InputService_OnChangedRotation(Vector2 direction)
    {
        characterTransform.rotation *= Quaternion.Euler((Vector3.up * direction.x) * speedrotationX);
        cameraRotatation = Mathf.Clamp(cameraRotatation - direction.y * speedrotationY, -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(cameraRotatation, 0, 0);
    }

    private void InputService_OnTapScreen(Vector2 position)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out hit) && hit.transform.TryGetComponent(out IGrabable hitedObject)
                                          &&!_playerInteraction.IsInteactedWithObject)
        {
            _playerInteraction.TakeObject(hitedObject);
        }
    }
}