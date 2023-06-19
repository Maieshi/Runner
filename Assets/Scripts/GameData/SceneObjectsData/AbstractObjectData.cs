using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractObjectData : MonoBehaviour
{
    public Transform Root;
    public ObjectType Type;
}
public enum ObjectType
{
    //Floor
    CircleFloor,
    DeepFloor1,
    DeepFloor2,
    FlatFloor,
    HillFloor,
    RampDownFloor1,
    RampDownFloor2,
    RampUpFloor1,
    RampUpFloor2,
    TransitionFloor,


    //Obstacle
    Saw,
    BigMovingSaw,
    BigSpikes,
    SmallSpikes,
    Rotor,
    Pendulum,
    MovingRotor,

    //Booster
    Fuel,
    Heart,
    Rocket,
    Star
}