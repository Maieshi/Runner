using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPlatformData : AbstractObjectData
{
    public Transform LeftCorner;

    public Transform RightCorner;

    public List<Transform> LeftPath;

    public List<Transform> RightPath;

    public BuffModificator OnMiddleLineDamage;

    private void OnTriggerEnter(Collider other)
    {
        TransitionSystem.OnStartTransition.Invoke(this);
    }

}
