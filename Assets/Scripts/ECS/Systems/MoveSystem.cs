using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
public class MoveSystem : IEcsInitSystem, IEcsRunSystem
{
    EcsFilter<MoveComponent, InputComponent> _moveFilter = null;

    public static Action OnEndTransition;

    private SceneData sceneData;

    public void Init()
    {
        OnEndTransition += EndTransition;
    }

    public void Run()
    {

        foreach (var entity in _moveFilter)
        {
            ref MoveComponent moveComponent = ref _moveFilter.Get1(entity);
            ref InputComponent inputComponent = ref _moveFilter.Get2(entity);
            Transform objectTransform = moveComponent.Character.transform;

            Vector3 direction = Vector3.zero;

            direction += HandleForwardMove(ref moveComponent, objectTransform);
            direction += HandleLineChange(ref moveComponent, objectTransform, ref inputComponent);
            direction += HandleGraviy(ref moveComponent, objectTransform);
            direction += HandleJump(ref moveComponent, objectTransform, ref inputComponent);
            moveComponent.Character.Move(direction * Time.deltaTime);
            // Debug.Log(moveComponent + "//" + moveComponent.Character);
        }


    }

    void EndTransition()
    {
        foreach (var entity in _moveFilter)
        {
            ref MoveComponent moveComponent = ref _moveFilter.Get1(entity);
            Transform objectTransform = moveComponent.Character.transform;

            moveComponent.TargetLinePosition = objectTransform.position;
        }
    }
    public Vector3 HandleGraviy(ref MoveComponent moveComponent, Transform objectTransform)
    {
        Vector3 direction = Vector3.zero;
        if (!moveComponent.Character.isGrounded)
        {
            direction += -objectTransform.up * moveComponent.GravityScale;
        }
        else moveComponent.JumpsCount = 2;
        return direction;
    }

    public Vector3 HandleLineChange(ref MoveComponent moveComponent, Transform objectTransform, ref InputComponent inputComponent)
    {
        Vector3 direction = Vector3.zero;
        // Debug.Log(moveComponent.TargetLineIndex);

        if (inputComponent.Horizontal != 0)
        {

            int targetHorizontalDirection = Mathf.RoundToInt(Mathf.Sign(inputComponent.Horizontal));
            int tempTargetIndex = moveComponent.TargetLineIndex + 1 * targetHorizontalDirection;
            if (tempTargetIndex >= -1 && tempTargetIndex <= 1 && moveComponent.CurrentHorizontalDirection != targetHorizontalDirection)
            {
                moveComponent.CurrentHorizontalDirection = targetHorizontalDirection;
                moveComponent.TargetLineIndex = tempTargetIndex;
                moveComponent.TargetLinePosition = (moveComponent.TargetLinePosition + objectTransform.right.normalized * moveComponent.CurrentHorizontalDirection * sceneData.DistanceBeetweenLines);
                moveComponent.TargetValue = (RotationEven()) ? moveComponent.TargetLinePosition.x : moveComponent.TargetLinePosition.z;
            }
        }
        if (moveComponent.CurrentHorizontalDirection != 0)
        {
            float currentPosition = (RotationEven()) ? objectTransform.position.x : objectTransform.position.z;
            if (Mathf.Abs(moveComponent.TargetValue - currentPosition) < 0.35f)
            {
                Vector3 objPos = objectTransform.position;
                moveComponent.Character.enabled = false;
                objectTransform.position = (RotationEven()) ? new Vector3(moveComponent.TargetValue, objPos.y, objPos.z) : new Vector3(objPos.x, objPos.y, moveComponent.TargetValue);
                moveComponent.Character.enabled = true;
                moveComponent.CurrentHorizontalDirection = 0;

            }
            else
            {
                direction += objectTransform.right * moveComponent.HorizontalSpeed * moveComponent.CurrentHorizontalDirection;
            }
            // Debug.Log($"targetvalue{moveComponent.TargetValue}, currentPosition{currentPosition} , posX{objectTransform.position}");

        }
        return direction;
    }

    public Vector3 HandleJump(ref MoveComponent moveComponent, Transform objectTransform, ref InputComponent inputComponent)
    {
        Vector3 direction = Vector3.zero;
        if (inputComponent.Vectical == 0 && moveComponent.JumpsCount > 0)
            moveComponent.CanJump = true;
        if (moveComponent.CanJump && inputComponent.Vectical > 0)
        {
            moveComponent.IsJumping = true;
            moveComponent.CanJump = false;
            moveComponent.JumpStartTime = 0;
            moveComponent.JumpsCount--;
            // Debug.Log(moveComponent.JumpsCount);
        }

        if (moveComponent.IsJumping == true)
        {
            if (moveComponent.JumpStartTime == 0)
            {
                moveComponent.JumpStartTime = Time.time;
            }
            float jumpEndTime = moveComponent.JumpStartTime + moveComponent.JumpDuration;
            if (Time.time > jumpEndTime)
            {
                moveComponent.IsJumping = false;
                return Vector3.zero;
            }
            float jumpCurveValue = moveComponent.JumpCurve.Evaluate((Time.time - moveComponent.JumpStartTime) / (jumpEndTime - moveComponent.JumpStartTime));
            direction += objectTransform.up * jumpCurveValue * moveComponent.JumpSpeed.Value;
            // Debug.Log(moveComponent.JumpSpeed.Value);
        }
        return direction;
    }

    public Vector3 HandleForwardMove(ref MoveComponent moveComponent, Transform objectTransform)
    {
        Vector3 direction = Vector3.zero;
        // Debug.Log(objectTransform.forward + "//" + moveComponent.ForwardSpeed.TargetValue + "//" + moveComponent.ForwardSpeed._modificator?.name);
        if (sceneData.IsGameStarted) direction = objectTransform.forward * moveComponent.ForwardSpeed.Value;
        return direction;
    }

    bool RotationEven()
    {
        return ((SceneData.currentRotation / 90) % 2) == 0;
    }
}
