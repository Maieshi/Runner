using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class InputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    EcsFilter<InputComponent> inputFilter = null;

    MonoBehaviour coroutineManager;

    // public Action<Vector2, float> OnStartTouch;

    // public Action<Vector2, float> OnEndTouch;

    // public Action OnTapped;
    // public Action OnMultiTapped;

    // public Action OnLeftSwipe;
    // public Action OnRightSwipe;
    // public Action OnUpSwipe;
    // public Action OnDownSwipe;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _startTime;
    private float _endTime;

    private Vector2 screenPosition { get { return playerContronls.Touch.PrimaryPosition.ReadValue<Vector2>(); } }

    private NewControls playerContronls;

    private InputData _inputData;
    float Horizontal;
    float Vertical;

    Coroutine OnResetHorizontal;


    public void Init()
    {
        playerContronls = new NewControls();
        OnResetHorizontal = null;
        playerContronls.Enable();

        playerContronls.Touch.PrimaryContact.started += StartTouch;
        playerContronls.Touch.PrimaryContact.canceled += EndTouch;

        playerContronls.Key.Horizontal.started += KeyHorizontalStarted;
        playerContronls.Key.Horizontal.canceled += KeyHorizontalCancaled;

        playerContronls.Key.Jump.started += x => { Vertical = 1; };
        playerContronls.Key.Jump.canceled += x => { Vertical = 0; };
    }
    public void Run()
    {

        foreach (var inputEntity in inputFilter)
        {
            ref InputComponent inputComponent = ref inputFilter.Get1(inputEntity);
            inputComponent.Horizontal = Horizontal;
            inputComponent.Vectical = Vertical;
            // Debug.Log(inputComponent.Vectical + "//" + inputComponent.Horizontal);
        }
    }


    #region Key
    void KeyHorizontalStarted(InputAction.CallbackContext x)
    {
        Horizontal = x.ReadValue<float>();
    }
    void KeyHorizontalCancaled(InputAction.CallbackContext x)
    {
        Horizontal = 0;
    }

    void KeyVericalStarted(InputAction.CallbackContext x)
    {
        Vertical = 1;
    }
    void KeyVerticalCancaled()
    {
        Vertical = 0;
    }
    #endregion

    #region Touch
    private void StartTouch(InputAction.CallbackContext x)
    {
        Vertical = 0;
        Horizontal = 0;
        _startPosition = screenPosition;
        _startTime = (float)x.time;
        // Debug.Log(Vertical);
    }
    private void EndTouch(InputAction.CallbackContext x)
    {
        _endPosition = screenPosition; ;
        _endTime = (float)x.time;
        DetectSwipe();
        // Debug.Log(Vertical);
    }
    void DetectSwipe()
    {
        if ((_endTime - _startTime) < _inputData._maxTime)
        {
            if (Vector3.Magnitude(_endPosition - _startPosition) >= _inputData._minDistance)
            {

                Vector2 direction = new Vector2((_endPosition - _startPosition).x, (_endPosition - _startPosition).z).normalized;
                if (Vector2.Dot(direction, Vector2.right) > _inputData._directionThreshold)
                {
                    Horizontal = 1;
                }
                if (Vector2.Dot(direction, Vector2.left) > _inputData._directionThreshold)
                {
                    Horizontal = -1;
                }
                if (OnResetHorizontal != null)
                    coroutineManager.StopCoroutine(OnResetHorizontal);
                OnResetHorizontal = coroutineManager.StartCoroutine(ResetHorizontal());

            }
            else
            {
                Vertical = 1;
            }
        }
    }

    IEnumerator ResetHorizontal()
    {
        yield return new WaitForSeconds(0.1f);
        Horizontal = 0;
        OnResetHorizontal = null;
        // Debug.Log("reset");
    }

    public void Destroy()
    {
        playerContronls.Touch.PrimaryContact.started -= StartTouch;
        playerContronls.Touch.PrimaryContact.canceled -= EndTouch;

        playerContronls.Key.Horizontal.started -= KeyHorizontalStarted;
        playerContronls.Key.Horizontal.canceled -= KeyHorizontalCancaled;

        playerContronls.Key.Jump.started -= x => { Vertical = 1; };
        playerContronls.Key.Jump.canceled -= x => { Vertical = 0; };
        playerContronls.Dispose();

    }

    // private class CoroutineManager: MonoBehaviour
    // {

    // }
    #endregion
}
