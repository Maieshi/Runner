using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using Buff;
public class BuffSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    public static Action<BuffModificator> OnSetModificator;

    public EcsWorld _world;

    private BuffContainer _container;

    private Dictionary<BuffModificator, BuffTarget> _buffMap;

    private EcsFilter<BuffComponent> _buffFilter;



    public void Init()
    {
        _buffMap = new Dictionary<BuffModificator, BuffTarget>();
        foreach (var buffPair in _container.data)
        {
            foreach (var modificator in buffPair.Modificators)
            {
                if (_buffMap.ContainsKey(modificator))
                {
                    _buffMap[modificator] = buffPair.Target;
                    Debug.LogError("BuffMapCollision");
                }
                else
                {
                    _buffMap.Add(modificator, buffPair.Target);
                }
            }

        }
        ResetModificators();
        OnSetModificator += SetModificator;
    }

    void ResetModificators()
    {
        foreach (var buff in _buffMap.Values)
        {
            buff.ResetResultValue();
        }
    }

    public void Run()
    {
        foreach (var entity in _buffFilter)
        {
            ref BuffComponent buff = ref _buffFilter.Get1(entity);
            buff.RemainedTime -= Time.deltaTime;
            if (buff.RemainedTime <= 0)
            {
                buff.Target.RemoveModificator(buff.Modificator);
                _buffFilter.GetEntity(entity).Destroy();
            }
        }

    }

    public void SetModificator(BuffModificator modificator)
    {
        bool containsBlocker = false;
        // Debug.Log(modificator.name);
        foreach (var entity in _buffFilter)
        {
            ref BuffComponent buff = ref _buffFilter.Get1(entity);
            if (buff.Target == _buffMap[modificator] && buff.Modificator.IsBlocker == true)
                containsBlocker = true;
        }
        if (!containsBlocker)
        {
            if (modificator.Type != ModificatorType.Permanent)
            {
                EcsEntity entity = _world.NewEntity();
                ref BuffComponent buff = ref entity.Get<BuffComponent>();
                buff.Modificator = modificator;
                buff.Target = _buffMap[modificator];
                buff.RemainedTime = modificator.Duration;
            }
            _buffMap[modificator].AddModificator(modificator);
        }
    }
    public void Destroy()
    {
        // Debug.Log("destroy");
        ResetModificators();
        OnSetModificator -= SetModificator;
    }
}
