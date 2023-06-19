using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using DG.Tweening;
using System.Linq;
public class TransitionSystem : IEcsInitSystem, IEcsRunSystem
{
    public static Action<TransitionPlatformData> OnStartTransition;

    EcsFilter<MoveComponent, InputComponent> _moveFilter = null;

    TransitionPlatformData transition;
    public void Init()
    {
        OnStartTransition += StartTransition;
    }

    public void Run()
    {
        if (transition)
        {

            ref MoveComponent moveComponent = ref _moveFilter.Get1(0);
            Transform playerTransfrom = moveComponent.Character.transform;
            MonitoringSystem.OnSetActiveSystem("MoveSystem", false);
            MonitoringSystem.OnSetActiveSystem("LevelSpawnSystem", false);

            bool isOnMiddle = moveComponent.TargetLineIndex == 0;


            bool isRight = (moveComponent.TargetLineIndex >= 0) ? true : false;

            if (isOnMiddle)
            {
                moveComponent.TargetLineIndex = 1;
                BuffSystem.OnSetModificator(transition.OnMiddleLineDamage);
            }

            List<Vector3> path = new List<Vector3>() { playerTransfrom.position };
            List<Transform> points = (isRight) ? transition.RightPath : transition.LeftPath;
            transition = null;
            path.AddRange(points.Select(x => x.position));
            playerTransfrom.DOPath(path.ToArray(), 2, PathType.CatmullRom, PathMode.Ignore, 10);
            playerTransfrom.DORotate(new Vector3(0, playerTransfrom.rotation.eulerAngles.y + 90 * ((isRight) ? 1 : -1), 0), 2).OnComplete(() =>
            {
                LevelSpawnSystem.OnTransition.Invoke(((isRight) ? true : false));
                MoveSystem.OnEndTransition.Invoke();
                SceneData.currentRotation = playerTransfrom.rotation.eulerAngles.y;
                MonitoringSystem.OnSetActiveSystem("MoveSystem", true);
                MonitoringSystem.OnSetActiveSystem("LevelSpawnSystem", true);
                UISystem.OnTransition.Invoke();
            }
            );

        }
    }

    void StartTransition(TransitionPlatformData tr)
    {
        transition = tr;

    }

}
