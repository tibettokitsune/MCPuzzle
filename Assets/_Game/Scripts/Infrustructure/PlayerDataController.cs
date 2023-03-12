using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrustructure
{

    [System.Serializable]
    public class PlayerData
    {
        public int level;

        public int money;

        public int skinId;
        public PlayerData()
        {
            
        }
    }
    public interface IPlayerDataController
    {
        ReactiveProperty<PlayerData> PlayerData { get; }

        void IncrementLevel();
        void IncrementMoney(int range);
    }
    
    public class PlayerDataController : IPlayerDataController, IInitializable, IDisposable
    {
        public ReactiveProperty<PlayerData> PlayerData { get; } = new ReactiveProperty<PlayerData>();
        public void IncrementLevel()
        {
            PlayerData.Value.level++;
            PlayerData.SetValueAndForceNotify(PlayerData.Value);
        }

        public void IncrementMoney(int range)
        {
            PlayerData.Value.money += range;
            PlayerData.SetValueAndForceNotify(PlayerData.Value);
        }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private const string DataKey = "PlayerData";
        private void Load()
        {
            var jsonData = PlayerPrefs.GetString(DataKey);
            if (string.IsNullOrEmpty(jsonData))
            {
                PlayerData.Value = new PlayerData();
            }
            else
            {
                PlayerData.Value = JsonUtility.FromJson<PlayerData>(jsonData);
            }
        }

        private void Save()
        {
            var jsonData = JsonUtility.ToJson(PlayerData.Value);
            PlayerPrefs.SetString(DataKey, jsonData);
        }

        public void Initialize()
        {
            Load();

            PlayerData.Subscribe(_ => Save()).AddTo(Disposable);
        }

        public void Dispose()
        {
            PlayerData?.Dispose();
            Disposable?.Dispose();
        }
    }
}