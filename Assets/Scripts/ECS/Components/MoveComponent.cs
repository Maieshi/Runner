using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buff;
namespace ECS.Component
{
    public struct MoveComponent
    {
        public CharacterController Character;
        // public Rigidbody Rigidbody;
        public BuffTarget ForwardSpeed;

        public int CurrentHorizontalDirection;

        public Vector3 TargetLinePosition;

        public int TargetLineIndex;

        public float TargetValue;

        public float GravityScale;

        public float HorizontalSpeed;

        public BuffTarget JumpSpeed;

        public int JumpsCount;

        public bool CanJump;

        public bool IsJumping;

        public AnimationCurve JumpCurve;

        public float JumpDuration;

        public float JumpStartTime;
    }
}