using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
namespace ECS.Component
{
    public struct PathNodeComponent
    {
        public PathNode Node;

    }
    public class PathNode
    {
        public string name;
        public Transform Start { get { return Floors[0].Root; } }

        public Transform End { get { return Floors[Floors.Count - 1].End; } }

        public TransitionPlatformData Transition;

        public List<FloorPrefabData> Floors;

        public Dictionary<int, ObstaclePrefabData> Obstacles;

        public List<BoosterData> Boosters;

        public PathNode Left;
        public PathNode Right;

        public PathNode Root;
        public PathNode(string name)
        {
            this.name = name;
            Floors = new List<FloorPrefabData>();
            Obstacles = new Dictionary<int, ObstaclePrefabData>();
            Boosters = new List<BoosterData>();
            // Debug.Log(name + "//");
        }


        // public PathNode(Transform StartPosition)
        // {
        //     this.Start = StartPosition;
        //     Floors = new List<FloorPrefabData>();
        //     Obstacles = new List<ObstaclePrefabData>();
        // }

        // public PathNode(FloorPrefabData StartPlatfrom)
        // {
        //     Floors = new List<FloorPrefabData>();
        //     Obstacles = new List<ObstaclePrefabData>();
        //     Start = StartPlatfrom.Root;
        //     End = StartPlatfrom.End;
        //     Floors.Add(StartPlatfrom);
        // }
    }
}
