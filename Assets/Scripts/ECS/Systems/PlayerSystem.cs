using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
public class PlayerSystem : IEcsRunSystem
{

    EcsFilter<PlayerComponent> _playerFilter;



    public void Run()
    {
        ref PlayerComponent player = ref _playerFilter.Get1(0);
        if (player.currentHp != player.Hp.Value)
        {
            if (player.Hp.Value == 0)
                Death();
            else
            {
                player.currentHp = (int)player.Hp.Value;
                // Debug.Log(player.currentHp + "\\\\" + player.Hp.Value);
            }
        }
    }

    private void Death()
    {
        ref PlayerComponent player = ref _playerFilter.Get1(0);
        player.Hp.ResetResultValue();
        UISystem.OnFinish.Invoke();
        // MonitoringSystem.OnSetActiveSystem.Invoke("MoveSystem", false);
        // MonitoringSystem.OnSetActiveSystem.Invoke("LevelSpawnSystem", false);
        // MonitoringSystem.OnSetActiveSystem.Invoke("BuffSystem", false);
    }
}