using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
public class CameraRotationaSystem : IEcsRunSystem
{

    private PlayerData _playerData;
    EcsFilter<InputComponent, CameraRotationComponent> _cameraRotationFilter = null;
    public void Run()
    {
        foreach (var entity in _cameraRotationFilter)
        {
            ref InputComponent inputComponent = ref _cameraRotationFilter.Get1(entity);
            ref CameraRotationComponent cameraRotationComponent = ref _cameraRotationFilter.Get2(entity);

            cameraRotationComponent.XRotation += inputComponent.RotationDirection.x * _playerData.PlayerCameraSensitivity;
            cameraRotationComponent.YRotation += inputComponent.RotationDirection.y * _playerData.PlayerCameraSensitivity;
            cameraRotationComponent.YRotation = Mathf.Clamp(cameraRotationComponent.YRotation, -90, 90);

            cameraRotationComponent.Camera.transform.localRotation = Quaternion.Euler(-cameraRotationComponent.YRotation, 0, 0);
            cameraRotationComponent.CameraParent.transform.rotation = Quaternion.Euler(0, cameraRotationComponent.XRotation, 0);
        }
    }
}
