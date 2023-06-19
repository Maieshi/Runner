using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
public class AnimationSystem : IEcsRunSystem
{
    EcsFilter<AnimationComponent, MoveComponent, PlayerComponent, InputComponent> _animationFliter = null;


    public void Run()
    {
        foreach (var entity in _animationFliter)
        {
            // Debug.Log("aanim");
            ref AnimationComponent animationComponent = ref _animationFliter.Get1(entity);
            ref MoveComponent moveComponent = ref _animationFliter.Get2(entity);
            ref PlayerComponent playerComponent = ref _animationFliter.Get3(entity);
            ref InputComponent inputComponent = ref _animationFliter.Get4(entity);

            if (moveComponent.Character.isGrounded)
            {
                if (!animationComponent.IsGrounded)
                {
                    animationComponent.Animator.SetTrigger("Run");
                    animationComponent.IsGrounded = true;
                    // Debug.Log("Run");
                }
                if (animationComponent.CurrentHp > playerComponent.Hp.Value)
                {
                    animationComponent.Animator.SetTrigger("Damage");
                    animationComponent.CurrentHp = (int)playerComponent.Hp.Value;
                    // Debug.Log("Damage");
                }
            }
            if (inputComponent.Vectical == 0 && moveComponent.JumpsCount > 0)
            {
                animationComponent.CanJump = true;
            }
            if (animationComponent.CanJump && inputComponent.Vectical > 0)
            {
                animationComponent.IsGrounded = false;
                animationComponent.CanJump = false;
                if (moveComponent.JumpsCount == 0)
                {
                    animationComponent.Animator.SetTrigger("Double Jump");
                    // Debug.Log("Double Jump");
                }
                else
                {
                    animationComponent.Animator.SetTrigger("Jump");
                    // Debug.Log("First Jump");
                }
            }
        }
    }
}
