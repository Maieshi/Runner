using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Component
{
    public struct AnimationComponent
    {
        public Animator Animator;

        public bool IsGrounded;

        public bool CanJump;

        public bool IsJumping;
        public int CurrentHp;
    }
}