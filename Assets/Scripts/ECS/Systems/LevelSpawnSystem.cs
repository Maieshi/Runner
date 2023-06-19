using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using Ekstrand.Collections.Generic;
using System.Threading.Tasks;
public class LevelSpawnSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private SpawnData _spawnData;

    private SceneData _sceneData;

    private Transform _currentCorner;

    // private EcsEntity _currentPathNodeEntity;

    public static PathNode _currentPathNode;

    private FloorPrefabData _startPlatform;

    private EcsWorld _world;

    private ProbabilityMap _platformMap;

    private ProbabilityMap _obstacleMap;

    private ProbabilityMap _boosterMap;

    private TwoKeyDictionary<int, ObjectType, ObjectPool<AbstractObjectData>> _floorPool;

    private TwoKeyDictionary<int, ObjectType, ObjectPool<AbstractObjectData>> _obstaclePool;

    private TwoKeyDictionary<int, ObjectType, ObjectPool<AbstractObjectData>> _boosterPool;

    private ObjectPool<TransitionPlatformData> _transitionsPool;



    private int _currentTransitions;

    public static Action<bool> OnTransition;

    private EcsFilter<PlayerComponent, MoveComponent> playerFilter;


    private List<PathNode> _nodesToRemove;

    public void Init()
    {
        _currentTransitions = 0;
        _currentPathNode = null;

        _nodesToRemove = new List<PathNode>();
        OnTransition += Transition;
        // _currentPathNodeEntity = _world.NewEntity();

        // ref PathNodeComponent nodeComponent = ref _currentPathNodeEntity.Get<PathNodeComponent>();
        _startPlatform = GameObject.Instantiate(_spawnData.StartPlatform, _sceneData.PlatfromSpawnpoint.position, Quaternion.identity);
        // nodeComponent.Node = new PathNode();
        _transitionsPool = new ObjectPool<TransitionPlatformData>(
            () =>
            {
                return GameObject.Instantiate(_spawnData.TransitionPlatform);
            },
            (x) => { x.gameObject.SetActive(true); },
            (x) => { x.gameObject.SetActive(false); }

        );
        CreateMapAndDict(ref _floorPool, ref _platformMap, _spawnData.PlatformData);
        CreateMapAndDict(ref _obstaclePool, ref _obstacleMap, _spawnData.ObstacleData);
        CreateMapAndDict(ref _boosterPool, ref _boosterMap, _spawnData.BoosterData);

        // Debug.Log(_platfromsDictionary + "//");

        // CreatePath(_currentPathNode, startPlatform.End, (int)SceneData.currentRotation);
        CreateFork();
    }

    private void CreateMapAndDict(ref TwoKeyDictionary<int, ObjectType, ObjectPool<AbstractObjectData>> dict, ref ProbabilityMap map, List<ObjectData> data)
    {
        dict = new TwoKeyDictionary<int, ObjectType, ObjectPool<AbstractObjectData>>();
        map = new ProbabilityMap(0);
        float currentRange = 0;
        for (int i = 0; i < data.Count; i++)
        {
            int index = i;
            map.Add(new ObjectIndex(currentRange, currentRange + data[i].Probability, i));
            dict.Add(
                i,
                data[index].PrefabData.Type,
                new ObjectPool<AbstractObjectData>(
                    () =>
                    {

                        return GameObject.Instantiate(data[index].PrefabData);
                    },
                    (x) => { x.gameObject.SetActive(true); },
                    (x) => { x.gameObject.SetActive(false); }
                    )
                );

            currentRange += data[index].Probability;
        }
        map.MaxValue = currentRange;
    }

    public void CreatePath(PathNode node, Transform startPosition, float rotationDirection)
    {
        Transform currentPosition = startPosition;
        int platfromAmount = UnityEngine.Random.Range(_spawnData.MinPlatformAmount, _spawnData.MaxPlatformAmount);
        Quaternion platformRotation = Quaternion.Euler(0, rotationDirection, 0);
        for (int i = 0; i < platfromAmount; i++)
        {
            float randomValue = UnityEngine.Random.Range(0.001f, _platformMap.MaxValue);
            int platformIndex = _platformMap.Find(randomValue);
            FloorPrefabData platfrom = (FloorPrefabData)_floorPool[platformIndex].Get();
            platfrom.transform.position = currentPosition.position;
            platfrom.transform.rotation = platformRotation;
            node.Floors.Add(platfrom);
            currentPosition = platfrom.End;
        }
        // Debug.Log($" amount:{platfromAmount},count:{nodeComponent.Node.Floors.Count}");

        int platfromIndex = _spawnData.SpawnBorder;
        while (platfromIndex < platfromAmount - _spawnData.SpawnBorder)
        {
            if (UnityEngine.Random.Range(0, 100) < _spawnData.ObstacleSpawnProbability)
            {

                FloorPrefabData currentPlatform = node.Floors[platfromIndex];
                ObstaclePrefabData obstacle = (ObstaclePrefabData)_obstaclePool[_obstacleMap.Find(UnityEngine.Random.Range(0.001f, _obstacleMap.MaxValue))].Get();
                Transform spawnPoint = currentPlatform.SpawnPoint;
                obstacle.Root.position = spawnPoint.position + spawnPoint.right * _sceneData.DistanceBeetweenLines * ((obstacle.Size != ObstacleSize.Large) ? UnityEngine.Random.Range(-1, 2) : 0);
                obstacle.Root.rotation = Quaternion.Euler(
                    (obstacle.IsRotatesToNormal) ? spawnPoint.rotation.eulerAngles.x : 0,
                     rotationDirection,
                     (obstacle.IsRotatesToNormal) ? spawnPoint.rotation.eulerAngles.z : 0);
                node.Obstacles.Add(platfromIndex, obstacle);
                platfromIndex += _spawnData.MinObstaclesDistance;

            }
            else
            {
                platfromIndex++;
            }
        }
        platfromIndex = _spawnData.SpawnBorder;
        while (platfromIndex < platfromAmount - _spawnData.SpawnBorder)
        {
            if (UnityEngine.Random.Range(0, 100) < _spawnData.BoostSpawnProbability && !node.Obstacles.ContainsKey(platfromIndex))
            {
                FloorPrefabData currentPlatform = node.Floors[platfromIndex];
                BoosterData booster = (BoosterData)_boosterPool[_boosterMap.Find(UnityEngine.Random.Range(0.001f, _boosterMap.MaxValue))].Get();
                Transform spawnPoint = currentPlatform.SpawnPoint;
                booster.Root.position = spawnPoint.position + spawnPoint.right * _sceneData.DistanceBeetweenLines * UnityEngine.Random.Range(-1, 2);
                booster.Root.rotation = Quaternion.Euler(0, rotationDirection, 0);
                node.Boosters.Add(booster);
                platfromIndex += _spawnData.MinObstaclesDistance;

            }
            else
            {
                platfromIndex++;
            }
        }

        if (_currentTransitions + 1 < _spawnData.MaxTransitions)
        {
            TransitionPlatformData transition = _transitionsPool.Get();
            transition.Root.position = node.End.position;
            transition.Root.rotation = Quaternion.Euler(0, rotationDirection, 0);
            node.Transition = transition;
        }
        else
        {
            //spawn finish
            FloorPrefabData finish = GameObject.Instantiate(_spawnData.FinishPlatform);
            finish.Root.position = node.End.position;
            finish.Root.rotation = Quaternion.Euler(0, rotationDirection, 0);
        }
    }

    public void CreateFork()
    {
        if (_currentPathNode == null && _currentTransitions == 0)
        {
            _currentPathNode = new PathNode("root");
            CreatePath(_currentPathNode, _startPlatform.End, (int)SceneData.currentRotation);
        }
        PathNode leftNode = new PathNode(_currentPathNode.name + "/left");
        leftNode.Root = _currentPathNode;
        _currentPathNode.Left = leftNode;
        CreatePath(leftNode, _currentPathNode.Transition.LeftCorner, SceneData.currentRotation - 90);
        PathNode rightNode = new PathNode(_currentPathNode.name + "/right");
        rightNode.Root = _currentPathNode;
        _currentPathNode.Right = rightNode;
        CreatePath(rightNode, _currentPathNode.Transition.RightCorner, SceneData.currentRotation + 90);
    }


    public void RemoveUnnecessaryPlatform()
    {
        _nodesToRemove.Clear();
        GetNodes(_currentPathNode.Root, _currentPathNode);
        foreach (var node in _nodesToRemove)
        {
            // Debug.Log(node.name + "//" + node.Floors);
            foreach (var floor in node.Floors)
            {
                // Debug.Log(floor.Type);
                _floorPool[floor.Type].Release(floor);
            }
            node.Floors.Clear();
            node.Floors = null;
            foreach (var obstacle in node.Obstacles)
            {
                // Debug.Log(obstacle.Value.Type);
                _obstaclePool[obstacle.Value.Type].Release(obstacle.Value);
            }
            node.Obstacles.Clear();
            node.Obstacles = null;
            foreach (var booster in node.Boosters)
            {
                // Debug.Log(booster.Type);
                _boosterPool[booster.Type].Release(booster);
            }
            node.Boosters.Clear();
            node.Obstacles = null;
            _transitionsPool.Release(node?.Transition);
            if (node.Left != null)
                node.Left.Root = null;
            if (node.Right != null)
                node.Right.Root = null;
            node.Left = null;
            node.Right = null;
            node.Root = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        _nodesToRemove.Clear();
    }

    private void GetNodes(PathNode nodeToAdd, PathNode sender)
    {
        // Debug.Log($"Node to delete:{nodeToAdd?.name}, Left:{nodeToAdd?.Left?.name}, Right:{nodeToAdd?.Right?.name}, Root:{nodeToAdd?.Root?.name} ");
        _nodesToRemove.Add(nodeToAdd);



        if (nodeToAdd?.Left != sender && nodeToAdd.Left != null)

            GetNodes(nodeToAdd.Left, nodeToAdd);

        if (nodeToAdd?.Right != sender && nodeToAdd?.Right != null)

            GetNodes(nodeToAdd.Right, nodeToAdd);

        if (nodeToAdd?.Root != sender && nodeToAdd?.Root != null)

            GetNodes(nodeToAdd.Root, nodeToAdd);
    }
    void Transition(bool isRight)
    {
        _currentTransitions++;
        // Debug.Log(_currentPathNode.Right + "//" + _currentPathNode.Left);
        _currentPathNode = (isRight) ? _currentPathNode.Right : _currentPathNode.Left;

    }
    public void Run()
    {
        // Debug.Log(_currentPathNode);
        MoveComponent moveComponent = playerFilter.Get2(0);
        float pathLength = Vector3.Magnitude(_currentPathNode.End.position - _currentPathNode.Start.position);
        float playerPathLength = Vector3.Magnitude(moveComponent.Character.transform.position - _currentPathNode.Start.position);
        if (playerPathLength > pathLength / 2 && _currentPathNode.Left == null && _currentTransitions < _spawnData.MaxTransitions)
        {
            CreateFork();
            RemoveUnnecessaryPlatform();
            if (_startPlatform)
                GameObject.Destroy(_startPlatform?.gameObject);
        }
    }

    public void Destroy()
    {
        OnTransition -= Transition;
    }
}



public struct ProbabilityMap
{
    public List<ObjectIndex> _map;

    public float MaxValue;

    public ProbabilityMap(float maxValue)
    {
        _map = new List<ObjectIndex>();
        MaxValue = maxValue;
    }

    public void Add(ObjectIndex index)
    {
        _map.Add(index);
    }

    public int Find(float probability)
    {
        int rangeIndex = _map.FindIndex(x => x.Check(probability) != -1);
        return (rangeIndex == -1) ? -1 : rangeIndex;
    }
}

public struct ObjectIndex
{
    public ObjectRange Range;

    public int Index;

    public ObjectIndex(float min, float max, int index)
    {
        Range = new ObjectRange(min, max);
        Index = index;
    }
    public int Check(float probability)
    {
        return (Range.Contains(probability)) ? Index : -1;
    }
}
public struct ObjectRange
{
    public float _min;

    public float _max;

    public bool Contains(float value)
    {
        return value > _min && value <= _max;
    }

    public ObjectRange(float min, float max)
    {
        _min = min;
        _max = max;
    }
}


public class ObjectPool<T> where T : AbstractObjectData
{
    private Queue<T> _poolQueue;

    private Func<T> OnCreate;
    private Action<T> OnGet;
    private Action<T> OnRelease;

    public ObjectPool(Func<T> OnCreate, Action<T> OnGet = null, Action<T> OnRelease = null)
    {
        _poolQueue = new Queue<T>();
        this.OnCreate = OnCreate;
        this.OnGet = OnGet;
        this.OnRelease = OnRelease;
    }

    public T Get()
    {
        T result = null;
        if (_poolQueue.Count == 0) result = OnCreate.Invoke();
        else result = _poolQueue.Dequeue();
        OnGet.Invoke(result);
        return result;
    }
    public void Release(T t)
    {
        OnRelease.Invoke(t);
        _poolQueue.Enqueue(t);
    }
}


