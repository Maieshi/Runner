using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using ECS.Component;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UISystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    EcsWorld _world;
    EcsFilter<PlayerComponent, MoveComponent> _playerFilter;

    private Dictionary<ObjectType, int> platFormdata;

    public static Action OnFinish;
    public static Action OnTransition;

    EcsFilter<UIComponent> _uiFilter;
    private UISpawnData _uiSpawnData;

    private int _currentPlatform;

    public void Init()
    {
        ref UIComponent _uIComponent = ref _uiFilter.Get1(0);
        OnFinish += Finish;
        OnTransition += Transition;
        platFormdata = new Dictionary<ObjectType, int>();
        ref PlayerComponent _playerComponent = ref _playerFilter.Get1(0);
        _uIComponent.data.PauseButton.onClick.AddListener(Pause);
        _uIComponent.data.ContnitueButton.onClick.AddListener(Continue);
        _uIComponent.data.RestartButton.onClick.AddListener(Restart);
        for (int i = (int)_playerComponent.Hp.Value; i > 0; i--)
        {
            _uIComponent.HpBar.Add(GameObject.Instantiate(_uiSpawnData.HPImage, _uIComponent.data.HpBar));
        }
    }


    public void Run()
    {
        ref PlayerComponent player = ref _playerFilter.Get1(0);
        ref UIComponent _uIComponent = ref _uiFilter.Get1(0);
        ref MoveComponent _moveComponent = ref _playerFilter.Get2(0);
        if (player.currentHp != player.Hp.Value)
        {
            if (player.currentHp < player.Hp.Value)
            {
                _uIComponent.HpBar.Add(GameObject.Instantiate(_uiSpawnData.HPImage, _uIComponent.data.HpBar));
            }
            else
            {
                Image obj = _uIComponent.HpBar[0];
                _uIComponent.HpBar.Remove(obj);
                GameObject.Destroy(obj.gameObject);
            }

        }

        PathNode node = LevelSpawnSystem._currentPathNode;

        if (_currentPlatform < node.Floors.Count)
        {
            float playerPathLength = Vector3.Magnitude(_moveComponent.Character.transform.position - node.Start.position);
            float floorPathLength = Vector3.Magnitude(node.Floors[_currentPlatform].End.position - node.Start.position);
            // Debug.Log(playerPathLength + "//" + floorPathLength + "//" + _currentPlatform);
            if (playerPathLength > floorPathLength)
            {
                if (node.Obstacles.ContainsKey(_currentPlatform))
                {
                    ObjectType type = node.Obstacles[_currentPlatform].Type;
                    if (!platFormdata.ContainsKey(type))
                    {
                        platFormdata.Add(type, 1);
                    }
                    else
                        platFormdata[type]++;

                }
                _currentPlatform++;
            }
            string stats = "Stats:\r\n";
            foreach (var data in platFormdata)
            {
                stats += $"{data.Key.ToString()} : {data.Value} \r\n";
            }
            _uIComponent.data.Stats.text = stats;
        }

    }



    private void Restart()
    {
        Time.timeScale = 1;
        // int y = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.UnloadSceneAsync(y);
        SceneManager.LoadScene("SampleScene");
    }

    private void Pause()
    {
        ref UIComponent _uIComponent = ref _uiFilter.Get1(0);
        _uIComponent.data.Panel.SetActive(true);
        // _uIComponent.data.ContnitueButton.gameObject.SetActive((_playerComponent.Hp.Value == 0) ? false : true);
        Time.timeScale = 0;
    }
    private void Finish()
    {
        ref UIComponent _uIComponent = ref _uiFilter.Get1(0);
        _uIComponent.data.Panel.SetActive(true);
        _uIComponent.data.ContnitueButton.gameObject.SetActive(false);
        _uIComponent.data.Win.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    private void Continue()
    {
        ref UIComponent _uIComponent = ref _uiFilter.Get1(0);
        _uIComponent.data.Panel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Transition()
    {
        _currentPlatform = 1;
    }

    public void Destroy()
    {
        OnFinish -= Finish;
        OnTransition -= Transition;
    }
}