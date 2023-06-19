using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leopotam.Ecs;
using ECS.Component;
public class ActivatedSystem : IEcsInitSystem, IEcsRunSystem
{
    protected bool IsActive;

    public Action<bool> OnSetActive;

    public void Init()
    {
        OnSetActive += (x) => { IsActive = x; };
        IsActive = true;
    }

    public virtual void Run()
    {
        if (!IsActive) return;
    }
}
