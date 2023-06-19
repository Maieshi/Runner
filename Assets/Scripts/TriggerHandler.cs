using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using Buff;
public class TriggerHandler : MonoBehaviour
{
    [SerializeField]
    private List<BuffModificator> modificators;
    [SerializeField]
    private bool UnactiveAfterImpact;

    void OnTriggerEnter(Collider other)
    {
        CharacterController controller;
        // Debug.Log(other.transform.name);
        if (other.transform.TryGetComponent<CharacterController>(out controller))
        {
            // Debug.Log(other.transform.name);
            foreach (var mod in modificators)
            {
                BuffSystem.OnSetModificator.Invoke(mod);
            }
            if (UnactiveAfterImpact)
                this.gameObject.SetActive(false);
        }
    }

}
