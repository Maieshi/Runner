using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buff;
namespace ECS.Component
{
    public struct PlayerComponent
    {
        public BuffTarget Hp;

        public int currentHp;
    }
}