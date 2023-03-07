using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Configs;
using _Game.Scripts.Infrustructure;
using _Game.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


public interface ISceneController
{
    void MoveToCurrentLevel();

    void NextLevel();
}
public class SceneController : IInitializable, ISceneController, ITickable
{
    [Inject] private LevelEvents _levelEvents;
    [Inject] private ScenesConfig _scenesConfig;
    [Inject] private IPlayerDataController _dataController;
    [Inject] private LoadingScreen _loadingScreen;
    private Queue<AsyncOperation> _loadingOperations = new Queue<AsyncOperation>();

    private string _loadedLevel;
    public void Initialize()
    {
        MoveToCurrentLevel();
    }

    public void MoveToCurrentLevel()
    {
        if (!string.IsNullOrEmpty(_loadedLevel))
        {
            var unload = UnloadLevel(_loadedLevel);
            _loadingOperations.Enqueue(unload);
        }

        var loading = LoadLevel(CurrentLevel());
        _loadingOperations.Enqueue(loading);

        loading.completed += a =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(CurrentLevel()));
            _loadedLevel = CurrentLevel();
            _levelEvents.OnGameLoaded.Execute();
        };
        
        _loadingScreen.StartLoading(_loadingOperations);
    }

    private string CurrentLevel()
    {
        if (string.IsNullOrEmpty(_scenesConfig.overrideScene))
        {
            var currentLevel = _dataController.PlayerData.Value.level;
            return _scenesConfig.sceneNames[currentLevel % _scenesConfig.sceneNames.Length];
        }
        
        return _scenesConfig.overrideScene;
    }

    public void NextLevel()
    {
        _dataController.IncrementLevel();
        
        MoveToCurrentLevel();
    }

    private AsyncOperation LoadLevel(string name) => SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

    private AsyncOperation UnloadLevel(string name) => SceneManager.UnloadSceneAsync(name);

    public void Tick()
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_EDITOR_64
        if (Input.GetKeyDown(KeyCode.R))
        {
            MoveToCurrentLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
#endif
    }
}
