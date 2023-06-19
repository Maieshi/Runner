using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using UnityEngine.UI;
public class GameInitSystem : IEcsInitSystem
{
    private
    EcsWorld _world;

    private SceneData _sceneData;
    private PlayerData _playerData;

    private UISpawnData _uiData;

    public WeaponData WeaponData;
    public void Init()
    {
        EcsEntity playerEntity = _world.NewEntity();
        ref AnimationComponent animationComponent = ref playerEntity.Get<AnimationComponent>();
        ref MoveComponent moveComponent = ref playerEntity.Get<MoveComponent>();
        // ref CameraRotationComponent cameraRotationComponent = ref playerEntity.Get<CameraRotationComponent>();
        // ref ChunksTransitionComponent chunksTransitionComponent = ref playerEntity.Get<ChunksTransitionComponent>();
        ref PlayerComponent playerComponent = ref playerEntity.Get<PlayerComponent>();
        playerEntity.Get<InputComponent>();


        var playerObject = GameObject.Instantiate(_playerData.PlayerPrefab, _sceneData.PlayerSpawnpoint.position, Quaternion.Euler(0, 0, 0));


        moveComponent.ForwardSpeed = _playerData.PlayerForwardSpeed;
        moveComponent.JumpSpeed = _playerData.PlayerJumpSpeed;

        moveComponent.JumpDuration = _playerData.PlayerJumpDuration;
        moveComponent.JumpCurve = _playerData.PlayerJumpCurve;
        moveComponent.HorizontalSpeed = _playerData.PlayerHorizontalSpeed;
        moveComponent.TargetLinePosition = playerObject.transform.position;
        moveComponent.TargetLineIndex = 0;
        moveComponent.Character = playerObject.GetComponent<CharacterController>();
        // Debug.Log(moveComponent.Character);
        moveComponent.GravityScale = _playerData.PlayerGravityScale;

        animationComponent.Animator = playerObject.GetComponentInChildren<Animator>();
        animationComponent.CanJump = false;
        animationComponent.IsGrounded = false;
        animationComponent.IsJumping = false;
        animationComponent.CurrentHp = (int)_playerData.PlayerHp.Value;

        // cameraRotationComponent.Camera = playerObject.transform.GetChild(0);
        // cameraRotationComponent.CameraParent = playerObject.transform;
        // cameraRotationComponent.XRotation = 0;
        // cameraRotationComponent.YRotation = 0;
        ECSStartup.player = playerObject.transform;

        // chunksTransitionComponent.rotationDirection = RotationDirection.Forward;

        playerComponent.Hp = _playerData.PlayerHp;
        playerComponent.currentHp = (int)playerComponent.Hp.Value;

        EcsEntity uiEntity = _world.NewEntity();
        ref UIComponent uiComponent = ref uiEntity.Get<UIComponent>();
        uiComponent.data = GameObject.Instantiate(_uiData.UIData);
        uiComponent.HpBar = new List<Image>();

        SceneData.currentRotation = 0;

        // Debug.Log(_playerData.PlayerJumpSpeed._startValue + "//" + _playerData.PlayerJumpSpeed.Value);
    }


}
