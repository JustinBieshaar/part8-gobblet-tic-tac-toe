        using System;
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        public class GameManager : MonoBehaviour {
            private static GameManager _instance;

            public static GameManager Instance =>
                _instance ? _instance : new GameObject("Game Manager").AddComponent<GameManager>();

            [SerializeField] private MarkerSelection _xSelection;
            [SerializeField] private MarkerSelection _oSelection;

            private int _rows;
            private int _turn;
            private int _match = 3;
            private bool _gameEnd = false;
            private bool _devMode = true;
            private Board _board;
            private Dictionary<string, HitBox> _fields = new Dictionary<string, HitBox>();
            private List<HitBox> _matchedPattern = new List<HitBox>();

            public int Turn => _turn % 2;
            public int Rows => _rows;
            public int Match => _match;
            public bool GameEnd => _gameEnd;
            public bool DevMode => _devMode;
            public Board Board => _board;
            public List<HitBox> Pattern => _matchedPattern;

            public Marker GetSelectedMarker => Turn == 0 ? _xSelection.SelectedMarker : _oSelection.SelectedMarker;

            private int _maxMoves => _rows * _rows;

            public event Action<bool, int> OnGameEnd;

            void Awake() {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                _devMode = false;
            }

            private void Start() {
                _xSelection.SetTurn(0);
                _oSelection.SetTurn(1);
            }

            public void Set(int rows, int match = 3) {
                _rows = rows;
                _match = match;
            }

            public void AddHitBox(HitBox hitBox, int x, int y) {
                // store hit box with x y row as key
                _fields.Add($"{x},{y}", hitBox);
            }

            public void Clear() {
                _fields.Clear();
                _matchedPattern?.Clear();
                _gameEnd = false;
                _turn = 0;
                OnGameEnd?.Invoke(_gameEnd, -1);
            }

            public void MoveMade() {
                _turn++;

                _matchedPattern = PatternFinder.CheckWin(_fields);
                if (_matchedPattern != null) {
                    // WINNER
                    _gameEnd = true;
                    OnGameEnd?.Invoke(_gameEnd, _matchedPattern[0].Type);
                }
                //else if (_turn >= _maxMoves) {
                //    // TIE
                //    _gameEnd = true;
                //    OnGameEnd?.Invoke(_gameEnd, -1);
                //}

                _xSelection.UpdateMarkers();
                _oSelection.UpdateMarkers();
            }

            public bool ToggleDevMode() {
                _devMode = !_devMode;
                return _devMode;
            }

            public void SetBoard(Board board) {
                _board = board;
            }

            public void Reset() {
                _board.Reset();
                _xSelection.Reset();
                _oSelection.Reset();
            }
        }