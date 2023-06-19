using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Component
{
    public struct CameraRotationComponent
    {
        public Transform Camera;

        public Transform CameraParent;

        public float XRotation;

        public float YRotation;
    }
}