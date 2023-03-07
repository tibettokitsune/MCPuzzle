using UniRx;
using UnityEngine;

namespace _Game.Scripts.Infrustructure
{
    public class LevelEvents
    {
        public ReactiveCommand OnGameLoaded { get; } = new ReactiveCommand();
        public ReactiveCommand OnGameStart { get; } = new ReactiveCommand();
        public ReactiveCommand<bool> OnGameEnd { get; } = new ReactiveCommand<bool>();
        public ReactiveCommand<Vector3Int> OnPlayerMove { get; } = new ReactiveCommand<Vector3Int>();
        
    }
}