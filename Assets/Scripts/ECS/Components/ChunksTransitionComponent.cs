using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Component
{
    public struct ChunksTransitionComponent
    {
        public RotationDirection rotationDirection;
    }

}
public enum RotationDirection
{
    Forward = 0,
    Left = 1,
    Back = 2,
    Right = 3
}