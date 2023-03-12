using _Game.Scripts.Infrustructure;
using _Game.Scripts.UI;
using UniRx;
using UnityEngine;
using Zenject;
using System;
using System.Linq;

namespace _Game.Scripts.Infrustructure
{
    public interface IInputController
    {
        public ReactiveCommand OnTap { get; }
        public ReactiveCommand OnTouch { get; }
        public ReactiveCommand<Vector2Int> OnSwipe { get; }
    }

    }
    
    public class InputController : IInputController, ITickable
    {
        [Inject] private UIInputBlocker[] _uiBlockers;
        public ReactiveCommand OnTap { get; } = new ReactiveCommand();
        public ReactiveCommand OnTouch { get; } = new ReactiveCommand();
        public ReactiveCommand<Vector2Int> OnSwipe { get; } = new ReactiveCommand<Vector2Int>();


        private const float TouchDelay = 0.3f;
        private bool _isDetection;
        private bool _isTouch;
        private bool _isSwipe;
        private Vector2 _swipeMousePosition;
        private float _startTouchTime;

        private Vector3 _startSwipeMousePosition;
        
        
        public void Tick()
        {
            if (Input.GetMouseButton(0))
            {
                if(_uiBlockers.Any(x => x.IsOverUIElement)) return;
                OnTap.Execute();
                
                if (!_isDetection)
                {
                    _isDetection = true;
                    _startTouchTime = Time.time;
                    _swipeMousePosition = Input.mousePosition;
                    _isTouch = false;
                    _isSwipe = false;
                }
                else
                {
                    if (Time.time - _startTouchTime > TouchDelay && !_isTouch && !_isSwipe)
                    {
                        OnTouch.Execute();
                        _isTouch = true;
                    }

                    if ((_swipeMousePosition - (Vector2)Input.mousePosition).magnitude > Screen.width / 10f && !_isSwipe && !_isTouch)
                    {
                        _isSwipe = true;
                        _startSwipeMousePosition = Input.mousePosition;
                    }
                }
            }
            else
            {
                _isDetection = false;
                _isTouch = false;
                if (_isSwipe)
                {
                    OnSwipe.Execute(SwipeDirection());
                }
                _isSwipe = false;
                
            }

            EditorInput();
        }

        private void EditorInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnSwipe.Execute(new Vector2Int(-1, 0));
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnSwipe.Execute(new Vector2Int(1, 0));
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnSwipe.Execute(new Vector2Int(0, 1));
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnSwipe.Execute(new Vector2Int(0, -1));
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnTouch.Execute();
            }
            
        }

        private Vector2Int SwipeDirection()
        {
            var horizontalDif =  Input.mousePosition.x - _startSwipeMousePosition.x;
            var verticalDif = Input.mousePosition.y - _startSwipeMousePosition.y;

            if (Mathf.Abs( horizontalDif) > Mathf.Abs(verticalDif)) return new Vector2Int(horizontalDif > 0? 1 : -1, 0);
            else return new Vector2Int(0, verticalDif > 0? 1 : -1);
        }
    }