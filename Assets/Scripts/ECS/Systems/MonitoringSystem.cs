using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
public class MonitoringSystem : IEcsInitSystem
{
    private EcsSystems _systems;
    private SceneData _sceneData;

    public static Action<string, bool> OnSetActiveSystem;
    public void Init()
    {
        OnSetActiveSystem += SetActiveSystem;
        foreach (var system in _sceneData.InitDeactiavatedSystems)
        {
            SetActiveSystem(system, false);
        }
    }

    void SetActiveSystem(string systemName, bool value)
    {
        var idx = _systems.GetNamedRunSystem(systemName);
        // Debug.Log(idx);
        _systems.SetRunSystemState(idx, value);
    }
}
