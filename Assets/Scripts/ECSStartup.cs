using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using Buff;
public class ECSStartup : MonoBehaviour
{
    EcsWorld _world = null;

    EcsSystems _systems = null;

    public static Transform player;

    public static MoveComponent playerMove;

    private SceneData _sceneData;

    private PlayerData _playerData;

    private SpawnData _spawnData;

    private BuffContainer _buffContainer;

    private UISpawnData _uiSpawnData;

    private InputData _inputData;

    private void Start()
    {
        // Debug.Log("start" + _world + _systems);
        _sceneData = GetComponent<SceneData>();
        _playerData = (PlayerData)Resources.Load("PlayerData");
        _spawnData = (SpawnData)Resources.Load("SpawnData");
        _buffContainer = (BuffContainer)Resources.Load("BuffContainer");
        _uiSpawnData = (UISpawnData)Resources.Load("UISpawnData");
        _inputData = (InputData)Resources.Load("InputData");
        if (_world == null)
            _world = new EcsWorld();

        if (_systems == null)
            _systems = new EcsSystems(_world);

        _systems.Add(new GameInitSystem())
                .Inject(_sceneData)
                .Inject(_playerData);
        _systems.Add(new InputSystem(), "InputSystem")
                .Inject(_inputData);
        _systems.Add(new MoveSystem(), "MoveSystem");
        _systems.Add(new LevelSpawnSystem(), "LevelSpawnSystem")
                .Inject(_spawnData)
                .Inject(_sceneData);
        _systems.Add(new TransitionSystem());
        _systems.Add(new MonitoringSystem())
               .Inject(_sceneData)
               .Inject(_systems);
        _systems.Add(new BuffSystem(), "BuffSystem")
                .Inject(_buffContainer);
        _systems.Add(new UISystem(), "UISystem")
                .Inject(_uiSpawnData);
        _systems.Add(new PlayerSystem(), "PlayerSystem");
        _systems.Add(new AnimationSystem(), "AnimationSystem");


        _systems.Init();

    }

    void Update()
    {
        _systems.Run();
    }

    void OnDestroy()
    {
        _systems.Destroy();
        _world.Destroy();
    }

}
